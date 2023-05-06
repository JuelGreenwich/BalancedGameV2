using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource mouseHover;
    public AudioSource mouseClick;

    public void PlayOnClick()
    {
        mouseClick.Play();
    }

    public void PlayOnHover()
    {
        mouseHover.Play();
    }
}
