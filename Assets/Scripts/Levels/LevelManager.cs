using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField] List<Button> levelButtons;
    [SerializeField] List<Image> highlightArrows;
    [SerializeField] GameObject timerPanel;

    [SerializeField] Material buttonHighlightMaterial;


    [SerializeField] Vector2 timerPanellDisplayPosition;
    [SerializeField] Vector2 timerPanelHidePosition;

    int currentLevelIndex = 0;

    Coroutine levelRoutine;
    Coroutine countDownRoutine;


    private void Awake()
    {
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += HideTitleScreen;
        //GameManager.OnStartGame += LevelStartCountDown;
        AnimalManager.OnZeroSheepOnMap += EndCurrentLevel;
        OnLevelComplete += UnlockLevelButtons;
        //OnNewLevelStart += DisplayInGameUI;
        //OnLevelComplete += HideInGameUI;
            
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= HideTitleScreen;
        //GameManager.OnStartGame -= LevelStartCountDown;
        AnimalManager.OnZeroSheepOnMap -= EndCurrentLevel;
        OnLevelComplete -= UnlockLevelButtons;
        //OnNewLevelStart -= DisplayInGameUI;
        //OnLevelComplete -= HideInGameUI;

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
        //currentLevelIndex++;
        /*yield return new WaitForSeconds(intervalBetweenLevels);*/



    }


    public void StartLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        Debug.Log($"{currentLevelIndex} Level");
        StartLevels(currentLevelIndex);
    }

    public void EndCurrentLevel()
    {
        StartCoroutine(EndCurrentLevelRoutine());


    }

    IEnumerator EndCurrentLevelRoutine()
    {
        yield return null;
        OnLevelComplete?.Invoke(currentLevelIndex);

        if (levelRoutine != null)
        {
            StopCoroutine(levelRoutine);

        }
    }

    //After game start button pressed
    public void LevelStartCountDown()
    {
        LevelStartCountDown(0);
    }

    //After pressing next level button on level menu
    public void LevelStartCountDown(int levelIndex)
    {
        if(countDownRoutine !=null)
        {
            StopCoroutine (countDownRoutine);
        }

        countDownRoutine = StartCoroutine(LevelStartCountDownRoutine(levelIndex));
    }

    IEnumerator LevelStartCountDownRoutine(int levelIndex)
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


        StartLevel(levelIndex);
    }


    void UnlockLevelButtons(int levelIndex)
    {
        if(levelIndex < levelButtons.Count-1)
        {
            levelButtons[levelIndex + 1].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (levelButtons[i].GetComponent<Button>().interactable)
            {
                levelButtons[i].GetComponent<Image>().material = buttonHighlightMaterial;
            }
            else
            {
                levelButtons[i].GetComponent<Image>().material = null;

            }
        }

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if(i < levelButtons.Count-1)
            {
                if (levelButtons[i].interactable && !levelButtons[i+1].interactable)
                {
                    highlightArrows[i].gameObject.SetActive(true);
                }
                else
                {
                    highlightArrows[i].gameObject.SetActive(false);

                }
            }
            else
            {
                if (levelButtons[i].interactable)
                {
                    highlightArrows[i].gameObject.SetActive(true);
                }
                else
                {
                    highlightArrows[i].gameObject.SetActive(false);
                }
            }
        }



    }


    void BlurBackground()
    {
        
    }


    void UnBlurBackground()
    {

    }

    /*void HideInGameUI(int levelIndex)
    {
        timerPanel.GetComponent<UIPanelMove>().MoveImageTo(timerPanelHidePosition);
    }

    void DisplayInGameUI(int levelIndex)
    {
        timerPanel.GetComponent<UIPanelMove>().MoveImageTo(timerPanellDisplayPosition);

    }*/

}
