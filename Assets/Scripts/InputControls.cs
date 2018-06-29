using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    [Serializable]
    public class InputControls
    {
        static Dictionary<string, KeyCode> keyMapping;

        static string[] keyMaps = new string[]
        {
            "Up",
            "Down",
            "Left",
            "Right"
        };

        static KeyCode[] defaults = new KeyCode[]
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D
        };

        static InputControls()
        {
            keyMapping = new Dictionary<string, KeyCode>();
            for (int i = 0; i < keyMaps.Length; ++i)
            {
                keyMapping.Add(keyMaps[i], defaults[i]);
            }
        }

        public static void SetKeyMap(string keyMap, KeyCode key)
        {
            if (!keyMapping.ContainsKey(keyMap))
                throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
            keyMapping[keyMap] = key;
        }

        public static bool GetKeyDown(string keyMap)
        {
            return Input.GetKeyDown(keyMapping[keyMap]);
        }
    }
}