using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using AYellowpaper.SerializedCollections;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    [SerializedDictionary("PlayerEnum", "Player")]
    public SerializedDictionary<PlayersEnum, Player> players;
    public PlayersEnum playerInTurn = PlayersEnum.PLAYER_1;
    public BallMovement ball;
    public GameStatus gameStatus = GameStatus.START_SCREEN;
    public AudioManager audioManager;
    public LevelsManager levelsManager;

    // Start is called before the first frame update
    void Start()
    {
        levelsManager.countdown.OnUpdateTime += UpdateTime;
        levelsManager.countdown.OnTimeOut += TimeOut;
        audioManager = GetComponent<AudioManager>();
        StartGame();
    }
    void Disable()
    {
        levelsManager.countdown.OnUpdateTime -= UpdateTime;
        levelsManager.countdown.OnTimeOut -= TimeOut;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Player GetPlayerInTurn()
    {
        return players[playerInTurn];
    }
    public void StartGame()
    {
        levelsManager.StartLevels();
        gameStatus = GameStatus.START_SCREEN;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].StartPlayer();
    }
    public void RestartGame()
    {
        // Debug.Log(("GAMEMANAGER RESTART"));
        levelsManager.RestartGame();
        players[playerInTurn].Restart();
        ball.Restart(playerInTurn);
        StartGame();
    }
    public void Pause()
    {
        // Debug.Log(("GAMEMANAGER PAUSE"));
        levelsManager.Pause();
        gameStatus = GameStatus.PAUSE_SCREEN;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].Pause();
        ball.Pause();
        audioManager.OnPause();
    }

    public void Resume()
    {
        // Debug.Log(("GAMEMANAGER RESUME"));
        levelsManager.Resume();
        gameStatus = GameStatus.IN_PLAY;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].Resume();
        ball.Resume();
        audioManager.OnResume();
    }

    public void StartGameLevel()
    {
        InitBoard();
        RestartBoard();
        ball.StartGameLevel(playerInTurn);
    }
    public void NextAttempt()
    {
        RestartBoard();
        players[playerInTurn].Restart();
        ball.Restart(playerInTurn);
    }
    public void InitBoard(){
        players[playerInTurn].score = 0;
        uIManager.UpdateScore(playerInTurn, 0);
        gameStatus = GameStatus.IN_PLAY;
        uIManager.UpdateScreen(gameStatus);
        levelsManager.countdown.SetAvailableTime();
    }
    public void RestartBoard(){
        uIManager.UpdateLives(playerInTurn, players[playerInTurn].lives);
        // int option = new System.Random().Next(1);
        // if (option == 1)
        // {
        //     playerInTurn = playerInTurn;
        // }
    }
    public void FirstLaunchBall()
    {
        Debug.Log("First Launch");
        levelsManager.countdown.Run();
    }

    public void SetCurrentPlayerInTurn(PlayersEnum player)
    {

        playerInTurn = player;
    }

    public Tuple<PlayersEnum, int> Score(PlayersEnum player)
    {
        audioManager.OnScore();
        int score = players[playerInTurn].score;
        if (gameStatus == GameStatus.IN_PLAY)
        {
            score++;
            players[playerInTurn].score = score;
            uIManager.UpdateScore(playerInTurn, score);
            if (levelsManager.IsLevelCompleted())
            {
                Win(player);
            }
        }
        return new(playerInTurn, score);
    }

    public void LoseLive()
    {
        if (gameStatus == GameStatus.IN_PLAY)
        {
            bool hasLost = players[playerInTurn].LoseLive();
            if (hasLost)
            {
                uIManager.UpdateLives(playerInTurn, players[playerInTurn].lives);
                GameOver(playerInTurn);
            }
            else
            {
                uIManager.UpdateLives(playerInTurn, players[playerInTurn].lives);
                NextAttempt();
            }
        }
    }

    public void UpdateTime(int time)
    {
        uIManager.UpdateCountdown(playerInTurn, time);
    }

    public void TimeOut()
    {
        // Debug.Log(("DEV - TIME OUT"));
        audioManager.OnGameOver();
        gameStatus = GameStatus.TIME_OUT_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }
    public void GameOver(PlayersEnum player)
    {
        // Debug.Log(("DEV - GAME OVER"));
        audioManager.OnGameOver();
        gameStatus = GameStatus.GAME_OVER_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void Win(PlayersEnum player)
    {
        audioManager.OnGameWin();
        levelsManager.countdown.Stop();
        if (levelsManager.HasNextLevel())
        {
            // Debug.Log("DEV - GameManager - Win() - There's next level");
            gameStatus = GameStatus.NEXT_LEVEL_SCREEN;
        }
        else
        {
            // Debug.Log("DEV - GameManager - Win() - There's no next level");
            gameStatus = GameStatus.WIN_SCREEN;
            uIManager.UpdateScreen(gameStatus);
        }
    }

    public void NextLevel()
    {
        levelsManager.NextLevel();
        StartGameLevel();
    }
}
