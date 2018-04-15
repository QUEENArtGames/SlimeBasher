using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreator
{
    [System.Serializable]
    public class PC_Visual
    {
        public Color pathColor = Color.green;
        public Color inactivePathColor = Color.gray;
        public Color cylinderColor = Color.magenta;
        public Color handleColor = Color.yellow;
    }

    public enum PC_CurveType
    {
        EaseInAndOut,
        Linear,
        Custom
    }


    [System.Serializable]
    public class PC_Waypoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public float radius = 0.5f;

        public Vector3 handlePrev = Vector3.back;
        public Vector3 handleNext = Vector3.forward;
        public bool chained = true;

        public PC_CurveType curveTypePosition = PC_CurveType.Linear;
        public AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public PC_CurveType curveTypeRotation = PC_CurveType.EaseInAndOut;
        public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public PC_Waypoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }

        internal Vector3 GetRandomPosition()
        {
            return new Vector3(UnityEngine.Random.Range(position.x - radius, position.x + radius), 0, UnityEngine.Random.Range(position.z - radius, position.z + radius));
        }

        internal List<Vector3> GetSample()
        {
            var l = new List<Vector3>();
            l.Add(position);
            l.Add(position + Vector3.right * radius);
            l.Add(position + Vector3.left * radius);
            l.Add(position + Vector3.forward * radius);
            l.Add(position + Vector3.back * radius);
            return l;
        }
    }


    public class PC_Path : MonoBehaviour
    {
        public List<PC_Waypoint> points = new List<PC_Waypoint>();
        public PC_Visual visual;

        public bool alwaysShow = true;
        public bool showComplexPath = true;


        void Start()
        {

        }

        internal List<PA_Waypoint> GetRandomWaypoints()
        {
            var p = new List<PA_Waypoint>();
            foreach (var point in points)
            {
                p.Add(new PA_Waypoint(point));
            }
            return p;
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject == gameObject || alwaysShow)
            {
                if (points.Count >= 2)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (i < points.Count - 1)
                        {
                            var handleNext = points[i].handleNext;
                            var handlePrev = points[i + 1].handlePrev;

                            if (showComplexPath)
                            {
                                var indexSample = points[i].GetSample();
                                var indexNextSample = points[i + 1].GetSample();

                                foreach (var pos in indexSample)
                                {
                                    foreach (var posNext in indexNextSample)
                                    {
                                        UnityEditor.Handles.DrawBezier(pos, posNext, pos + handleNext, posNext + handlePrev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 3);
                                    }
                                }
                            }
                            else
                            {
                                var pos = points[i].position;
                                var posNext = points[i + 1].position;
                                UnityEditor.Handles.DrawBezier(pos, posNext, pos + handleNext, posNext + handlePrev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                            }
                        }
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    var index = points[i];
                    Gizmos.color = visual.cylinderColor;
                    Gizmos.DrawWireSphere(index.position, index.radius);
                }
            }
        }
#endif
    }
}
