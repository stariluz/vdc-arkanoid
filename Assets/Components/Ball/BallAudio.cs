using UnityEngine;
using System.Collections;

public class BallAudio : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip paddleSound;
    [SerializeField] AudioClip wallSound;

    // Use this for initialization
    void Start()
	{
        sfxSource = GetComponent<AudioSource>();
    }

    public void OnHitWall()
    {
        sfxSource.clip = wallSound;
        sfxSource.Play();
    }
    public void OnHitPaddle()
    {
        sfxSource.clip = paddleSound;
        sfxSource.Play();
    }
}

