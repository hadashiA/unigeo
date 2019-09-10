using UnityEditor;
using UnityEngine;

namespace Unigeo.Editor
{
    [CustomEditor(typeof(SplineViewer))]
    public class SplineViewerInspector : UnityEditor.Editor
    {
        const float HandleSize = 0.04f;
        const float PickSize = 0.06f;

        SplineViewer splineViewer;
        Transform handleTransform;
        Quaternion handleRotation;
        int selectedIndex = -1;

        void OnEnable()
        {
            splineViewer = target as SplineViewer;
            handleTransform = splineViewer.transform;
        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            serializedObject.Update();
            var stepsProp = serializedObject.FindProperty("steps");
            EditorGUILayout.PropertyField(stepsProp);

            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                var closed = EditorGUILayout.ToggleLeft("Closed", splineViewer.Spline.Closed);
                if (scope.changed)
                {
                    Undo.RecordObject(splineViewer, "Set closed");
                    // splineViewer.Spline.Closed = closed;
                }
            }

            if (selectedIndex >= 0 && selectedIndex < splineViewer.Spline.ControlPointCount)
            {
                GUILayout.Label($"Selected Point ({selectedIndex})");
                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    var point = EditorGUILayout.Vector3Field("Position", splineViewer.Spline.GetControlPoint(selectedIndex));
                    if (scope.changed)
                    {
                        Undo.RecordObject(splineViewer, "Move Point");
                        splineViewer.Spline.SetControlPoint(selectedIndex, point);
                        EditorUtility.SetDirty(splineViewer);
                    }
                }
            }

//            if (GUILayout.Button("Add Curve"))
//            {
//                Undo.RecordObject(splineViewer, "Add Curve");
//                splineViewer.Spline.AddCurve();
//                EditorUtility.SetDirty(splineViewer);
//            }

            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            if (splineViewer.Spline == null)
                return;

            handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;

            var p0 = ShowPoint(0);
            for (var i = 1; i < splineViewer.Spline.ControlPointCount; i += 3)
            {
                var p1 = ShowPoint(i);
                var p2 = ShowPoint(i + 1);
                var p3 = ShowPoint(i + 2);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);
                p0 = p3;
            }

            Handles.color = Color.blue;
            var currentPoint = handleTransform.TransformPoint(splineViewer.Spline.GetPoint(0f));

            var stepsProp = serializedObject.FindProperty("steps");
            var segments = stepsProp.intValue * splineViewer.Spline.ControlPointCount;
            for (var i = 0; i < segments; i++)
            {
                var t = i / (float) (segments - 1);
                var nextPoint = handleTransform.TransformPoint(splineViewer.Spline.GetPoint(t));
                Handles.DrawLine(currentPoint, nextPoint);

    //            Handles.color = Color.green;
    //            Handles.DrawLine(nextPoint, nextPoint + _line.Spline.GetTangent(i / (float)segments));
                currentPoint = nextPoint;
            }
        }

        Vector3 ShowPoint(int i)
        {
            if (i >= splineViewer.Spline.ControlPointCount || i < 0) return Vector3.zero;

            var point = handleTransform.TransformPoint(splineViewer.Spline.GetControlPoint(i));
            var size = HandleUtility.GetHandleSize(point);

            if (Handles.Button(point, handleRotation, HandleSize * size, PickSize * size, Handles.DotCap))
            {
                selectedIndex = i;
                Repaint();
            }

            if (selectedIndex == i)
            {
                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    point = Handles.DoPositionHandle(point, handleRotation);
                    if (scope.changed)
                    {
                        Undo.RecordObject(splineViewer, "Move Point");
                        var p = handleTransform.InverseTransformPoint(point);
                        splineViewer.Spline.SetControlPoint(i, p);
                        EditorUtility.SetDirty(splineViewer);
                    }
                }
            }
            return point;
        }
    }
}
