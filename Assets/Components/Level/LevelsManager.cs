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
    private Level[] _levels;
    public Level[] Levels => _levels;
    private int currentLevelIndex = 0;
    private int _highestCompletedLevel = 0; // Para guardar el mayor nivel completado
    public int HighestCompletedLevel => _highestCompletedLevel;
    public Countdown countdown;
    public delegate void UpdateTimeEvent(int time);
    public UpdateTimeEvent OnUpdateTime;
    // Start is called before the first frame update
    void Start()
    {
        LoadCompletedLevels();
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
        _highestCompletedLevel = PlayerPrefs.GetInt("HighestCompletedLevel", 0);
        currentLevelIndex = HighestCompletedLevel;
        if (currentLevelIndex >= Levels.Length)
        {
            currentLevelIndex = Levels.Length - 1; // Asegura que no exceda el índice de los niveles disponibles
        }
    }

    public void SaveCompletedLevels()
    {
        // Guardar el mayor nivel completado en PlayerPrefs
        PlayerPrefs.SetInt("HighestCompletedLevel", HighestCompletedLevel);
        PlayerPrefs.Save();  // Asegura que los cambios se guardan inmediatamente
    }

    public Level LoadLevel(int levelIndex)
    {
        // Verificar que el índice de nivel es válido
        if (levelIndex < 0 || levelIndex >= Levels.Length)
        {
            Debug.LogError("Nivel inválido.");
            return null;
        }

        currentLevelIndex = levelIndex;
        countdown.SetAvailableTime(180);

        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject); // Destruir el nivel anterior
        }

        GameObject levelToLoad = Instantiate(Levels[levelIndex].gameObject, transform.position, Quaternion.identity);
        currentLevel = levelToLoad.GetComponent<Level>();
        currentLevel.gameObject.SetActive(true);
        
        return currentLevel;
    }
    public void UpdateHighestCompletedLevel()
    {
        _highestCompletedLevel = currentLevelIndex;
        SaveCompletedLevels();
    }
    public Level NextLevel()
    {
        countdown.SetAvailableTime(180);
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        currentLevelIndex++;

        GameObject levelToLoad = Instantiate(Levels[currentLevelIndex].gameObject, transform.position, Quaternion.identity);
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
        return currentLevelIndex < Levels.Length - 1;
    }
}
