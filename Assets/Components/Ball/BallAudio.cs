using UnityEngine;
using System.Collections;

public class BallAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip paddleSound;
    [SerializeField] AudioClip wallSound;

    // Use this for initialization
    void Start()
	{
        audioSource = GetComponent<AudioSource>();
    }

    public void onHitWall()
    {
        audioSource.clip = wallSound;
        audioSource.Play();
    }
    public void onHitPaddle()
    {
        audioSource.clip = paddleSound;
        audioSource.Play();
    }
}

