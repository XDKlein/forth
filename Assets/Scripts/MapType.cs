using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace forth
{
    [CreateAssetMenu(fileName = "NewMapType", menuName = "Map/MapType")]
    public class MapType : ScriptableObject
    {
        public string name = "NewMapType";
        public Vector2 size = new Vector2(250, 250);
        public int minSystems = 250;
        public int maxSystems = 500;
        public float objectsSizeMultiplier = 1f;
        public Texture2D template;
        public List<SystemTypeAndProbability> systemTypes = new List<SystemTypeAndProbability>();
    }

    [Serializable]
    public struct SystemTypeAndProbability
    {
        public SystemType systemType;
        public float probability;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(MapType))]
    public class MapTypeEditor : Editor
    {
        private bool generalSettings = true;
        private bool systemTypesSettings = true;

        override public void OnInspectorGUI()
        {
            MapType mapType = (MapType)target;
            EditorUtility.SetDirty(mapType);

            EditorGUILayout.BeginVertical();
            string storedName = mapType.name;
            mapType.name = EditorGUILayout.DelayedTextField("Map Type Name: ", mapType.name);
            if (mapType.name != storedName)
            {
                string assetPath = AssetDatabase.GetAssetPath(mapType.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, mapType.name);
                AssetDatabase.SaveAssets();
            }

            generalSettings = EditorGUILayout.Foldout(generalSettings, "General Map Settings", true);
            if (generalSettings)
            {
                EditorGUI.indentLevel++;
                mapType.size = EditorGUILayout.Vector2Field("Map Size: ", mapType.size);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
                GUILayout.Label("Stars: ");
                EditorGUIUtility.labelWidth = 60f;
                mapType.minSystems = EditorGUILayout.IntField("Min: ", mapType.minSystems);
                mapType.maxSystems = EditorGUILayout.IntField("Max: ", mapType.maxSystems);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUIUtility.labelWidth = 120f;
                mapType.template = (Texture2D) EditorGUILayout.ObjectField("Map Texture: ", mapType.template, typeof(Texture2D), false);
                EditorGUI.indentLevel--;
            }

            systemTypesSettings = EditorGUILayout.Foldout(systemTypesSettings, "System Types Settings", true);
            if(systemTypesSettings)
            {
                EditorGUI.indentLevel++;
                int objectsCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size: ", mapType.systemTypes.Count));
                while (objectsCount < mapType.systemTypes.Count)
                    mapType.systemTypes.RemoveAt(mapType.systemTypes.Count - 1);
                while (objectsCount > mapType.systemTypes.Count)
                    mapType.systemTypes.Add(new SystemTypeAndProbability());
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                if (objectsCount > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    GUILayout.Label("System Type");
                    GUILayout.Label("Probability");
                    EditorGUILayout.EndHorizontal();
                }
                for (int i = 0; i < mapType.systemTypes.Count; i++)
                {
                    SystemTypeAndProbability structure = mapType.systemTypes[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    structure.systemType = (SystemType)EditorGUILayout.ObjectField(mapType.systemTypes[i].systemType, typeof(SystemType), false);
                    structure.probability = EditorGUILayout.Slider(mapType.systemTypes[i].probability, 0f, 1f);
                    EditorGUILayout.EndHorizontal();
                    mapType.systemTypes[i] = structure;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            if (GUILayout.Button("Save Object"))
            {
                mapType.systemTypes.Sort((x, y) => y.probability.CompareTo(x.probability));

                string assetPath = AssetDatabase.GetAssetPath(mapType.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, mapType.name);
                AssetDatabase.SaveAssets();
                
            }
            EditorGUILayout.EndVertical();
        }
    }
#endif
}