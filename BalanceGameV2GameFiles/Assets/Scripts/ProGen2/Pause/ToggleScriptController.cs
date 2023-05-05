using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScriptController : MonoBehaviour
{
    public MonoBehaviour targetScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTargetScript();
        }
    }

    void ToggleTargetScript()
    {
        targetScript.enabled = !targetScript.enabled;
    }
}
