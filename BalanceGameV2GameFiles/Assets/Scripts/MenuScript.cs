using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    public string sampleScene;

    public GameObject menuScreen;

    public GameObject settingsScreen;

    public GameObject creditsScreen;

    public GameObject controlsScreen;

    private void Start()
    {
        menuScreen.SetActive(true);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        controlsScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sampleScene);
    }

    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
        menuScreen.SetActive(false);
    }



    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
        controlsScreen.SetActive(false);
        menuScreen.SetActive(false);
    }

    

    public void OpenControls()
    {
        controlsScreen.SetActive(true);
        settingsScreen.SetActive(false);
        menuScreen.SetActive(false);

    }



    public void BackToMenu()
    {
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting");
    }
}
