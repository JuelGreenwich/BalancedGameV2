using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource mouseHover;
    public AudioSource mouseClick;

    public void OnMouseDown()
    {
        mouseClick.Play();
    }

    public void OnMouseOver()
    {
        //mouseHover.Play();
    }

    public void OnMouseEnter()
    {
        mouseHover.Play();
    }

    //public void OnMouse
}
