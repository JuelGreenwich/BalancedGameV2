using UnityEngine;
using System.Collections;

public class TimedCanvasController : MonoBehaviour
{
    public GameObject loadingScreen;
    public MonoBehaviour targetScript;
    public float displayTime = 5f;

    void Start()
    {
        targetScript.enabled = false;
        StartCoroutine(DisplayCanvas());
    }

    IEnumerator DisplayCanvas()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        loadingScreen.SetActive(false);
        targetScript.enabled = true;
    }
}
