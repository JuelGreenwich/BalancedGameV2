using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DistanceAudioController : MonoBehaviour
{
    public float maxDistance = 10f;

    private AudioSource audioSource;
    private float currentDistance;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (currentDistance <= maxDistance && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (currentDistance > maxDistance && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}




