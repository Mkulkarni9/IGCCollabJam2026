using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static event Action OnTimerEnd;

    public enum TimerType
    {
        CountUpFixed,
        CountUpEndless,
        CountDown,
    }

    [SerializeField] TimerType timerType;
    //[SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image timerImage;
    [SerializeField] SunMovement sunMovement;
    float timerDurationInSec;

    bool timerStarted;
    bool timerPaused;
    bool timerEnded;

    float timeMin;
    float timeSec;
    float currentTime;
    float timerFraction;


    


    
    private void OnEnable()
    {
        LevelManager.OnNewLevelTimerUpdate += UpdateTimerDuration;
        LevelManager.OnNewLevelStart += StartTimer;
        LevelManager.OnLevelComplete += StopTimer;
        OnTimerEnd += StopTimer;
    }


    private void OnDisable()
    {
        LevelManager.OnNewLevelTimerUpdate -= UpdateTimerDuration;
        LevelManager.OnNewLevelStart -= StartTimer;
        LevelManager.OnLevelComplete -= StopTimer;
        OnTimerEnd -= StopTimer;
    }
    private void Start()
    {
        //ResetTimer();
        
    }
    private void Update()
    {
        if(timerStarted && !timerPaused && !timerEnded)
        {
            UpdateTimer();
            //UpdateTimerText();
            UpdateTimerImage();
        }
        
    }

    public void ResetTimer()
    {
        timerStarted = false;
        timerPaused = false;
        timerEnded = false;



        switch (timerType)
        {
            case TimerType.CountUpFixed:
                currentTime = 0f;
                timerFraction = 0f;
                break;
            case TimerType.CountUpEndless:
                currentTime = 0f;
                timerFraction = 1f;
                break;
            case TimerType.CountDown:
                currentTime = timerDurationInSec;
                timerFraction = 1f;
                break;

        }

        //UpdateTimerText();
        UpdateTimerImage();
    }
    
    public void StartTimer(int levelIndex)
    {
        timerStarted = true;
        timerPaused = false;

        sunMovement.StartSunMovement();
    }


    public void UpdateTimer()
    {
        switch(timerType)
        {
            case TimerType.CountUpFixed:
                if (currentTime < timerDurationInSec)
                {
                    currentTime += Time.deltaTime;
                    timerFraction = currentTime / timerDurationInSec;
                }
                else
                {
                    OnTimerEnd?.Invoke();
                }
                break;
            case TimerType.CountUpEndless:
                currentTime += Time.deltaTime;
                timerFraction = 1f;
                break;
            case TimerType.CountDown:
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                    timerFraction = currentTime / timerDurationInSec;
                }
                else
                {
                    OnTimerEnd?.Invoke();
                }
                break;

        }
    }

    

    public void TogglePauseTimer()
    {
        if(timerStarted && !timerEnded)
        {
            timerPaused = !timerPaused;
        }
        
    }

    public void PauseTimer()
    {
        if (timerStarted && !timerEnded)
        {
            timerPaused = true;
        }
        
    }

    public void ResumeTimer()
    {
        if (timerStarted && !timerEnded)
        {
            timerPaused = false;
        }
        
    }

    public void StopTimer()
    {
        timerEnded = true;
    }

    public void StopTimer(int levelIndex)
    {
        timerEnded = true;
        sunMovement.StopMovement();
    }


    void UpdateTimerText()
    {

        timeMin = Mathf.Max(Mathf.Floor(currentTime / 60), 0);
        timeSec = Mathf.Max(Mathf.Floor(currentTime % 60), 0);

        //timerText.text = timeMin.ToString("00") + ":" + timeSec.ToString("00");

    }

    public void UpdateTimerImage()
    {
        timerImage.fillAmount = timerFraction;
    }

    public void UpdateTimerDuration(float duration)
    {
        Debug.Log("Updating timer duration to: "+ duration);
        timerDurationInSec = duration;

        ResetTimer();
        sunMovement.SetSunSpeed(timerDurationInSec);
    }

    

}
