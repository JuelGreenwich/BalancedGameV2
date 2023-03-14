using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public struct KeyboardShortcut
{
    public KeyCode keyInput;
    public string stringValue;

    public KeyboardShortcut(KeyCode keyInput, string stringValue)
    {
        this.keyInput = keyInput;
        this.stringValue = stringValue;
    }
}

[Serializable]
public class ShortcutArrayData
{
    public KeyboardShortcut[] shortcutArray;
}

public class KeyboardShortcutArray : MonoBehaviour
{
    public KeyboardShortcut[] shortcutArray;

    private void Start()
    {
        LoadData();
    }

    public void AddShortcut(KeyCode keyInput, string stringValue)
    {
        if (shortcutArray == null)
        {
            shortcutArray = new KeyboardShortcut[0];
        }
        Array.Resize(ref shortcutArray, shortcutArray.Length + 1);
        shortcutArray[shortcutArray.Length - 1] = new KeyboardShortcut(keyInput, stringValue);
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string directoryPath = Application.dataPath + "/SaveFiles";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = Path.Combine(directoryPath, "Shortcuts.txt");
        FileStream stream = new FileStream(filePath, FileMode.Create);

        ShortcutArrayData data = new ShortcutArrayData();
        data.shortcutArray = shortcutArray;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void LoadData()
    {
        string directoryPath = Application.dataPath + "/SaveFiles";
        if (Directory.Exists(directoryPath))
        {
            string filePath = Path.Combine(directoryPath, "Shortcuts.txt");
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);
                ShortcutArrayData data = (ShortcutArrayData)formatter.Deserialize(stream);
                stream.Close();
                shortcutArray = data.shortcutArray;
            }
        }
    }
}
