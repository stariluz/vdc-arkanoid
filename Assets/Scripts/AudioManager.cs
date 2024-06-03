using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] AudioClip background;
    [SerializeField] AudioClip background2;

    [Header("SFX Clips")]
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip scoreSound;

    void Start(){
        musicSource.clip=background;
        musicSource.Play();
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
    public void OnPause(){

    }
    public void OnResume(){

    }
}

