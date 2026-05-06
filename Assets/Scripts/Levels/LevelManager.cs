using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<int> OnLevelComplete;
    public static event Action<int> OnNewLevelStart;


    [SerializeField] List<LevelSO> levels = new List<LevelSO>();
    [SerializeField] float intervalBetweenLevels;



    Coroutine levelRoutine;

    

    private void Start()
    {
        StartLevels();
    }


    void StartLevels()
    {
        if(levelRoutine!=null)
        {
            StopCoroutine(levelRoutine);
        }

        levelRoutine = StartCoroutine(LevelRoutine());
    }


    IEnumerator LevelRoutine()
    {

        for (int i = 0; i < levels.Count; i++)
        {
            Debug.Log("Starting level: " + i);
            OnNewLevelStart?.Invoke(i);
            yield return new WaitForSeconds(levels[i].levelDuration);

            OnLevelComplete?.Invoke(i);
            yield return new WaitForSeconds(intervalBetweenLevels);

        }



    }

}
