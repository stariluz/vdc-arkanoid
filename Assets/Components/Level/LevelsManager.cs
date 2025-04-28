using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    // public Level initialLevel;
    private Level currentLevel;
    [SerializeField]
    private Level startLevel;

    [SerializeField]
    private Level[] levels;
    private int currentLevelIndex = 0;
    private int highestCompletedLevel = 0; // Para guardar el mayor nivel completado

    [SerializeField]
    private Transform buttonsParent; // Contenedor de los botones en la UI
    [SerializeField]
    private GameObject levelButtonPrefab; // Prefab del botón interactuable
    [SerializeField]
    private GameObject lockedButtonPrefab; // Prefab del botón bloqueado

    private List<Button> levelButtons = new List<Button>(); // Lista de botones para los niveles

    public Countdown countdown;
    public delegate void UpdateTimeEvent(int time);
    public UpdateTimeEvent OnUpdateTime;
    // Start is called before the first frame update
    void Start()
    {
        // LoadCompletedLevels();   
        // DisplayLevelButtons();
    }
    void Enable()
    {
        countdown.OnUpdateTime += UpdateTime;
    }
    void Disable()
    {
        countdown.OnUpdateTime -= UpdateTime;
    }
    public void UpdateTime(int time)
    {
        OnUpdateTime?.Invoke(time);
    }

    public void StartLevel()
    {
        currentLevelIndex = -1;
        currentLevel = Instantiate(startLevel.gameObject, transform.position, Quaternion.identity).GetComponent<Level>();
    }

    public void RestartGame()
    {
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        // Debug.Log("Nivel desturido");
    }
    public void Pause()
    {
        countdown.Pause();
    }
    public void Resume()
    {
        countdown.Resume();
    }

    public void LoadCompletedLevels()
    {
        // Cargar el índice del nivel completado más alto desde PlayerPrefs
        highestCompletedLevel = PlayerPrefs.GetInt("HighestCompletedLevel", 0);
        currentLevelIndex = highestCompletedLevel;
        if (currentLevelIndex >= levels.Length)
        {
            currentLevelIndex = levels.Length - 1; // Asegura que no exceda el índice de los niveles disponibles
        }
    }

    public void SaveCompletedLevels()
    {
        // Guardar el mayor nivel completado en PlayerPrefs
        PlayerPrefs.SetInt("HighestCompletedLevel", highestCompletedLevel);
        PlayerPrefs.Save();  // Asegura que los cambios se guardan inmediatamente
    }

    public Level LoadLevel(int levelIndex)
    {
        // Verificar que el índice de nivel es válido
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Nivel inválido.");
            return null;
        }

        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject); // Destruir el nivel anterior
        }

        GameObject levelToLoad = Instantiate(levels[levelIndex].gameObject, transform.position, Quaternion.identity);
        currentLevel = levelToLoad.GetComponent<Level>();
        currentLevel.gameObject.SetActive(true);

        return currentLevel;
    }
    public void UpdateHighestCompletedLevel()
    {
        highestCompletedLevel = currentLevelIndex;
        SaveCompletedLevels();
    }
    public Level NextLevel()
    {
        countdown.SetAvailableTime(180);
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        currentLevelIndex++;

        GameObject levelToLoad = Instantiate(levels[currentLevelIndex].gameObject, transform.position, Quaternion.identity);
        currentLevel = levelToLoad.GetComponent<Level>();
        currentLevel.gameObject.SetActive(true);

        return currentLevel;
    }
    public bool IsLevelCompleted()
    {
        return currentLevel.IsLevelCompleted();
    }
    public bool HasNextLevel()
    {
        return currentLevelIndex < levels.Length - 1;
    }
    
    private void DisplayLevelButtons()
    {
        // Limpiar cualquier botón anterior
        foreach (var button in levelButtons)
        {
            Destroy(button.gameObject);
        }

        levelButtons.Clear();

        // Crear botones para cada nivel
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject buttonObj;
            Button button;

            if (i <= highestCompletedLevel)
            {
                // Crear un botón interactuable para los niveles completados o alcanzados
                buttonObj = Instantiate(levelButtonPrefab, buttonsParent);
                button = buttonObj.GetComponent<Button>();
                int levelIndex = i; // Capturar el índice para el delegado
                button.onClick.AddListener(() => LoadLevel(levelIndex)); // Añadir acción al clic
            }
            else
            {
                // Crear un botón bloqueado para los niveles no alcanzados
                buttonObj = Instantiate(lockedButtonPrefab, buttonsParent);
                button = buttonObj.GetComponent<Button>();
                button.interactable = false; // Deshabilitar interacción
            }

            levelButtons.Add(button);
        }
    }
}
