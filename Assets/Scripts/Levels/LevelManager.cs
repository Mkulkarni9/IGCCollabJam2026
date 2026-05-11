using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static event Action<int> OnLevelComplete;
    public static event Action OnLevelCountDownStart;
    public static event Action<int> OnNewLevelStart;
    public static event Action<float> OnNewLevelTimerUpdate;


    [SerializeField] List<LevelSO> levels = new List<LevelSO>();
    //[SerializeField] float intervalBetweenLevels;

    [SerializeField] int totalCountDowns;
    [SerializeField] int intervalBetweenCOuntdown;


    
    [SerializeField] Image titleScreen;
    [SerializeField] Image currentCountDownImage;
    [SerializeField] List<Sprite> countDownSprites;

    int currentLevelIndex = 0;

    Coroutine levelRoutine;
    Coroutine countDownRoutine;

    private void OnEnable()
    {
        GameManager.OnStartGame += HideTitleScreen;
        GameManager.OnStartGame += LevelStartCountDown;
        AnimalManager.OnZeroSheepOnMap += EndCurrentLevel;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= HideTitleScreen;
        GameManager.OnStartGame -= LevelStartCountDown;
        AnimalManager.OnZeroSheepOnMap -= EndCurrentLevel;

    }

    void HideTitleScreen()
    {
        titleScreen.gameObject.SetActive(false);
    }


    void StartLevels(int levelIndex)
    {
        if(levelRoutine!=null)
        {
            StopCoroutine(levelRoutine);
        }

        levelRoutine = StartCoroutine(LevelRoutine(levelIndex));
    }


    IEnumerator LevelRoutine(int levelIndex)
    {

        Debug.Log("Starting level: " + levelIndex);
        OnNewLevelTimerUpdate?.Invoke(levels[levelIndex].levelDuration);
        OnNewLevelStart?.Invoke(levelIndex);
        yield return new WaitForSeconds(levels[levelIndex].levelDuration);

        OnLevelComplete?.Invoke(levelIndex);
        currentLevelIndex++;
        /*yield return new WaitForSeconds(intervalBetweenLevels);*/



    }


    public void StartNextLevel()
    {
        Debug.Log($"{currentLevelIndex} Level");
        StartLevels(currentLevelIndex);
    }

    public void EndCurrentLevel()
    {
        OnLevelComplete?.Invoke(currentLevelIndex);

        if (levelRoutine != null)
        {
            StopCoroutine(levelRoutine);

        }

        currentLevelIndex++;

    }

    //After pressing next level button on level menu
    public void LevelStartCountDown()
    {
        if(countDownRoutine !=null)
        {
            StopCoroutine (countDownRoutine);
        }

        countDownRoutine = StartCoroutine(LevelStartCountDownRoutine());
    }

    IEnumerator LevelStartCountDownRoutine()
    {
        int currentCountDownIndex = 0;

        OnLevelCountDownStart?.Invoke();

        currentCountDownImage.gameObject.SetActive(true);

        while (currentCountDownIndex < totalCountDowns)
        {
            currentCountDownImage.sprite = countDownSprites[currentCountDownIndex];
            currentCountDownImage.GetComponent<CountDown>().ScaleUpDownImage();

            currentCountDownIndex++;
            yield return new WaitForSeconds(intervalBetweenCOuntdown);
        }

        currentCountDownImage.gameObject.SetActive(false);


        StartNextLevel();
    }


}
