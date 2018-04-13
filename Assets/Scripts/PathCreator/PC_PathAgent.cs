using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreator
{
    public class PA_Visual
    {
        public Color pathColor = Color.blue;
        public Color inactivePathColor = Color.gray;
        public Color cubeColor = Color.white;
    }

    public class PA_Waypoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public Vector3 handlePrev = Vector3.back;
        public Vector3 handleNext = Vector3.forward;

        public PC_CurveType curveTypePosition = PC_CurveType.Linear;
        public AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public PC_CurveType curveTypeRotation = PC_CurveType.EaseInAndOut;
        public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


        public PA_Waypoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public class PC_PathAgent : MonoBehaviour
    {
        public PC_Path path;
        public List<PA_Waypoint> points;
        public PA_Visual visual;
        public float timeToDestination;

        private bool paused = false;
        private bool playing = false;
        private int currentWaypointIndex;
        private float currentTimeInWaypoint;
        private float timePerSegment;



        void Awake()
        {
            path.GetIndividualWaypoints();
        }

        // Use this for initialization
        void Start()
        {
            //foreach (var index in points)
            //{
            //    if (index.curveTypeRotation == PC_CurveType.EaseInAndOut)
            //        index.rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            //    if (index.curveTypeRotation == PC_CurveType.Linear)
            //        index.rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            //    if (index.curveTypePosition == PC_CurveType.EaseInAndOut)
            //        index.positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            //    if (index.curveTypePosition == PC_CurveType.Linear)
            //        index.positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
            //}

            PlayPath(timeToDestination);
        }

        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator FollowPath(float time)
        {
            UpdateTimeInSeconds(time);
            currentWaypointIndex = 0;
            while (currentWaypointIndex < points.Count)
            {
                currentTimeInWaypoint = 0;
                while (currentTimeInWaypoint < 1)
                {
                    if (!paused)
                    {
                        currentTimeInWaypoint += Time.deltaTime / timePerSegment;
                        RefreshTransform();
                    }
                    yield return 0;
                }
                currentWaypointIndex++;
            }
            StopPath();
        }

        /// <summary>
        /// When index/time are set while the path is not playing, this method will teleport the camera to the position/rotation specified
        /// </summary>
        public void RefreshTransform()
        {
            transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
            transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
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

        private Quaternion GetLerpRotation(int pointIndex, float time)
        {
            return Quaternion.LerpUnclamped(points[pointIndex].rotation, points[GetNextIndex(pointIndex)].rotation, points[pointIndex].rotationCurve.Evaluate(time));
        }

        /// <summary>
        /// Plays the path
        /// </summary>
        /// <param name="time">The time in seconds how long the camera takes for the entire path</param>
        public void PlayPath(float time)
        {
            if (time <= 0)
                time = 0.001f;
            paused = false;
            playing = true;
            StopAllCoroutines();
            StartCoroutine(FollowPath(time));
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
        /// Gets if the path is paused
        /// </summary>
        /// <returns>Returns paused state</returns>
        public bool IsPaused()
        {
            return paused;
        }

        /// <summary>
        /// Pauses the camera's movement - resumable with ResumePath()
        /// </summary>
        public void PausePath()
        {
            paused = true;
            playing = false;
        }

        /// <summary>
        /// Can be called after PausePath() to resume
        /// </summary>
        public void ResumePath()
        {
            if (paused)
                playing = true;
            paused = false;
        }

        /// <summary>
        /// Stops the path
        /// </summary>
        public void StopPath()
        {
            playing = false;
            paused = false;
            StopAllCoroutines();
        }

        /// <summary>
        /// Allows to change the time variable specified in PlayPath(float time) on the fly
        /// </summary>
        /// <param name="seconds">New time in seconds for entire path</param>
        public void UpdateTimeInSeconds(float seconds)
        {
            timePerSegment = seconds / (points.Count - 1);
        }

        /// <summary>
        /// Gets the index of the current waypoint
        /// </summary>
        /// <returns>Returns waypoint index</returns>
        public int GetCurrentWayPoint()
        {
            return currentWaypointIndex;
        }

        /// <summary>
        /// Gets the time within the current waypoint (Range is 0-1)
        /// </summary>
        /// <returns>Returns time of current waypoint (Range is 0-1)</returns>
        public float GetCurrentTimeInWaypoint()
        {
            return currentTimeInWaypoint;
        }

        public int GetNextIndex(int index)
        {
            if (index == points.Count - 1)
                return 0;
            return index + 1;
        }

        /// <summary>
        /// Sets the current waypoint index of the path
        /// </summary>
        /// <param name="value">Waypoint index</param>
        public void SetCurrentWayPoint(int value)
        {
            currentWaypointIndex = value;
        }

        /// <summary>
        /// Sets the time in the current waypoint 
        /// </summary>
        /// <param name="value">Waypoint time (Range is 0-1)</param>
        public void SetCurrentTimeInWaypoint(float value)
        {
            currentTimeInWaypoint = value;
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
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
#endif
    }
}
