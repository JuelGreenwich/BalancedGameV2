using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{

    public AudioSource audioSource;

    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            // AI is moving, play sound
            audioSource.Play();
        }
        else
        {
            // AI is not moving, stop sound
            audioSource.Stop();
        }
    }
}