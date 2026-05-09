using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<int> OnLevelComplete;
    public static event Action<int> OnNewLevelStart;
    public static event Action<float> OnNewLevelTimerUpdate;


    [SerializeField] List<LevelSO> levels = new List<LevelSO>();
    //[SerializeField] float intervalBetweenLevels;

    int currentLevelIndex;

    Coroutine levelRoutine;

    private void OnEnable()
    {
        AnimalManager.OnZeroSheepOnMap += EndCurrentLevel;
    }

    private void OnDisable()
    {
        AnimalManager.OnZeroSheepOnMap -= EndCurrentLevel;

    }

    private void Start()
    {
        StartLevels(0);
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


    //After pressing next level button on level menu
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


}
