using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace PathCreator
{
    public enum PC_ManipulationModes
    {
        Free,
        SelectAndTransform
    }

    public enum PC_NewWaypointMode
    {
        SceneCamera,
        LastWaypoint,
        WaypointIndex,
        WorldCenter
    }

    [CustomEditor(typeof(PC_Path))]
    public class PC_PathInspector : Editor
    {
        private PC_Path p;
        private ReorderableList pointReorderableList;


        //Editor variables
        private bool visualFoldout;
        private bool manipulationFoldout;
        private bool showRawValues;

        private PC_ManipulationModes waypointTranslateMode;
        private PC_ManipulationModes waypointRotationMode;
        private PC_ManipulationModes handlePositionMode;
        private PC_NewWaypointMode waypointAddMode;

        private int waypointIndex = 1;

        private PC_CurveType allCurveType = PC_CurveType.Linear;
        private AnimationCurve allAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);


        //GUIContents
        private GUIContent addPointContent = new GUIContent("Add Point", "Adds a waypoint into the scene ...");
        private GUIContent deletePointContent = new GUIContent("X", "Deletes this waypoint");
        private GUIContent gotoPointContent = new GUIContent("Goto", "Teleports the scene camera to the specified waypoint");
        private GUIContent alwaysShowContent = new GUIContent("Always show", "When true, shows the curve even when the GameObject is not selected - \"Inactive path color\" will be used as path color instead");
        private GUIContent chainedContent = new GUIContent("o───o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
        private GUIContent unchainedContent = new GUIContent("o─x─o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
        private GUIContent replaceAllPositionContent = new GUIContent("Replace all position lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint position lerp types with the specified values");
        private GUIContent replaceAllRotationContent = new GUIContent("Replace all rotation lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint rotation lerp types with the specified values");


        //Serialized Properties
        private SerializedObject serializedObjectTarget;
        private SerializedProperty visualPathProperty;
        private SerializedProperty visualInactivePathProperty;
        private SerializedProperty visualCylinderProperty;
        private SerializedProperty visualHandleProperty;
        private SerializedProperty alwaysShowProperty;

        private int selectedIndex = -1;

        private bool hasScrollBar = false;

        void OnEnable()
        {
            EditorApplication.update += Update;

            p = (PC_Path) target;
            if (p == null)
                return;

            SetupEditorVariables();
            GetVariableProperties();
            SetupReorderableList();
        }

        void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        void Update()
        {
            if (p == null)
                return;
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            serializedObjectTarget.Update();
            Rect scale = GUILayoutUtility.GetLastRect();
            hasScrollBar = (Screen.width - scale.width <= 12);
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
            DrawVisualDropdown();
            GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
            DrawManipulationDropdown();
            GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
            GUILayout.Space(10);
            DrawWaypointList();
            GUILayout.Space(10);
            DrawRawValues();
            serializedObjectTarget.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            if (p.points.Count >= 2)
            {
                for (int i = 0; i < p.points.Count; i++)
                {
                    DrawHandles(i);
                    Handles.color = Color.white;
                }
            }
        }

        void SelectIndex(int index)
        {
            selectedIndex = index;
            pointReorderableList.index = index;
            Repaint();
        }

        void SetupEditorVariables()
        {
            waypointTranslateMode = (PC_ManipulationModes) PlayerPrefs.GetInt("CPC_cameraTranslateMode", 1);
            waypointRotationMode = (PC_ManipulationModes) PlayerPrefs.GetInt("CPC_cameraRotationMode", 1);
            handlePositionMode = (PC_ManipulationModes) PlayerPrefs.GetInt("CPC_handlePositionMode", 0);
            waypointAddMode = (PC_NewWaypointMode) PlayerPrefs.GetInt("PC_waypointMode", 0);
        }

        void GetVariableProperties()
        {
            serializedObjectTarget = new SerializedObject(p);
            visualPathProperty = serializedObjectTarget.FindProperty("visual.pathColor");
            visualInactivePathProperty = serializedObjectTarget.FindProperty("visual.inactivePathColor");
            visualCylinderProperty = serializedObjectTarget.FindProperty("visual.cylinderColor");
            visualHandleProperty = serializedObjectTarget.FindProperty("visual.handleColor");
            alwaysShowProperty = serializedObjectTarget.FindProperty("alwaysShow");
        }

        void SetupReorderableList()
        {
            pointReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"), true, true, false, false);

            pointReorderableList.elementHeight *= 2;

            pointReorderableList.drawElementCallback = (rect, index, active, focused) => {
                if (index > p.points.Count - 1)
                    return;

                float startRectY = rect.y;
                float fullWidth = rect.width - 16 * (hasScrollBar ? 1 : 0);
                rect.height -= 2;
                rect.width = 40;
                fullWidth -= 40;
                rect.height /= 2;
                GUI.Label(rect, "#" + (index + 1));

                rect.y += rect.height - 3;
                rect.x -= 14;
                rect.width += 12;
                if (GUI.Button(rect, p.points[index].chained ? chainedContent : unchainedContent))
                {
                    Undo.RecordObject(p, "Changed chain type");
                    p.points[index].chained = !p.points[index].chained;
                }

                rect.x += rect.width + 2;
                rect.y = startRectY;
                rect.width = (fullWidth - 22) / 3 - 1;
                EditorGUI.BeginChangeCheck();
                PC_CurveType tempP = (PC_CurveType) EditorGUI.EnumPopup(rect, p.points[index].curveTypePosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p, "Changed enum value");
                    p.points[index].curveTypePosition = tempP;
                }

                rect.y += pointReorderableList.elementHeight / 2 - 4;
                EditorGUI.BeginChangeCheck();
                GUI.enabled = p.points[index].curveTypePosition == PC_CurveType.Custom;
                AnimationCurve tempACP = EditorGUI.CurveField(rect, p.points[index].positionCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p, "Changed position curve");
                    p.points[index].positionCurve = tempACP;
                }
                GUI.enabled = true;

                rect.x += rect.width + 2;
                rect.y = startRectY;
                rect.width = (fullWidth - 22) / 3 - 1;
                EditorGUI.BeginChangeCheck();
                PC_CurveType temp = (PC_CurveType) EditorGUI.EnumPopup(rect, p.points[index].curveTypeRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p, "Changed enum value");
                    p.points[index].curveTypeRotation = temp;
                }

                rect.y += pointReorderableList.elementHeight / 2 - 4;
                EditorGUI.BeginChangeCheck();
                GUI.enabled = p.points[index].curveTypeRotation == PC_CurveType.Custom;
                AnimationCurve tempAC = EditorGUI.CurveField(rect, p.points[index].rotationCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p, "Changed rotation curve");
                    p.points[index].rotationCurve = tempAC;
                }
                GUI.enabled = true;

                rect.y = startRectY;
                rect.height *= 2;
                rect.x += rect.width + 2;
                rect.width = (fullWidth - 22) / 3;
                if (GUI.Button(rect, gotoPointContent))
                {
                    pointReorderableList.index = index;
                    selectedIndex = index;
                    SceneView.lastActiveSceneView.pivot = p.points[pointReorderableList.index].position;
                    SceneView.lastActiveSceneView.size = 3;
                    SceneView.lastActiveSceneView.Repaint();
                }
                
                rect.y = startRectY;
                rect.x += rect.width + 2;
                rect.width = 20;
                if (GUI.Button(rect, deletePointContent))
                {
                    Undo.RecordObject(p, "Deleted a waypoint");
                    p.points.Remove(p.points[index]);
                    SceneView.RepaintAll();
                }
            };

            pointReorderableList.drawHeaderCallback = rect => {
                float fullWidth = rect.width;
                rect.width = 56;
                GUI.Label(rect, "Sum: " + p.points.Count);
                rect.x += rect.width;
                rect.width = (fullWidth - 78) / 3;
                GUI.Label(rect, "Position Lerp");
                rect.x += rect.width;
                GUI.Label(rect, "Rotation Lerp");
            };

            pointReorderableList.onSelectCallback = l => {
                selectedIndex = l.index;
                SceneView.RepaintAll();
            };
        }

        void DrawVisualDropdown()
        {
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            visualFoldout = EditorGUILayout.Foldout(visualFoldout, "Visual");
            alwaysShowProperty.boolValue = GUILayout.Toggle(alwaysShowProperty.boolValue, alwaysShowContent);
            GUILayout.EndHorizontal();
            if (visualFoldout)
            {
                GUILayout.BeginVertical("Box");
                visualPathProperty.colorValue = EditorGUILayout.ColorField("Path color", visualPathProperty.colorValue);
                visualInactivePathProperty.colorValue = EditorGUILayout.ColorField("Inactive path color", visualInactivePathProperty.colorValue);
                visualCylinderProperty.colorValue = EditorGUILayout.ColorField("Frustum color", visualCylinderProperty.colorValue);
                visualHandleProperty.colorValue = EditorGUILayout.ColorField("Handle color", visualHandleProperty.colorValue);
                if (GUILayout.Button("Default colors"))
                {
                    Undo.RecordObject(p, "Reset to default color values");
                    p.visual = new PC_Visual();
                }
                GUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        void DrawManipulationDropdown()
        {
            manipulationFoldout = EditorGUILayout.Foldout(manipulationFoldout, "Transform manipulation modes");
            EditorGUI.BeginChangeCheck();
            if (manipulationFoldout)
            {
                GUILayout.BeginVertical("Box");
                waypointTranslateMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Waypoint Translation", waypointTranslateMode);
                waypointRotationMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Waypoint Rotation", waypointRotationMode);
                handlePositionMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Handle Translation", handlePositionMode);
                GUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                PlayerPrefs.SetInt("CPC_cameraTranslateMode", (int) waypointTranslateMode);
                PlayerPrefs.SetInt("CPC_cameraRotationMode", (int) waypointRotationMode);
                PlayerPrefs.SetInt("CPC_handlePositionMode", (int) handlePositionMode);
                SceneView.RepaintAll();
            }
        }

        void DrawWaypointList()
        {
            GUILayout.Label("Replace all lerp types");
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            allCurveType = (PC_CurveType) EditorGUILayout.EnumPopup(allCurveType, GUILayout.Width(Screen.width / 3f));
            if (GUILayout.Button(replaceAllPositionContent))
            {
                Undo.RecordObject(p, "Applied new position");
                foreach (var index in p.points)
                {
                    index.curveTypePosition = allCurveType;
                    if (allCurveType == PC_CurveType.Custom)
                        index.positionCurve.keys = allAnimationCurve.keys;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.enabled = allCurveType == PC_CurveType.Custom;
            allAnimationCurve = EditorGUILayout.CurveField(allAnimationCurve, GUILayout.Width(Screen.width / 3f));
            GUI.enabled = true;
            if (GUILayout.Button(replaceAllRotationContent))
            {
                Undo.RecordObject(p, "Applied new rotation");
                foreach (var index in p.points)
                {
                    index.curveTypeRotation = allCurveType;
                    if (allCurveType == PC_CurveType.Custom)
                        index.rotationCurve = allAnimationCurve;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width / 2f - 20);
            GUILayout.Label("↓");
            GUILayout.EndHorizontal();
            serializedObject.Update();
            pointReorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            Rect r = GUILayoutUtility.GetRect(Screen.width - 16, 18);
            //r.height = 18;
            r.y -= 10;
            GUILayout.Space(-30);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(addPointContent))
            {
                Undo.RecordObject(p, "Added camera path point");
                switch (waypointAddMode)
                {
                    case PC_NewWaypointMode.SceneCamera:
                        p.points.Add(new PC_Waypoint(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation));
                        break;
                    case PC_NewWaypointMode.LastWaypoint:
                        if (p.points.Count > 0)
                            p.points.Add(new PC_Waypoint(p.points[p.points.Count - 1].position, p.points[p.points.Count - 1].rotation) { handleNext = p.points[p.points.Count - 1].handleNext, handlePrev = p.points[p.points.Count - 1].handlePrev });
                        else
                        {
                            p.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                            Debug.LogWarning("No previous waypoint found to place this waypoint, defaulting position to world center");
                        }
                        break;
                    case PC_NewWaypointMode.WaypointIndex:
                        if (p.points.Count > waypointIndex - 1 && waypointIndex > 0)
                            p.points.Add(new PC_Waypoint(p.points[waypointIndex - 1].position, p.points[waypointIndex - 1].rotation) { handleNext = p.points[waypointIndex - 1].handleNext, handlePrev = p.points[waypointIndex - 1].handlePrev });
                        else
                        {
                            p.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                            Debug.LogWarning("Waypoint index " + waypointIndex + " does not exist, defaulting position to world center");
                        }
                        break;
                    case PC_NewWaypointMode.WorldCenter:
                        p.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                selectedIndex = p.points.Count - 1;
                SceneView.RepaintAll();
            }
            GUILayout.Label("at", GUILayout.Width(20));
            EditorGUI.BeginChangeCheck();
            waypointAddMode = (PC_NewWaypointMode) EditorGUILayout.EnumPopup(waypointAddMode, waypointAddMode == PC_NewWaypointMode.WaypointIndex ? GUILayout.Width(Screen.width / 4) : GUILayout.Width(Screen.width / 2));
            if (waypointAddMode == PC_NewWaypointMode.WaypointIndex)
            {
                waypointIndex = EditorGUILayout.IntField(waypointIndex, GUILayout.Width(Screen.width / 4));
            }
            if (EditorGUI.EndChangeCheck())
            {
                PlayerPrefs.SetInt("PC_waypointMode", (int) waypointAddMode);
            }
            GUILayout.EndHorizontal();
        }

        void DrawHandles(int i)
        {
            DrawHandleLines(i);
            Handles.color = p.visual.handleColor;
            DrawNextHandle(i);
            DrawPrevHandle(i);
            DrawWaypointHandles(i);
            DrawSelectionHandles(i);
        }

        void DrawHandleLines(int i)
        {
            Handles.color = p.visual.handleColor;
            if (i < p.points.Count - 1 || p.looped == true)
                Handles.DrawLine(p.points[i].position, p.points[i].position + p.points[i].handleNext);
            if (i > 0 || p.looped == true)
                Handles.DrawLine(p.points[i].position, p.points[i].position + p.points[i].handlePrev);
            Handles.color = Color.white;
        }

        void DrawNextHandle(int i)
        {
            if (i < p.points.Count - 1 || loopedProperty.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 posNext = Vector3.zero;
                float size = HandleUtility.GetHandleSize(p.points[i].position + p.points[i].handleNext) * 0.1f;
                if (handlePositionMode == PC_ManipulationModes.Free)
                {
#if UNITY_5_5_OR_NEWER
                    posNext = Handles.FreeMoveHandle(p.points[i].position + p.points[i].handleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
#else
                posNext = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereCap);
#endif
                }
                else
                {
                    if (selectedIndex == i)
                    {
#if UNITY_5_5_OR_NEWER
                        Handles.SphereHandleCap(0, p.points[i].position + p.points[i].handleNext, Quaternion.identity, size, EventType.Repaint);
#else
                    Handles.SphereCap(0, t.points[i].position + t.points[i].handleNext, Quaternion.identity, size);
#endif
                        posNext = Handles.PositionHandle(p.points[i].position + p.points[i].handleNext, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
#if UNITY_5_5_OR_NEWER
                        if (Handles.Button(p.points[i].position + p.points[i].handleNext, Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(i);
                        }
#else
                    if (Handles.Button(t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, size, Handles.CubeCap))
                    {
                        SelectIndex(i);
                    }
#endif
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Handle Position");
                    p.points[i].handleNext = posNext - p.points[i].position;
                    if (p.points[i].chained)
                        p.points[i].handlePrev = p.points[i].handleNext * -1;
                }
            }

        }

        void DrawPrevHandle(int i)
        {
            if (i > 0 || loopedProperty.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 posPrev = Vector3.zero;
                float size = HandleUtility.GetHandleSize(p.points[i].position + p.points[i].handlePrev) * 0.1f;
                if (handlePositionMode == PC_ManipulationModes.Free)
                {
#if UNITY_5_5_OR_NEWER
                    posPrev = Handles.FreeMoveHandle(p.points[i].position + p.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(p.points[i].position + p.points[i].handlePrev), Vector3.zero, Handles.SphereHandleCap);
#else
                posPrev = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handlePrev), Vector3.zero, Handles.SphereCap);
#endif
                }
                else
                {
                    if (selectedIndex == i)
                    {
#if UNITY_5_5_OR_NEWER
                        Handles.SphereHandleCap(0, p.points[i].position + p.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(p.points[i].position + p.points[i].handleNext), EventType.Repaint);
#else
                    Handles.SphereCap(0, t.points[i].position + t.points[i].handlePrev, Quaternion.identity,
                        0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handleNext));
#endif
                        posPrev = Handles.PositionHandle(p.points[i].position + p.points[i].handlePrev, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
#if UNITY_5_5_OR_NEWER
                        if (Handles.Button(p.points[i].position + p.points[i].handlePrev, Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(i);
                        }
#else
                    if (Handles.Button(t.points[i].position + t.points[i].handlePrev, Quaternion.identity, size, size,
                        Handles.CubeCap))
                    {
                        SelectIndex(i);
                    }
#endif
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Handle Position");
                    p.points[i].handlePrev = posPrev - p.points[i].position;
                    if (p.points[i].chained)
                        p.points[i].handleNext = p.points[i].handlePrev * -1;
                }
            }
        }

        void DrawWaypointHandles(int i)
        {
            if (Tools.current == Tool.Move)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 pos = Vector3.zero;
                if (waypointTranslateMode == PC_ManipulationModes.SelectAndTransform)
                {
                    if (i == selectedIndex)
                        pos = Handles.PositionHandle(p.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? p.points[i].rotation : Quaternion.identity);
                }
                else
                {
#if UNITY_5_5_OR_NEWER
                    pos = Handles.FreeMoveHandle(p.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? p.points[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(p.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);
#else
                pos = Handles.FreeMoveHandle(t.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.points[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(t.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleCap);
#endif
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Moved Waypoint");
                    p.points[i].position = pos;
                }
            }
            else if (Tools.current == Tool.Rotate)
            {

                EditorGUI.BeginChangeCheck();
                Quaternion rot = Quaternion.identity;
                if (waypointRotationMode == PC_ManipulationModes.SelectAndTransform)
                {
                    if (i == selectedIndex)
                        rot = Handles.RotationHandle(p.points[i].rotation, p.points[i].position);
                }
                else
                {
                    rot = Handles.FreeRotateHandle(p.points[i].rotation, p.points[i].position, HandleUtility.GetHandleSize(p.points[i].position) * 0.2f);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Rotated Waypoint");
                    p.points[i].rotation = rot;
                }
            }
        }

        void DrawSelectionHandles(int i)
        {
            if (Event.current.button != 1 && selectedIndex != i)
            {
                if (waypointTranslateMode == PC_ManipulationModes.SelectAndTransform && Tools.current == Tool.Move
                    || waypointRotationMode == PC_ManipulationModes.SelectAndTransform && Tools.current == Tool.Rotate)
                {
                    float size = HandleUtility.GetHandleSize(p.points[i].position) * 0.2f;
#if UNITY_5_5_OR_NEWER
                    if (Handles.Button(p.points[i].position, Quaternion.identity, size, size, Handles.CubeHandleCap))
                    {
                        SelectIndex(i);
                    }
#else
                if (Handles.Button(t.points[i].position, Quaternion.identity, size, size, Handles.CubeCap))
                {
                    SelectIndex(i);
                }
#endif
                }
            }
        }

        void DrawRawValues()
        {
            if (GUILayout.Button(showRawValues ? "Hide raw values" : "Show raw values"))
                showRawValues = !showRawValues;

            if (showRawValues)
            {
                foreach (var i in p.points)
                {
                    EditorGUI.BeginChangeCheck();
                    GUILayout.BeginVertical("Box");
                    Vector3 pos = EditorGUILayout.Vector3Field("Waypoint Position", i.position);
                    Quaternion rot = Quaternion.Euler(EditorGUILayout.Vector3Field("Waypoint Rotation", i.rotation.eulerAngles));
                    Vector3 posp = EditorGUILayout.Vector3Field("Previous Handle Offset", i.handlePrev);
                    Vector3 posn = EditorGUILayout.Vector3Field("Next Handle Offset", i.handleNext);
                    GUILayout.EndVertical();
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(p, "Changed waypoint transform");
                        i.position = pos;
                        i.rotation = rot;
                        i.handlePrev = posp;
                        i.handleNext = posn;
                        SceneView.RepaintAll();
                    }
                }
            }
        }
    }
}
