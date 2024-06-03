using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip background1;
    public AudioClip background2;

    [Header("SFX Clips")]
    public AudioClip winSound;
    public AudioClip gameOverSound;
    public AudioClip scoreSound;

    void Start(){
        musicSource.clip=background1;
        musicSource.Play();
        musicSource.loop=true;
    }
    public void OnGameOver()
    {
        sfxSource.clip = gameOverSound;
        sfxSource.Play();
    }
	public void OnGameWin()
	{
        sfxSource.clip = winSound;
        sfxSource.Play();
    }
    public void OnScore()
    {
        sfxSource.clip = scoreSound;
        sfxSource.Play();
    }
    float savedTime=0.0f;
    float savedPauseTime=0.0f;
    public void OnPause(){
        musicSource.Pause();
        savedTime=musicSource.time;

        musicSource.clip=background2;
        musicSource.time=savedPauseTime;
        musicSource.Play();
    }
    public void OnResume(){
        musicSource.Pause();
        savedPauseTime=musicSource.time;

        musicSource.clip=background1;
        musicSource.time=savedTime;
        musicSource.Play();
    }
}

