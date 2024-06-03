using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip scoreSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void onGameOver()
    {
        audioSource.clip = gameOverSound;
        audioSource.Play();
    }
	public void onGameWin()
	{
        audioSource.clip = winSound;
        audioSource.Play();
    }
    public void onScore()
    {
        audioSource.clip = scoreSound;
        audioSource.Play();
    }
    public void OnPause(){

    }
    public void OnResume(){

    }
}

