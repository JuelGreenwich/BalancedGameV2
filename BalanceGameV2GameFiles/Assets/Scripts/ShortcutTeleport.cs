using System.Collections.Generic;
using UnityEngine;

public class ShortcutTeleport : MonoBehaviour
{
    public KeyboardShortcutArray shortcutArray;

    private Dictionary<string, GameObject[]> taggedObjects;

    private void Start()
    {
        taggedObjects = new Dictionary<string, GameObject[]>();
    }

    private void Update()
    {
        if (shortcutArray == null)
        {
            return;
        }

        // Loop through each shortcut in the shortcutArray
        for (int i = 0; i < shortcutArray.shortcutArray.Length; i++)
        {
            // Check if the left control key and the current shortcut key are being pressed
            while (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(shortcutArray.shortcutArray[i].keyInput))
            {
                // Find all the game objects with the tag specified by the current shortcut string
                GameObject[] taggedObjectArray = GameObject.FindGameObjectsWithTag(shortcutArray.shortcutArray[i].stringValue);
                if (taggedObjectArray == null)
                {
                    Debug.LogError($"No objects with tag {shortcutArray.shortcutArray[i].stringValue} were found.");
                    continue;
                }

                taggedObjects[shortcutArray.shortcutArray[i].stringValue] = taggedObjectArray;

                // Loop through the integers from 0 to 9
                for (int j = 0; j <= 9; j++)
                {
                    // Check if the corresponding key (e.g. Alpha0 for 0, Alpha1 for 1, etc.) is being pressed
                    KeyCode integerKey = (KeyCode)((int)KeyCode.Alpha0 + j);
                    if (Input.GetKeyDown(integerKey))
                    {
                        // Subtract 1 from the integer to get the index of the object in the tagged object array
                        int objectIndex = j - 1;

                        // If the object index is valid, log the position of the object at that index to the console
                        if (objectIndex >= 0 && objectIndex < taggedObjectArray.Length)
                        {
                            Debug.Log($"The position of object {objectIndex + 1} with tag {shortcutArray.shortcutArray[i].stringValue} is: {taggedObjectArray[objectIndex].transform.position}");
                            Camera.main.transform.position = taggedObjectArray[objectIndex].transform.position;
                        }
                        else
                        {
                            Debug.LogError($"Invalid object index: {objectIndex}. There are only {taggedObjectArray.Length} objects with tag {shortcutArray.shortcutArray[i].stringValue}.");
                        }

                        // Break out of the inner loop
                        break;
                    }
                }

                // Break out of the outer loop
                break;
            }
        }
    }
}
