using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelsUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform buttonsParent; // Contenedor de los botones en la UI
    [SerializeField]
    private GameObject levelButtonPrefab; // Prefab del botón interactuable
    [SerializeField]
    private GameObject lockedButtonPrefab; // Prefab del botón bloqueado

    private List<Button> levelButtons = new List<Button>(); // Lista de botones para los niveles
    public GameManager gameManager;

    void OnEnable()
    {
        if (gameManager != null)
        {
            DisplayLevelButtons();
        }
    }
    private void DisplayLevelButtons()
    {
        // Limpiar cualquier botón anterior
        foreach (var button in levelButtons)
        {
            Destroy(button.gameObject);
        }

        levelButtons.Clear();

        // Crear un botón interactuable para los niveles completados o alcanzados
        for (int i = 0; i <= gameManager.LevelsManager.HighestCompletedLevel; i++)
        {
            GameObject buttonObj;
            Button button;

            buttonObj = Instantiate(levelButtonPrefab, buttonsParent);
            button = buttonObj.GetComponent<Button>();
            int levelIndex = i; // Capturar el índice para el delegado
            button.onClick.AddListener(() =>
            {
                gameManager.LoadLevel(levelIndex);
            }); // Añadir acción al clic


            levelButtons.Add(button);
        }

        // Crear un botón interactuable para los niveles completados o alcanzados
        for (int i = gameManager.LevelsManager.HighestCompletedLevel + 1; i < gameManager.LevelsManager.Levels.Length; i++)
        {
            GameObject buttonObj;
            Button button;

            // Crear un botón bloqueado para los niveles no alcanzados
            buttonObj = Instantiate(lockedButtonPrefab, buttonsParent);
            button = buttonObj.GetComponent<Button>();
            button.interactable = false; // Deshabilitar interacción

            levelButtons.Add(button);
        }
    }
}
