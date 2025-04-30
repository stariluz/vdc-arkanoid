using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AdSkipButton : MonoBehaviour
{
    public Button skipButton;        // Asigna el botón desde el Inspector
    public TMP_Text countdownText;       // Asigna el texto desde el Inspector
    public float waitTime = 5f;      // Tiempo para poder skipear
    public GameManager gameManager;
    private float timer;
    private bool canSkip = false;

    void OnEnable()
    {
        timer = waitTime;
        skipButton.gameObject.SetActive(true);    // El botón existe desde el inicio
        skipButton.interactable = false;          // Pero no se puede presionar
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (timer > 0f)
        {
            countdownText.text = "Puedes seguir en " + Mathf.Ceil(timer) + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        canSkip = true;
        skipButton.interactable = true;
        countdownText.text = "¡Puedes seguir!";
    }
    
    public void OnSkipPressed()
    {
        if (canSkip)
        {
            Debug.Log("Anuncio skipeado");
            gameManager.ReturnScreen();
            gameManager.NextLevel();
        }
    }
}
