using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSource.mute = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSource.mute = true;
        }
    }
}