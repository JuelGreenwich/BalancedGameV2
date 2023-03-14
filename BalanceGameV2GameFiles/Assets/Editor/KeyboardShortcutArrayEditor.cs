using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(KeyboardShortcutArray))]
public class KeyboardShortcutArrayEditor : Editor
{
    // Instance variables to store the input strings
    private string inputString = "";
    private KeyCode inputKey = KeyCode.None;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Get a reference to the KeyboardShortcutArray script
        KeyboardShortcutArray script = (KeyboardShortcutArray)target;

        // Display input fields for the string and key input
        EditorGUILayout.BeginHorizontal();
        inputString = EditorGUILayout.TextField("Input String", inputString);
        inputKey = (KeyCode)EditorGUILayout.EnumPopup("Input Key", inputKey);
        EditorGUILayout.EndHorizontal();

        // Add the input string and key to the shortcut array and update the array in the inspector
        if (GUILayout.Button("Add To Shortcut Array"))
        {
            script.AddShortcut(inputKey, inputString);
            EditorUtility.SetDirty(target);
        }

        // Display the shortcut array in the inspector
        if (script.shortcutArray != null)
        {
            for (int i = 0; i < script.shortcutArray.Length; i++)
            {
                EditorGUILayout.LabelField($"Shortcut {i + 1}: {script.shortcutArray[i].stringValue}, {script.shortcutArray[i].keyInput}");
            }

            // Save the shortcut array to disk
            if (GUILayout.Button("Save Shortcut Array"))
            {
                script.SaveData();
            }

            // Show the shortcut array in the console
            if (GUILayout.Button("Show Shortcut Array"))
            {
                Debug.Log($"Shortcut Array: {string.Join(", ", Array.ConvertAll(script.shortcutArray, t => $"({t.stringValue}, {t.keyInput})"))}");
            }
        }
    }
}