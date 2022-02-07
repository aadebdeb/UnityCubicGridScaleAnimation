using UnityEngine;
using UnityEditor;

namespace CubicGridScaleAnimation
{
    [CustomEditor(typeof(CubicGridScaleAnimator))]
    public class CubicGridScaleAnimatorEditor: Editor
    {
        readonly Vector3Int InitialGridSize = new Vector3Int(10, 10, 10);
        readonly Vector3 InitialGridSpacing = new Vector3(1.0f, 1.0f, 1.0f);

        bool isOpenGridGridGeneration = true;
        bool isOpenCurrentGrid = true;
        bool isOpenAnimation = true;
        bool isOPenGlbalScale = true;
        bool isOpenXScale = true;
        bool isOpenYScale = true;
        bool isOpenZScale = true;
        bool isOpenRadiusScale = true;
        bool isOpenRandomScale = true;

        CubicGridScaleAnimator cubicGridScaleAnimator;
        GameObject gridPoint;
        Vector3Int gridSize;
        Vector3 gridSpacing;
        SerializedProperty autoUpdate;
        SerializedProperty baseScale;
        SerializedProperty timeMultiplier;
        SerializedProperty globalScaleIntensity;
        SerializedProperty globalScaleSpeed;
        SerializedProperty globalScalePhase;
        SerializedProperty xScaleIntensity;
        SerializedProperty xScaleFrequency;
        SerializedProperty xScaleSpeed;
        SerializedProperty xScalePhase;
        SerializedProperty yScaleIntensity;
        SerializedProperty yScaleFrequency;
        SerializedProperty yScaleSpeed;
        SerializedProperty yScalePhase;
        SerializedProperty zScaleIntensity;
        SerializedProperty zScaleFrequency;
        SerializedProperty zScaleSpeed;
        SerializedProperty zScalePhase;
        SerializedProperty radiusScaleIntensity;
        SerializedProperty radiusScaleFrequency;
        SerializedProperty radiusScaleSpeed;
        SerializedProperty radiusScalePhase;
        SerializedProperty randomScaleIntensity;
        SerializedProperty randomScaleSpeed;
        void OnEnable()
        {
            cubicGridScaleAnimator = (CubicGridScaleAnimator)target;
            if (cubicGridScaleAnimator.hasGrid)
            {
                gridPoint = cubicGridScaleAnimator.currentGridPoint;
                gridSize = cubicGridScaleAnimator.currentGridSize;
                gridSpacing = cubicGridScaleAnimator.currentGridSpacing;
            }
            else
            {
                gridPoint = null;
                gridSize = InitialGridSize;
                gridSpacing = InitialGridSpacing;
            }

            autoUpdate = serializedObject.FindProperty("autoUpdate");
            baseScale = serializedObject.FindProperty("baseScale");
            timeMultiplier = serializedObject.FindProperty("timeMultiplier");
            globalScaleIntensity = serializedObject.FindProperty("globalScaleIntensity");
            globalScaleSpeed = serializedObject.FindProperty("globalScaleSpeed");
            globalScalePhase = serializedObject.FindProperty("globalScalePhase");
            xScaleIntensity = serializedObject.FindProperty("xScaleIntensity");
            xScaleFrequency = serializedObject.FindProperty("xScaleFrequency");
            xScaleSpeed = serializedObject.FindProperty("xScaleSpeed");
            xScalePhase = serializedObject.FindProperty("xScalePhase");
            yScaleIntensity = serializedObject.FindProperty("yScaleIntensity");
            yScaleFrequency = serializedObject.FindProperty("yScaleFrequency");
            yScaleSpeed = serializedObject.FindProperty("yScaleSpeed");
            yScalePhase = serializedObject.FindProperty("yScalePhase");
            zScaleIntensity = serializedObject.FindProperty("zScaleIntensity");
            zScaleFrequency = serializedObject.FindProperty("zScaleFrequency");
            zScaleSpeed = serializedObject.FindProperty("zScaleSpeed");
            zScalePhase = serializedObject.FindProperty("zScalePhase");
            radiusScaleIntensity = serializedObject.FindProperty("radiusScaleIntensity");
            radiusScaleFrequency = serializedObject.FindProperty("radiusScaleFrequency");
            radiusScaleSpeed = serializedObject.FindProperty("radiusScaleSpeed");
            radiusScalePhase = serializedObject.FindProperty("radiusScalePhase");
            randomScaleIntensity = serializedObject.FindProperty("randomScaleIntensity");
            randomScaleSpeed = serializedObject.FindProperty("randomScaleSpeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (isOpenGridGridGeneration = EditorGUILayout.Foldout(isOpenGridGridGeneration, "Grid Generation", EditorStyles.foldoutHeader))
            {
                EditorGUI.indentLevel++;
                gridPoint = (GameObject)EditorGUILayout.ObjectField("Point", gridPoint, typeof(GameObject), true);
                gridSize = EditorGUILayout.Vector3IntField("Size", gridSize);
                gridSpacing = EditorGUILayout.Vector3Field("Spacing", gridSpacing);

                var isPointNotNull = gridPoint != null;
                var isGridSizePositive = gridSize.x > 0 && gridSize.y > 0 && gridSize.z > 0;


                using(new EditorGUI.DisabledScope(!isPointNotNull || !isGridSizePositive))
                {
                    if (GUILayout.Button("Create Grid"))
                    {
                        cubicGridScaleAnimator.CreateGrid(gridPoint, gridSize, gridSpacing);
                    }
                }
                if (!isPointNotNull)
                {
                    EditorGUILayout.HelpBox("Grid point must no be null.", MessageType.Warning);
                }
                if (!isGridSizePositive)
                {
                    EditorGUILayout.HelpBox("Grid size must be positive values.", MessageType.Warning);
                }
                using (new EditorGUI.DisabledScope(!cubicGridScaleAnimator.hasGrid))
                {
                    if (GUILayout.Button("Delete Grid"))
                    {
                        cubicGridScaleAnimator.DeleteGrid();
                    }
                }
                EditorGUILayout.HelpBox("Please delete grid before reset to avoid missing references to grid points.", MessageType.Info);
                EditorGUI.indentLevel--;
            }
            if (isOpenCurrentGrid = EditorGUILayout.Foldout(isOpenCurrentGrid, "Current Grid", EditorStyles.foldoutHeader))
            {
                EditorGUI.indentLevel++;
                if (cubicGridScaleAnimator.hasGrid)
                {
                    using (new EditorGUI.DisabledGroupScope(true))
                    {
                        EditorGUILayout.ObjectField("Point", cubicGridScaleAnimator.currentGridPoint, typeof(GameObject), false);
                        EditorGUILayout.Vector3IntField("Size", cubicGridScaleAnimator.currentGridSize);
                        EditorGUILayout.Vector3Field("Spacing", cubicGridScaleAnimator.currentGridSpacing);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Grid is not created.", MessageType.None);
                }
                EditorGUI.indentLevel--;
            }

            if (isOpenAnimation = EditorGUILayout.Foldout(isOpenAnimation, "Animation", EditorStyles.foldoutHeader))
            {
                EditorGUI.indentLevel++;
                autoUpdate.boolValue = EditorGUILayout.Toggle("Auto Update", autoUpdate.boolValue);
                baseScale.floatValue = EditorGUILayout.FloatField("Base Scale", baseScale.floatValue);
                timeMultiplier.floatValue = EditorGUILayout.FloatField("Time Multiplier", timeMultiplier.floatValue);
                if (isOPenGlbalScale = EditorGUILayout.Foldout(isOPenGlbalScale, "Global Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    globalScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", globalScaleIntensity.floatValue, 0.0f, 1.0f);
                    globalScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", globalScaleSpeed.floatValue);
                    globalScalePhase.floatValue = EditorGUILayout.Slider("Phase", globalScalePhase.floatValue, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }
                if (isOpenXScale = EditorGUILayout.Foldout(isOpenXScale, "X Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    xScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", xScaleIntensity.floatValue, 0.0f, 1.0f);
                    xScaleFrequency.floatValue = EditorGUILayout.FloatField("Frequency", xScaleFrequency.floatValue);
                    xScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", xScaleSpeed.floatValue);
                    xScalePhase.floatValue = EditorGUILayout.Slider("Phase", xScalePhase.floatValue, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }
                if (isOpenYScale = EditorGUILayout.Foldout(isOpenYScale, "Y Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    yScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", yScaleIntensity.floatValue, 0.0f, 1.0f);
                    yScaleFrequency.floatValue = EditorGUILayout.FloatField("Frequency", yScaleFrequency.floatValue);
                    yScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", yScaleSpeed.floatValue);
                    yScalePhase.floatValue = EditorGUILayout.Slider("Phase", yScalePhase.floatValue, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }
                if (isOpenZScale = EditorGUILayout.Foldout(isOpenZScale, "Z Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    zScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", zScaleIntensity.floatValue, 0.0f, 1.0f);
                    zScaleFrequency.floatValue = EditorGUILayout.FloatField("Frequency", zScaleFrequency.floatValue);
                    zScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", zScaleSpeed.floatValue);
                    zScalePhase.floatValue = EditorGUILayout.Slider("Phase", zScalePhase.floatValue, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }
                if (isOpenRadiusScale = EditorGUILayout.Foldout(isOpenRadiusScale, "Radius Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    radiusScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", radiusScaleIntensity.floatValue, 0.0f, 1.0f);
                    radiusScaleFrequency.floatValue = EditorGUILayout.FloatField("Frequency", radiusScaleFrequency.floatValue);
                    radiusScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", radiusScaleSpeed.floatValue);
                    radiusScalePhase.floatValue = EditorGUILayout.Slider("Phase", radiusScalePhase.floatValue, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }
                if (isOpenRandomScale = EditorGUILayout.Foldout(isOpenRandomScale, "Random Scale Animation", EditorStyles.foldout))
                {
                    EditorGUI.indentLevel++;
                    randomScaleIntensity.floatValue = EditorGUILayout.Slider("Intensity", randomScaleIntensity.floatValue, 0.0f, 1.0f);
                    randomScaleSpeed.floatValue = EditorGUILayout.FloatField("Speed", randomScaleSpeed.floatValue);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}