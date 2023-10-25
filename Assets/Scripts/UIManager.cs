using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [SerializedDictionary("GameStatus", "GameObject")]
    public SerializedDictionary<GameStatus, GameObject> screens;

    [SerializedDictionary("Player", "PlayerUI")]
    public SerializedDictionary<PlayersEnum, PlayerUI> playerUI;

    // public TMPro.TMP_Text winnerText;
    // Start is called before the first frame update

    public Tuple<PlayersEnum, int> UpdateScore(PlayersEnum player, int score)
    {
        Debug.Log((player, score));
        playerUI[player].score.text = score.ToString();
        return new(player, score);
    }
    public Tuple<PlayersEnum, int> UpdateLives(PlayersEnum player, int lives)
    {
        playerUI[player].lives.text = lives.ToString();
        return new(player, lives);
    }

    public void ShowWinner(PlayersEnum player)
    {
        /*if (player == PlayersEnum.PLAYER_1)
        {
            winnerText.text = "Jugador 1";
        }*/
        UpdateScreen(GameStatus.WIN_SCREEN);
    }

    public void UpdateScreen(GameStatus gameStatus)
    {
        DeactivateOtherScreens(gameStatus);
        screens[gameStatus].SetActive(true);
    }

    private void DeactivateOtherScreens(GameStatus nextGameStatus)
    {
        Debug.Log(("DEV -DeactivatingScreens"));
        HashSet<GameStatus> gameStatusArray = new((GameStatus[]) Enum.GetValues(typeof(GameStatus)));
        gameStatusArray.Remove(nextGameStatus);
        foreach (GameStatus gameStatus in gameStatusArray)
        {
            screens[gameStatus].SetActive(false);
        }
    }


    void Restart()
    {
    }
}
