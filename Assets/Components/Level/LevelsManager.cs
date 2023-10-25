using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    // public Level initialLevel;
    private Level currentLevel;
    public Level[] levels;
    private int currentLevelIndex=0;
    // Start is called before the first frame update
    void Start()
    {
        // currentLevel = initialLevel;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartLevels()
    {
        currentLevelIndex = 0;
        GameObject firstLevel = Instantiate(levels[0].gameObject, new Vector3(0, 7.342612f, 0), Quaternion.identity);
        currentLevel = firstLevel.GetComponent<Level>();
    }
    public void Restart()
    {
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
    }

    public Level NextLevel()
    {
        currentLevel.gameObject.SetActive(false);
        Destroy(currentLevel.gameObject);
        currentLevelIndex++;
        GameObject nextLevel=Instantiate(levels[currentLevelIndex].gameObject, new Vector3(0, 7.342612f, 0), Quaternion.identity);
        currentLevel=nextLevel.GetComponent<Level>();
        currentLevel.gameObject.SetActive(true);

        return currentLevel;
    }
    public bool IsLevelCompleted()
    {
        return currentLevel.IsLevelCompleted();
    }
    public bool HasNextLevel()
    {
        return currentLevelIndex < levels.Length-1;
    }
}
