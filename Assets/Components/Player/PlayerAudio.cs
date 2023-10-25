using UnityEngine;
using System.Collections;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip loseLiveSound;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void onLoseLive()
    {
        audioSource.clip = loseLiveSound;
        audioSource.Play();
    }
}

