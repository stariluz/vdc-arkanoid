using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    // public Level initialLevel;
    private Level currentLevel;
    [SerializeField]
    private Level startLevel;

    [SerializeField]
    private Level[] levels;
    private int currentLevelIndex = 0;

    public Countdown countdown;
    public delegate void UpdateTimeEvent(int time);
    public UpdateTimeEvent OnUpdateTime;
    // Start is called before the first frame update
    void Start()
    {
        // currentLevel = initialLevel;
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
    public Level NextLevel()
    {
        countdown.SetAvailableTime(180);
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        currentLevelIndex++;
        GameObject nextLevel = Instantiate(levels[currentLevelIndex].gameObject, transform.position, Quaternion.identity);
        currentLevel = nextLevel.GetComponent<Level>();
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
}
