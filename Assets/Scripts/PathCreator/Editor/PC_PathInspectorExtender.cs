using UnityEditor;
using UnityEngine;

namespace PathCreator
{
    [CustomEditor(typeof(PC_Path))]
    public class PC_PathInspectorExtender : Editor
    {

        private PC_Path path;
        private int selectedIndex = -1;


        private void OnEnable()
        {
            path = target as PC_Path;
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            if (path.points.Count >= 2)
            {
                for (int i = 0; i < path.points.Count; i++)
                {
                    DrawHandlesForWaypointIndex(i);
                    Handles.color = Color.white;
                }
            }
        }


        void SelectIndex(int index)
        {
            selectedIndex = index;
            //pointReorderableList.index = index;
            Repaint();
        }

        private void DrawHandlesForWaypointIndex(int index)
        {
            DrawHandleLinesForWaypointIndex(index);
            Handles.color = path.visual.handleColor;
            DrawNextHandleForWaypointIndex(index);
            DrawPreviousHandleForWaypointIndex(index);
            DrawWaypointHandlesForWaypointIndex(index);
            DrawSelectionHandlesForWaypointIndex(index);
        }

        private void DrawHandleLinesForWaypointIndex(int index)
        {
            Handles.color = path.visual.handleColor;

            if (index < path.points.Count - 1)
                Handles.DrawLine(path.points[index].position, path.points[index].position + path.points[index].handleNext);

            if (index > 0)
                Handles.DrawLine(path.points[index].position, path.points[index].position + path.points[index].handlePrev);

            Handles.color = Color.white;
        }

        private void DrawNextHandleForWaypointIndex(int index)
        {
            if (index < path.points.Count - 1)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 posNext = Vector3.zero;
                float size = HandleUtility.GetHandleSize(path.points[index].position + path.points[index].handleNext) * 0.1f;

                if (path.handleManipulationMode == PC_ManipulationModes.Free)
                {
                    posNext = Handles.FreeMoveHandle(path.points[index].position + path.points[index].handleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
                }
                else
                {
                    if (selectedIndex == index)
                    {
                        Handles.SphereHandleCap(0, path.points[index].position + path.points[index].handleNext, Quaternion.identity, size, EventType.Repaint);

                        posNext = Handles.PositionHandle(path.points[index].position + path.points[index].handleNext, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
                        if (Handles.Button(path.points[index].position + path.points[index].handleNext, Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(index);
                        }
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Handle Position");

                    path.points[index].handleNext = posNext - path.points[index].position;

                    path.points[index].handlePrev = path.points[index].handleNext * -1; // always chained behaviour
                }
            }
        }

        private void DrawPreviousHandleForWaypointIndex(int index)
        {
            if (index > 0)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 posPrev = Vector3.zero;
                float size = HandleUtility.GetHandleSize(path.points[index].position + path.points[index].handlePrev) * 0.1f;

                if (path.handleManipulationMode == PC_ManipulationModes.Free)
                {
                    posPrev = Handles.FreeMoveHandle(path.points[index].position + path.points[index].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(path.points[index].position + path.points[index].handlePrev), Vector3.zero, Handles.SphereHandleCap);
                }
                else
                {
                    if (selectedIndex == index)
                    {
                        Handles.SphereHandleCap(0, path.points[index].position + path.points[index].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(path.points[index].position + path.points[index].handleNext), EventType.Repaint);
                        posPrev = Handles.PositionHandle(path.points[index].position + path.points[index].handlePrev, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
                        if (Handles.Button(path.points[index].position + path.points[index].handlePrev, Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(index);
                        }
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Handle Position");

                    path.points[index].handlePrev = posPrev - path.points[index].position;

                    path.points[index].handleNext = path.points[index].handlePrev * -1; // always chained behaviour
                }
            }
        }

        private void DrawWaypointHandlesForWaypointIndex(int index)
        {
            if (Tools.current == Tool.Move)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 pos = Vector3.zero;
                if (path.waypointManipulationMode == PC_ManipulationModes.SelectAndTransform)
                {
                    if (index == selectedIndex)
                        pos = Handles.PositionHandle(path.points[index].position, Quaternion.identity);
                }
                else
                {
                    pos = Handles.FreeMoveHandle(path.points[index].position, Quaternion.identity, HandleUtility.GetHandleSize(path.points[index].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Moved Waypoint");

                    path.points[index].position = pos;
                }
            }
        }

        private void DrawSelectionHandlesForWaypointIndex(int index)
        {
            if (Event.current.button != 1 && selectedIndex != index)
            {
                if (path.waypointManipulationMode == PC_ManipulationModes.SelectAndTransform && Tools.current == Tool.Move)
                {
                    float size = HandleUtility.GetHandleSize(path.points[index].position) * 0.2f;

                    if (Handles.Button(path.points[index].position, Quaternion.identity, size, size, Handles.CubeHandleCap))
                    {
                        SelectIndex(index);
                    }
                }
            }
        }
    }
}
