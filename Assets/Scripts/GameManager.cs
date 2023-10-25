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
    public GameAudio gameAudio;
    public LevelsManager levelsManager;

    // Start is called before the first frame update
    void Start()
    {
        gameAudio = GetComponent<GameAudio>();
        StartGame();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        levelsManager.StartLevels();
        gameStatus = GameStatus.START_SCREEN;
        uIManager.UpdateScreen(gameStatus);
        players[playerInTurn].StartPlayer();
    }
    public void Restart()
    {
        levelsManager.Restart();
        players[playerInTurn].Restart();
        ball.Restart(playerInTurn);
    }

    public void StartMatch()
    {
        players[PlayersEnum.PLAYER_1].score = 0;
        uIManager.UpdateScore(PlayersEnum.PLAYER_1, 0);

        uIManager.UpdateLives(PlayersEnum.PLAYER_1, players[PlayersEnum.PLAYER_1].lives);

        gameStatus = GameStatus.IN_PLAY;
        uIManager.UpdateScreen(gameStatus);

        int option = new System.Random().Next(1);
        if (option == 1)
        {
            playerInTurn = PlayersEnum.PLAYER_1;
        }
        ball.Restart(playerInTurn);
    }


    public void SetCurrentPlayerInTurn(PlayersEnum player)
    {

        playerInTurn = player;
    }

    public Tuple<PlayersEnum, int> Score(PlayersEnum player)
    {
        gameAudio.onScore();

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
        Debug.Log(gameStatus);
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
            }
        }

        RestartMatch();
    }

    public void GameOver(PlayersEnum player)
    {
        Debug.Log(("DEV - GAME OVER"));
        gameAudio.onGameOver();
        gameStatus = GameStatus.GAME_OVER_SCREEN;
        uIManager.UpdateScreen(gameStatus);
    }

    public void Win(PlayersEnum player)
    {
        gameAudio.onGameWin();
        if (levelsManager.HasNextLevel())
        {
            gameStatus = GameStatus.NEXT_LEVEL_SCREEN;
        }
        else
        {
            gameStatus = GameStatus.WIN_SCREEN;
        }
        uIManager.ShowWinner(player);

    }

    public void RestartMatch()
    {
        players[PlayersEnum.PLAYER_1].Restart();
        ball.Restart(playerInTurn);
    }

    public void NextLevel()
    {
        levelsManager.NextLevel();
        StartMatch();
    }
}
