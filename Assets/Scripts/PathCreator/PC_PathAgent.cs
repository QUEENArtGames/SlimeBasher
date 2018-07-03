// modified and extended version of: https://assetstore.unity.com/packages/tools/camera/camera-path-creator-84074

using System.Collections.Generic;
using UnityEngine;

namespace PathCreator
{
    [System.Serializable]
    public class PA_Visual
    {
        public Color pathColor = Color.blue;
        public Color inactivePathColor = Color.gray;
        public Color cubeColor = Color.white;
    }

    public class PA_Marker
    {
        public readonly int waypoint;
        public readonly float time;
        public readonly double distance;

        public PA_Marker(int waypoint, float time, double distance)
        {
            this.waypoint = waypoint;
            this.time = time;
            this.distance = distance;
        }
    }

    public class PA_Waypoint
    {
        public Vector3 position;

        public Vector3 handlePrev;
        public Vector3 handleNext;

        public PC_CurveType curveTypePosition = PC_CurveType.Linear;
        public AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public PC_CurveType curveTypeRotation = PC_CurveType.EaseInAndOut;
        public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


        public PA_Waypoint(PC_Waypoint point)
        {
            position = point.GetRandomPosition();

            handlePrev = point.handlePrev;
            handleNext = point.handleNext;
        }
    }

    public class PC_PathAgent : MonoBehaviour
    {
        public int precision = 100;
        public PA_Visual visual;
        public PC_Path path;

        public float maxV;
        public float acceleration;


        private bool playing = false;
        private bool slowed = false;
        private float slowDuration = 0;

        private double length = 0;
        private List<PA_Waypoint> points;
        private List<PA_Marker> positionList;
        private int lastPositionIndex = 0;

        private float originalMaxV;
        private double currentDistance = 0;
        private float currentV = 0;



        void Awake()
        {
            if (path)
            {
                points = path.GetRandomWaypoints();
                positionList = new List<PA_Marker>();

                CalculatePathLength(precision);
                originalMaxV = maxV;
            }
            else
            {
                Debug.LogError(gameObject.name + " hat keine Pfad zugewiesen bekommen.");
            }
        }

        // Use this for initialization
        void Start()
        {
            if (path)
            {
                PlayPath();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (playing)
            {
                if (slowed)
                {
                    if (maxV.Equals(originalMaxV))
                    {
                        maxV = 0.5f * originalMaxV;
                    }

                    slowDuration -= Time.deltaTime;

                    if (slowDuration <= 0)
                    {
                        slowDuration = 0;
                        slowed = false;
                        maxV = originalMaxV;
                    }
                }

                UpdateMovement();

                if (currentDistance.Equals(length))
                {
                    StopPath();
                    return;
                }

                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            currentDistance = GetDistanceAfterSeconds(Time.deltaTime);

            PA_Marker markerA = null;
            PA_Marker markerB = null;
            for (int i = lastPositionIndex; i < positionList.Count; i++)
            {
                if (positionList[i].distance < currentDistance)
                {
                    continue;
                }
                else if (positionList[i].distance.Equals(currentDistance))
                {
                    markerA = positionList[i];
                    break;
                }
                else if (positionList[i].distance > currentDistance)
                {
                    markerA = positionList[i - 1];
                    markerB = positionList[i];
                    break;
                }
            }

            int currentWaypointIndex = 0;
            float currentTimeInWaypoint = 0;
            if (markerA == null)
            {
                Debug.LogError(gameObject.name + " konnte den nächsten Marker vom Pfad nicht finden.");
                currentWaypointIndex = -1;
                currentTimeInWaypoint = 0;
                return;
            }

            if (markerB == null)
            {
                currentWaypointIndex = markerA.waypoint;
                currentTimeInWaypoint = markerA.time;
            }
            else
            {
                double markerDistance = markerB.distance - markerA.distance;
                double ownDistanceToA = currentDistance - markerA.distance;
                float percentage = (float) (ownDistanceToA / markerDistance);

                currentWaypointIndex = markerA.waypoint;
                currentTimeInWaypoint = markerA.time + (1.0f / precision) * percentage;
            }


            Vector3 lastPosition = transform.position;

            transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
            transform.forward = Vector3.Normalize(transform.position - lastPosition);

            lastPositionIndex = markerA.waypoint;
        }

        private void UpdateMovement()
        {
            if (currentV < maxV)
            {
                float possibleNewV = currentV + acceleration * Time.deltaTime;
                currentV = possibleNewV < maxV ? possibleNewV : maxV;
            }
            else if (currentV > maxV)
            {
                float possibleNewV = currentV - acceleration * Time.deltaTime;
                currentV = possibleNewV > maxV ? possibleNewV : maxV;
            }
        }

        private void CalculatePathLength(int precision)
        {
            Vector3 currentPosition;
            Vector3 previousPosition = points[0].position;

            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int t = 0; t <= precision; t++)
                {
                    currentPosition = GetBezierPosition(i, ((float) t) / 100);
                    length += Vector3.Distance(previousPosition, currentPosition);
                    positionList.Add(new PA_Marker(i, ((float) t) / 100, length));

                    previousPosition = currentPosition;
                }
            }
        }


        Vector3 GetBezierPosition(int pointIndex, float time)
        {
            float t = points[pointIndex].positionCurve.Evaluate(time);
            int nextIndex = GetNextIndex(pointIndex);
            return
                Vector3.Lerp(
                    Vector3.Lerp(
                        Vector3.Lerp(points[pointIndex].position,
                            points[pointIndex].position + points[pointIndex].handleNext, t),
                        Vector3.Lerp(points[pointIndex].position + points[pointIndex].handleNext,
                            points[nextIndex].position + points[nextIndex].handlePrev, t), t),
                    Vector3.Lerp(
                        Vector3.Lerp(points[pointIndex].position + points[pointIndex].handleNext,
                            points[nextIndex].position + points[nextIndex].handlePrev, t),
                        Vector3.Lerp(points[nextIndex].position + points[nextIndex].handlePrev,
                            points[nextIndex].position, t), t), t);
        }


        /// <summary>
        /// Plays the path
        /// </summary>
        public void PlayPath()
        {
            playing = true;
            currentDistance = 0;
            lastPositionIndex = 0;
            transform.position = points[0].position;
            maxV = originalMaxV;
        }

        /// <summary>
        /// Gets if the path is playing
        /// </summary>
        /// <returns>Returns playing state</returns>
        public bool IsPlaying()
        {
            return playing;
        }

        /// <summary>
        /// Can be called after PausePath() to resume
        /// </summary>
        public void ResumePath()
        {
            playing = true;
        }

        /// <summary>
        /// Stops the path
        /// </summary>
        public void StopPath()
        {
            playing = false;
        }

        public void Slow(float duration)
        {
            slowed = true;
            slowDuration = duration;
        }

        /// <summary>
        /// Gets the distance of the object to start
        /// </summary>
        /// <returns>Returns current distance</returns>
        public double GetCurrentDistance()
        {
            return currentDistance;
        }

        private double GetDistanceAfterSeconds(float time)
        {
            double possibleNewDistance = currentDistance + currentV * time;
            return possibleNewDistance < length ? possibleNewDistance : length;
        }

        public int GetNextIndex(int index)
        {
            if (index == points.Count - 1)
                return 0;
            return index + 1;
        }


#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                if (UnityEditor.Selection.activeGameObject == gameObject)
                {
                    if (points.Count >= 2)
                    {
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (i < points.Count - 1)
                            {
                                var index = points[i];
                                var indexNext = points[i + 1];
                                UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handleNext,
                                    indexNext.position + indexNext.handlePrev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                            }
                        }
                    }

                    for (int i = 0; i < points.Count; i++)
                    {
                        Gizmos.color = visual.cubeColor;
                        Gizmos.DrawCube(points[i].position, Vector3.one);
                    }
                }
            }
        }
#endif
    }
}
