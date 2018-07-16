using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace forth
{
    [CreateAssetMenu(fileName = "NewPlanetType", menuName = "Map/Planets/PlanetType")]
    public class PlanetType : ScriptableObject
    {
        public string name = "NewPlanetType";

        [CustomEditor(typeof(PlanetType))]
        public class PlanetTypeEditor : Editor
        {
            private bool generalSettings = true;

            override public void OnInspectorGUI()
            {
                PlanetType planetType = (PlanetType)target;
                EditorUtility.SetDirty(planetType);

                EditorGUILayout.BeginVertical();

                generalSettings = EditorGUILayout.Foldout(generalSettings, "Planet General Settings", true);
                if (generalSettings)
                {
                    string storedPlanetTypeName = planetType.name;
                    planetType.name = EditorGUILayout.DelayedTextField("Type Name: ", planetType.name);
                    if (planetType.name != storedPlanetTypeName)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(planetType.GetInstanceID());
                        AssetDatabase.RenameAsset(assetPath, planetType.name);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}