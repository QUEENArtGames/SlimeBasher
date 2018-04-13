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
    }


    public class PC_Path : MonoBehaviour
    {
        public List<PC_Waypoint> points = new List<PC_Waypoint>();
        public PC_Visual visual;

        public bool alwaysShow = true;


        void Start()
        {

        }

        internal void GetIndividualWaypoints()
        {
            throw new NotImplementedException();
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
                            var index = points[i];
                            var indexNext = points[i + 1];
                            UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handleNext,
                                indexNext.position + indexNext.handlePrev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor), null, 5);
                        }
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    var index = points[i];
                    Gizmos.matrix = Matrix4x4.TRS(index.position, index.rotation, Vector3.one);
                    Gizmos.color = visual.cylinderColor;
                    Gizmos.DrawFrustum(Vector3.zero, 90f, 0.25f, 0.01f, 1.78f);
                    Gizmos.matrix = Matrix4x4.identity;
                }
            }
        }
#endif

    }
}
