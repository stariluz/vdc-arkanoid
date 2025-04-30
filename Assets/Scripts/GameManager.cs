using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using AYellowpaper.SerializedCollections;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    [SerializedDictionary("PlayerEnum", "Player")]
    public SerializedDictionary<PlayersEnum, Player> players;
    public PlayersEnum playerInTurn = PlayersEnum.PLAYER_1;
    public BallMovement ball;
    public GameStatus gameStatus = GameStatus.START_SCREEN;
    public AudioManager audioManager;
    [SerializeField]
    private LevelsManager _levelsManager;
    public LevelsManager LevelsManager => _levelsManager;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.SetInt("HighestCompletedLevel", -1);
        // PlayerPrefs.Save();

        StartGame();
    }
    
    void OnEnable()
    {
        keyListener.OnKeyDown += HandleKeyDown;
        LevelsManager.countdown.OnUpdateTime += UpdateTime;
        LevelsManager.countdown.OnTimeOut += TimeOut;
    }
    void OnDisable()
    {
        keyListener.OnKeyDown -= HandleKeyDown;
        LevelsManager.countdown.OnUpdateTime -= UpdateTime;
        LevelsManager.countdown.OnTimeOut -= TimeOut;
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
        LevelsManager.StartLevel();
        gameStatus = GameStatus.START_SCREEN;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].StartPlayer();
    }

    public void RestartGame()
    {
        if (isPaused)
        {
            Resume();
        }
        // Debug.Log(("GAMEMANAGER RESTART"));
        LevelsManager.RestartGame();
        players[playerInTurn].Restart();
        ball.Restart(playerInTurn);
        StartGame();
    }

    public void Resume()
    {
        isPaused = false;
        // Debug.Log(("GAMEMANAGER RESUME"));
        LevelsManager.Resume();
        gameStatus = GameStatus.IN_PLAY;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].Resume();
        ball.Resume();
        audioManager.OnResume();
    }

    public void StartGameLevel()
    {
        RestartBoard();
        InitBoard();
        ball.StartGameLevel(playerInTurn);
    }
    public void NextAttempt()
    {
        RestartBoard();
        // players[playerInTurn].Restart();
        ball.Restart(playerInTurn);
    }
    public void InitBoard()
    {
        players[playerInTurn].score = 0;
        uIManager.UpdateScore(playerInTurn, 0);
        gameStatus = GameStatus.IN_PLAY;
        uIManager.UpdateScreen(gameStatus);
        LevelsManager.countdown.SetAvailableTime();
    }
    public void RestartBoard()
    {
        uIManager.UpdateLives(playerInTurn, players[playerInTurn].lives);
    }
    public void FirstLaunchBall()
    {
        // Debug.Log("First Launch");
        LevelsManager.countdown.Run();
    }

    public void SetCurrentPlayerInTurn(PlayersEnum player)
    {

        playerInTurn = player;
    }

    public Tuple<PlayersEnum, int> Score(PlayersEnum player)
    {
        // Debug.Log((player, audioManager));
        audioManager.OnScore();
        int score = players[playerInTurn].score;
        if (gameStatus == GameStatus.IN_PLAY)
        {
            score++;
            players[playerInTurn].score = score;
            uIManager.UpdateScore(playerInTurn, score);
            if (LevelsManager.IsLevelCompleted())
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
        LevelsManager.countdown.Stop();
        gameStatus = GameStatus.GAME_OVER_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void Win(PlayersEnum player)
    {
        audioManager.OnGameWin();
        LevelsManager.UpdateHighestCompletedLevel();
        LevelsManager.countdown.Stop();
        if (LevelsManager.HasNextLevel())
        {
            // Debug.Log("DEV - GameManager - Win() - There's next level");
            gameStatus = GameStatus.NEXT_LEVEL_SCREEN;
            uIManager.UpdateScreen(gameStatus);
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
        LevelsManager.NextLevel();
        StartGameLevel();
    }

    public void OpenLevelsMenu()
    {
        LevelsManager.LoadCompletedLevels();
        screensStack.Push(gameStatus);
        gameStatus = GameStatus.LEVELS_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }
    public void LoadLevel(int levelIndex)
    {
        LevelsManager.LoadLevel(levelIndex);
        screensStack.Pop();
        StartGameLevel();
    }

    public Stack<GameStatus> screensStack = new Stack<GameStatus>();

    [SerializeField]
    KeyListener keyListener;
    void HandleKeyDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Escape)
        {
            if (screensStack.Count > 0)
            {
                ReturnScreen();
            }
            else
            {
                OpenSettings();
            }
        }
    }
    public void ReturnScreen()
    {
        gameStatus = screensStack.Pop();
        if (Array.IndexOf(InGameUI.playable, gameStatus) != -1)
        {
            LevelsManager.Resume();
            players[playerInTurn].Resume();
            ball.Resume();
        }
        uIManager.UpdateScreen(gameStatus);
    }
    
    public void Pause()
    {
        isPaused = true;
        // Debug.Log(("GAMEMANAGER PAUSE"));
        gameStatus = GameStatus.PAUSE_SCREEN;
        uIManager.UpdateScreen(gameStatus);
        LevelsManager.Pause();
        players[playerInTurn].Pause();
        ball.Pause();
        audioManager.OnPause();
    }
    
    public void OpenSettings()
    {
        if (!isPaused)
        {
            LevelsManager.Pause();
            players[playerInTurn].Pause();
            ball.Pause();
        }
        screensStack.Push(gameStatus);
        gameStatus = GameStatus.SETTINGS_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void OpenControlSettings()
    {
        screensStack.Push(gameStatus);
        gameStatus = GameStatus.CONTROLS_SETTINGS_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void OpenSoundSettings()
    {
        screensStack.Push(gameStatus);
        gameStatus = GameStatus.SOUNDS_SETTINGS_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void OpenSkipLevelScreen()
    {
        if (!isPaused)
        {
            LevelsManager.Pause();
            players[playerInTurn].Pause();
            ball.Pause();
        }
        screensStack.Push(gameStatus);
        gameStatus = GameStatus.SKIP_LEVEL_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }
    public void OpenAdScreen()
    {
        gameStatus = GameStatus.AD_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
