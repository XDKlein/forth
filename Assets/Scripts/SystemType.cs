using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace forth
{
    [CreateAssetMenu(fileName = "NewSystemType", menuName = "Map/Systems/SystemType")]
    public class SystemType : ScriptableObject
    {
        public string name = "NewSystemType";
        public float minSizeMultiplier = 1f, maxSizeMultiplier = 1f;
        public List<string> names = new List<string>();
        public bool useDefault = true;
        public List<ObjectAndProbability> gameObjects = new List<ObjectAndProbability>();

        public int minPlanets = 0, maxPlanets = 0;
        public List<PlanetAndProbability> planets = new List<PlanetAndProbability>();

    }

    [Serializable]
    public struct ObjectAndProbability
    {
        public GameObject gameObject;
        public float probability;
    }

    [Serializable]
    public struct PlanetAndProbability
    {
        public PlanetType planet;
        public float probability;
    }

    [CustomEditor(typeof(SystemType))]
    public class SystemTypeEditor : Editor
    {
        private bool generalSettings = true;
        private bool namesSettings = true;
        private bool gameObjectsSettings = true;
        private bool planetsSettings = true;

        override public void OnInspectorGUI()
        {
            SystemType systemType = (SystemType)target;
            EditorUtility.SetDirty(systemType);

            EditorGUILayout.BeginVertical();

            generalSettings = EditorGUILayout.Foldout(generalSettings, "System General Settings", true);
            if (generalSettings)
            {
                string storedSystemTypeName = systemType.name;
                systemType.name = EditorGUILayout.DelayedTextField("Type Name: ", systemType.name);
                if (systemType.name != storedSystemTypeName)
                {
                    string assetPath = AssetDatabase.GetAssetPath(systemType.GetInstanceID());
                    AssetDatabase.RenameAsset(assetPath, systemType.name);
                    AssetDatabase.SaveAssets();
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
                GUILayout.Label("Size Multipliers: ");
                EditorGUIUtility.labelWidth = 40f;
                systemType.minSizeMultiplier = EditorGUILayout.FloatField("Min: ", systemType.minSizeMultiplier);
                systemType.maxSizeMultiplier = EditorGUILayout.FloatField("Max: ", systemType.maxSizeMultiplier);
                EditorGUIUtility.labelWidth = 120f;
                EditorGUILayout.EndHorizontal();
            }

            namesSettings = EditorGUILayout.Foldout(namesSettings, "System Names List", true);
            if (namesSettings)
            {
                EditorGUI.indentLevel++;
                int namesCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size: ", systemType.names.Count));
                while (namesCount < systemType.names.Count)
                    systemType.names.RemoveAt(systemType.names.Count - 1);
                while (namesCount > systemType.names.Count)
                    systemType.names.Add(null);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                for (int i = 0; i < systemType.names.Count; i++)
                {
                    systemType.names[i] = (string)EditorGUILayout.TextField("Name " + i, systemType.names[i]);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                GUILayout.Label("Use default names list: ");
                systemType.useDefault = EditorGUILayout.Toggle(systemType.useDefault);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            gameObjectsSettings = EditorGUILayout.Foldout(gameObjectsSettings, "System Type Objects", true);
            if (gameObjectsSettings)
            {
                EditorGUI.indentLevel++;
                int objectsCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size: ", systemType.gameObjects.Count));
                while (objectsCount < systemType.gameObjects.Count)
                    systemType.gameObjects.RemoveAt(systemType.gameObjects.Count - 1);
                while (objectsCount > systemType.gameObjects.Count)
                    systemType.gameObjects.Add(new ObjectAndProbability());
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                if (objectsCount > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    GUILayout.Label("GameObject");
                    GUILayout.Label("Probability");
                    EditorGUILayout.EndHorizontal();
                }
                for (int i = 0; i < systemType.gameObjects.Count; i++)
                {
                    ObjectAndProbability structure = systemType.gameObjects[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    structure.gameObject = (GameObject) EditorGUILayout.ObjectField(systemType.gameObjects[i].gameObject, typeof(GameObject), false);
                    structure.probability = EditorGUILayout.Slider(systemType.gameObjects[i].probability, 0f, 1f);
                    EditorGUILayout.EndHorizontal();
                    systemType.gameObjects[i] = structure;
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            planetsSettings = EditorGUILayout.Foldout(planetsSettings, "Planet Types", true);
            if (planetsSettings)
            {
                EditorGUI.indentLevel++;


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
                GUILayout.Label("Planet count: ");
                EditorGUIUtility.labelWidth = 40f;
                systemType.minPlanets = EditorGUILayout.IntField("Min: ", systemType.minPlanets);
                systemType.maxPlanets = EditorGUILayout.IntField("Max: ", systemType.maxPlanets);
                EditorGUIUtility.labelWidth = 120f;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                int objectsCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size: ", systemType.planets.Count));
                while (objectsCount < systemType.planets.Count)
                    systemType.planets.RemoveAt(systemType.planets.Count - 1);
                while (objectsCount > systemType.planets.Count)
                    systemType.planets.Add(new PlanetAndProbability());
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                if (objectsCount > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    GUILayout.Label("Planet");
                    GUILayout.Label("Probability");
                    EditorGUILayout.EndHorizontal();
                }
                for (int i = 0; i < systemType.planets.Count; i++)
                {
                    PlanetAndProbability structure = systemType.planets[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    structure.planet = (PlanetType)EditorGUILayout.ObjectField(systemType.planets[i].planet, typeof(PlanetType), false);
                    structure.probability = EditorGUILayout.Slider(systemType.planets[i].probability, 0f, 1f);
                    EditorGUILayout.EndHorizontal();
                    systemType.planets[i] = structure;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Save Object"))
            {
                systemType.gameObjects.Sort((x, y) => y.probability.CompareTo(x.probability));
                systemType.planets.Sort((x, y) => y.probability.CompareTo(x.probability));

                string assetPath = AssetDatabase.GetAssetPath(systemType.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, systemType.name);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndVertical();
        }
    }
}