using PathCreator;
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Pathcreator
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

    [CustomEditor(typeof(PC_NewWaypointMode))] // eigentlich PC_Path aber um Fehlermeldungen auszublenden etwas anderes bis gefixt
    public class PC_PathInspector : Editor
    {
        private PC_Path t;
        private ReorderableList pointReorderableList;

        //Editor variables
        private bool visualFoldout;
        private bool manipulationFoldout;
        private bool showRawValues;
        private float time;
        private PC_ManipulationModes cameraTranslateMode;
        private PC_ManipulationModes cameraRotationMode;
        private PC_ManipulationModes handlePositionMode;
        private PC_NewWaypointMode waypointMode;
        private int waypointIndex = 1;
        private PC_CurveType allCurveType = PC_CurveType.Custom;
        private AnimationCurve allAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        //GUIContents
        private GUIContent addPointContent = new GUIContent("Add Point", "Adds a waypoint at the scene view camera's position/rotation");
        private GUIContent testButtonContent = new GUIContent("Test", "Only available in play mode");
        private GUIContent deletePointContent = new GUIContent("X", "Deletes this waypoint");
        private GUIContent alwaysShowContent = new GUIContent("Always show", "When true, shows the curve even when the GameObject is not selected - \"Inactive cath color\" will be used as path color instead");
        private GUIContent chainedContent = new GUIContent("o───o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
        private GUIContent unchainedContent = new GUIContent("o─x─o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");

        //Serialized Properties
        private SerializedObject serializedObjectTarget;
        private SerializedProperty visualPathProperty;
        private SerializedProperty visualInactivePathProperty;
        private SerializedProperty visualFrustumProperty;
        private SerializedProperty visualHandleProperty;
        private SerializedProperty alwaysShowProperty;

        private int selectedIndex = -1;

        private bool hasScrollBar = false;

        void OnEnable()
        {
            EditorApplication.update += Update;

            t = (PC_Path) target;
            if (t == null)
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
            if (t == null)
                return;
        }

        public override void OnInspectorGUI()
        {
            serializedObjectTarget.Update();
            Rect scale = GUILayoutUtility.GetLastRect();
            hasScrollBar = (Screen.width - scale.width <= 12);
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
            GUILayout.Space(5);
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
            if (t.points.Count >= 2)
            {
                for (int i = 0; i < t.points.Count; i++)
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
            cameraTranslateMode = (PC_ManipulationModes) PlayerPrefs.GetInt("PC_cameraTranslateMode", 1);
            cameraRotationMode = (PC_ManipulationModes) PlayerPrefs.GetInt("PC_cameraRotationMode", 1);
            handlePositionMode = (PC_ManipulationModes) PlayerPrefs.GetInt("PC_handlePositionMode", 0);
            waypointMode = (PC_NewWaypointMode) PlayerPrefs.GetInt("PC_waypointMode", 0);
            time = PlayerPrefs.GetFloat("PC_time", 10);
        }

        void GetVariableProperties()
        {
            serializedObjectTarget = new SerializedObject(t);
            visualPathProperty = serializedObjectTarget.FindProperty("visual.pathColor");
            visualInactivePathProperty = serializedObjectTarget.FindProperty("visual.inactivePathColor");
            visualFrustumProperty = serializedObjectTarget.FindProperty("visual.frustrumColor");
            visualHandleProperty = serializedObjectTarget.FindProperty("visual.handleColor");
            alwaysShowProperty = serializedObjectTarget.FindProperty("alwaysShow");
        }

        void SetupReorderableList()
        {
            pointReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"), true, true, false, false);

            pointReorderableList.elementHeight *= 2;

            pointReorderableList.drawElementCallback = (rect, index, active, focused) => {
                float startRectY = rect.y;
                if (index > t.points.Count - 1)
                    return;
                rect.height -= 2;
                float fullWidth = rect.width - 16 * (hasScrollBar ? 1 : 0);
                rect.width = 40;
                fullWidth -= 40;
                rect.height /= 2;
                GUI.Label(rect, "#" + (index + 1));
                rect.y += rect.height - 3;
                rect.x -= 14;
                rect.width += 12;
                if (GUI.Button(rect, t.points[index].chained ? chainedContent : unchainedContent))
                {
                    Undo.RecordObject(t, "Changed chain type");
                    t.points[index].chained = !t.points[index].chained;
                }
                rect.x += rect.width + 2;
                rect.y = startRectY;
                //Position
                rect.width = (fullWidth - 22) / 3 - 1;
                EditorGUI.BeginChangeCheck();
                PC_CurveType tempP = (PC_CurveType) EditorGUI.EnumPopup(rect, t.points[index].curveTypePosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed enum value");
                    t.points[index].curveTypePosition = tempP;
                }
                rect.y += pointReorderableList.elementHeight / 2 - 4;
                //rect.x += rect.width + 2;
                EditorGUI.BeginChangeCheck();
                GUI.enabled = t.points[index].curveTypePosition == PC_CurveType.Custom;
                AnimationCurve tempACP = EditorGUI.CurveField(rect, t.points[index].positionCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed curve");
                    t.points[index].positionCurve = tempACP;
                }
                GUI.enabled = true;
                rect.x += rect.width + 2;
                rect.y = startRectY;

                //Rotation

                rect.width = (fullWidth - 22) / 3 - 1;
                EditorGUI.BeginChangeCheck();
                PC_CurveType temp = (PC_CurveType) EditorGUI.EnumPopup(rect, t.points[index].curveTypeRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed enum value");
                    t.points[index].curveTypeRotation = temp;
                }
                rect.y += pointReorderableList.elementHeight / 2 - 4;
                //rect.height /= 2;
                //rect.x += rect.width + 2;
                EditorGUI.BeginChangeCheck();
                GUI.enabled = t.points[index].curveTypeRotation == PC_CurveType.Custom;
                AnimationCurve tempAC = EditorGUI.CurveField(rect, t.points[index].rotationCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed curve");
                    t.points[index].rotationCurve = tempAC;
                }
                GUI.enabled = true;

                rect.y = startRectY;
                rect.height *= 2;
                rect.x += rect.width + 2;
                rect.width = (fullWidth - 22) / 3;
                rect.height = rect.height / 2 - 1;
                rect.height = (rect.height + 1) * 2;
                rect.y = startRectY;
                rect.x += rect.width + 2;
                rect.width = 20;

                if (GUI.Button(rect, deletePointContent))
                {
                    Undo.RecordObject(t, "Deleted a waypoint");
                    t.points.Remove(t.points[index]);
                    SceneView.RepaintAll();
                }
            };

            pointReorderableList.drawHeaderCallback = rect => {
                float fullWidth = rect.width;
                rect.width = 56;
                GUI.Label(rect, "Sum: " + t.points.Count);
                rect.x += rect.width;
                rect.width = (fullWidth - 78) / 3;
                GUI.Label(rect, "Position Lerp");
                rect.x += rect.width;
                GUI.Label(rect, "Rotation Lerp");
                //rect.x += rect.width*2;
                //GUI.Label(rect, "Del.");
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
                visualFrustumProperty.colorValue = EditorGUILayout.ColorField("Frustum color", visualFrustumProperty.colorValue);
                visualHandleProperty.colorValue = EditorGUILayout.ColorField("Handle color", visualHandleProperty.colorValue);
                if (GUILayout.Button("Default colors"))
                {
                    Undo.RecordObject(t, "Reset to default color values");
                    t.visual = new PC_Visual();
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
                cameraTranslateMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Waypoint Translation", cameraTranslateMode);
                cameraRotationMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Waypoint Rotation", cameraRotationMode);
                handlePositionMode = (PC_ManipulationModes) EditorGUILayout.EnumPopup("Handle Translation", handlePositionMode);
                GUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                PlayerPrefs.SetInt("PC_cameraTranslateMode", (int) cameraTranslateMode);
                PlayerPrefs.SetInt("PC_cameraRotationMode", (int) cameraRotationMode);
                PlayerPrefs.SetInt("PC_handlePositionMode", (int) handlePositionMode);
                SceneView.RepaintAll();
            }
        }

        void DrawWaypointList()
        {
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
                Undo.RecordObject(t, "Added camera path point");
                switch (waypointMode)
                {
                    case PC_NewWaypointMode.SceneCamera:
                        t.points.Add(new PC_Waypoint(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation));
                        break;
                    case PC_NewWaypointMode.LastWaypoint:
                        if (t.points.Count > 0)
                            t.points.Add(new PC_Waypoint(t.points[t.points.Count - 1].position, t.points[t.points.Count - 1].rotation) { handleNext = t.points[t.points.Count - 1].handleNext, handlePrev = t.points[t.points.Count - 1].handlePrev });
                        else
                        {
                            t.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                            Debug.LogWarning("No previous waypoint found to place this waypoint, defaulting position to world center");
                        }
                        break;
                    case PC_NewWaypointMode.WaypointIndex:
                        if (t.points.Count > waypointIndex - 1 && waypointIndex > 0)
                            t.points.Add(new PC_Waypoint(t.points[waypointIndex - 1].position, t.points[waypointIndex - 1].rotation) { handleNext = t.points[waypointIndex - 1].handleNext, handlePrev = t.points[waypointIndex - 1].handlePrev });
                        else
                        {
                            t.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                            Debug.LogWarning("Waypoint index " + waypointIndex + " does not exist, defaulting position to world center");
                        }
                        break;
                    case PC_NewWaypointMode.WorldCenter:
                        t.points.Add(new PC_Waypoint(Vector3.zero, Quaternion.identity));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                selectedIndex = t.points.Count - 1;
                SceneView.RepaintAll();
            }
            GUILayout.Label("at", GUILayout.Width(20));
            EditorGUI.BeginChangeCheck();
            waypointMode = (PC_NewWaypointMode) EditorGUILayout.EnumPopup(waypointMode, waypointMode == PC_NewWaypointMode.WaypointIndex ? GUILayout.Width(Screen.width / 4) : GUILayout.Width(Screen.width / 2));
            if (waypointMode == PC_NewWaypointMode.WaypointIndex)
            {
                waypointIndex = EditorGUILayout.IntField(waypointIndex, GUILayout.Width(Screen.width / 4));
            }
            if (EditorGUI.EndChangeCheck())
            {
                PlayerPrefs.SetInt("CPC_waypointMode", (int) waypointMode);
            }
            GUILayout.EndHorizontal();
        }

        void DrawHandles(int i)
        {
            DrawHandleLines(i);
            Handles.color = t.visual.handleColor;
            DrawNextHandle(i);
            DrawPrevHandle(i);
            DrawWaypointHandles(i);
            DrawSelectionHandles(i);
        }

        void DrawHandleLines(int i)
        {
            Handles.color = t.visual.handleColor;
            if (i < t.points.Count - 1)
                Handles.DrawLine(t.points[i].position, t.points[i].position + t.points[i].handleNext);
            if (i > 0)
                Handles.DrawLine(t.points[i].position, t.points[i].position + t.points[i].handlePrev);
            Handles.color = Color.white;
        }

        void DrawNextHandle(int i)
        {
            if (i < t.points.Count - 1)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 posNext = Vector3.zero;
                float size = HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handleNext) * 0.1f;
                if (handlePositionMode == PC_ManipulationModes.Free)
                {
#if UNITY_5_5_OR_NEWER
                    posNext = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
#else
                    posNext = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereCap);
#endif
                }
                else
                {
                    if (selectedIndex == i)
                    {
#if UNITY_5_5_OR_NEWER
                        Handles.SphereHandleCap(0, t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, EventType.Repaint);
#else
                        Handles.SphereCap(0, t.points[i].position + t.points[i].handleNext, Quaternion.identity, size);
#endif
                        posNext = Handles.PositionHandle(t.points[i].position + t.points[i].handleNext, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
#if UNITY_5_5_OR_NEWER
                        if (Handles.Button(t.points[i].position + t.points[i].handleNext, Quaternion.identity, size, size, Handles.CubeHandleCap))
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
                    t.points[i].handleNext = posNext - t.points[i].position;
                    if (t.points[i].chained)
                        t.points[i].handlePrev = t.points[i].handleNext * -1;
                }
            }

        }

        void DrawPrevHandle(int i)
        {
            if (i > 0)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 posPrev = Vector3.zero;
                float size = HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handlePrev) * 0.1f;
                if (handlePositionMode == PC_ManipulationModes.Free)
                {
#if UNITY_5_5_OR_NEWER
                    posPrev = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handlePrev), Vector3.zero, Handles.SphereHandleCap);
#else
                    posPrev = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handlePrev), Vector3.zero, Handles.SphereCap);
#endif
                }
                else
                {
                    if (selectedIndex == i)
                    {
#if UNITY_5_5_OR_NEWER
                        Handles.SphereHandleCap(0, t.points[i].position + t.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handleNext), EventType.Repaint);
#else
                        Handles.SphereCap(0, t.points[i].position + t.points[i].handlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handleNext));
#endif
                        posPrev = Handles.PositionHandle(t.points[i].position + t.points[i].handlePrev, Quaternion.identity);
                    }
                    else if (Event.current.button != 1)
                    {
#if UNITY_5_5_OR_NEWER
                        if (Handles.Button(t.points[i].position + t.points[i].handlePrev, Quaternion.identity, size, size, Handles.CubeHandleCap))
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
                    t.points[i].handlePrev = posPrev - t.points[i].position;
                    if (t.points[i].chained)
                        t.points[i].handleNext = t.points[i].handlePrev * -1;
                }
            }
        }

        void DrawWaypointHandles(int i)
        {
            if (Tools.current == Tool.Move)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 pos = Vector3.zero;
                if (cameraTranslateMode == PC_ManipulationModes.SelectAndTransform)
                {
                    if (i == selectedIndex)
                        pos = Handles.PositionHandle(t.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.points[i].rotation : Quaternion.identity);
                }
                else
                {
#if UNITY_5_5_OR_NEWER
                    pos = Handles.FreeMoveHandle(t.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.points[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(t.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);
#else
                    pos = Handles.FreeMoveHandle(t.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.points[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(t.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleCap);
#endif
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Moved Waypoint");
                    t.points[i].position = pos;
                }
            }
            else if (Tools.current == Tool.Rotate)
            {

                EditorGUI.BeginChangeCheck();
                Quaternion rot = Quaternion.identity;
                if (cameraRotationMode == PC_ManipulationModes.SelectAndTransform)
                {
                    if (i == selectedIndex)
                        rot = Handles.RotationHandle(t.points[i].rotation, t.points[i].position);
                }
                else
                {
                    rot = Handles.FreeRotateHandle(t.points[i].rotation, t.points[i].position, HandleUtility.GetHandleSize(t.points[i].position) * 0.2f);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Rotated Waypoint");
                    t.points[i].rotation = rot;
                }
            }
        }

        void DrawSelectionHandles(int i)
        {
            if (Event.current.button != 1 && selectedIndex != i)
            {
                if (cameraTranslateMode == PC_ManipulationModes.SelectAndTransform && Tools.current == Tool.Move
                    || cameraRotationMode == PC_ManipulationModes.SelectAndTransform && Tools.current == Tool.Rotate)
                {
                    float size = HandleUtility.GetHandleSize(t.points[i].position) * 0.2f;
#if UNITY_5_5_OR_NEWER
                    if (Handles.Button(t.points[i].position, Quaternion.identity, size, size, Handles.CubeHandleCap))
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
                foreach (var i in t.points)
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
                        Undo.RecordObject(t, "Changed waypoint transform");
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
