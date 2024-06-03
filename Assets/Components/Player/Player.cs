using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public PaddleMovement paddleMovement;
    public int score;
	public int initialLives;
    public int lives;
    PlayerAudio playerAudio;
    public Transform ballTransform;

	// Use this for initialization
	void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        paddleMovement = GetComponent<PaddleMovement>();
    }

	// Update is called once per frame
	void Update()
	{
			
	}
    public void StartPlayer()
    {
        score = 0;
        lives = initialLives;
    }

    public bool LoseLive()
	{
        bool hasLost = false;
		playerAudio.onLoseLive();
        lives--;
        Debug.Log(lives);
        if (lives == -1)
        {
            hasLost = true;
        }
        return hasLost;
    }
    public void Restart()
    {
		paddleMovement.Restart();
    }
    public void Pause()
    {
        paddleMovement.Pause();
    }
    public void Resume()
    {
        paddleMovement.Resume();
    }
}

