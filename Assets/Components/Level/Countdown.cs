using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Countdown : MonoBehaviour
{
    public int timeAvailable = 180;

    [SerializeField]
    private int currentTime = 0;

    public delegate void TimeOutEvent();
    public TimeOutEvent OnTimeOut;

    public delegate void UpdateTimeEvent(int time);
    public UpdateTimeEvent OnUpdateTime;

    private Coroutine currentCorroutine = null;
    public void SetAvailableTime()
    {
        SetAvailableTime(timeAvailable);
    }

    public void SetAvailableTime(int seconds)
    {
        currentTime = seconds;
        UpdateTime(currentTime);
    }
    public void Run()
    {
        Debug.Log("RUN TIME");
        Run(timeAvailable);
    }
    public void Run(int seconds)
    {
        if (currentCorroutine == null)
        {
            currentCorroutine = StartCoroutine(Timer(seconds));
        }
        else
        {
            Debug.LogWarning("You can't use the same countdown twice, stop it, and run again.");
        }
    }

    public void Stop()
    {
        Debug.Log(("Stop countdown", currentCorroutine));
        if (currentCorroutine != null)
        {
            StopCoroutine(currentCorroutine);
            currentCorroutine = null;
        }
        else
        {
            Debug.LogWarning("You countdown is already null.");
        }
    }
    private bool isPaused = false;
    public void Pause()
    {
        if (currentCorroutine != null)
        {
            isPaused = true;
        }
        else
        {
            Debug.LogWarning("You countdown is already null.");
        }
    }
    public void Resume()
    {
        if (currentCorroutine != null)
        {
            isPaused = false;
        }
        else
        {
            Debug.LogWarning("You countdown is already null.");
        }
    }

    IEnumerator Timer(int secondsAvailable)
    {
        currentTime = secondsAvailable;
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            if (!isPaused)
            {
                currentTime -= 1;
                UpdateTime(currentTime);
            }
        }
        currentCorroutine = null;
        UpdateTime(currentTime);
        TimeOut();
    }
    private void TimeOut()
    {
        OnTimeOut?.Invoke();
    }

    private void UpdateTime(int time)
    {
        OnUpdateTime?.Invoke(time);
    }

}
