using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] List<int> maxLevelScores;
    [SerializeField] List<Sprite> scoreEmojis;
    [SerializeField] int pointsOnCorrectCapture;
    [SerializeField] int penaltyOnWrongCapture;
    [SerializeField] int penaltyOnWolfEat;


    [SerializeField] Image scoreBar;
    [SerializeField] Image scoreEmoji;
    [SerializeField] Slider scoreSlider;
    [SerializeField] List<Image> levelWiseEmojiScores;
    [SerializeField] Image levelScorePanel;
    [SerializeField] GameObject inGameScorePanel;

    [SerializeField] Vector2 levelPanelDisplayPosition;
    [SerializeField] Vector2 levelPanelHidePosition;

    [SerializeField] Vector2 inGameScorePanellDisplayPosition;
    [SerializeField] Vector2 inGameScorePanelHidePosition;

    public int TotalLevelScore { get; private set; }

    int maxLevelScore;
    private void OnEnable()
    {
        LevelManager.OnLevelCountDownStart += HideLevelScorePanel;
        LevelManager.OnLevelCountDownStart += ResetLevelScore;
        //LevelManager.OnLevelCountDownStart += HideInGameScoreUI;
        LevelManager.OnNewLevelStart += SetMaxLevelScore;
        LevelManager.OnNewLevelStart += DisplayInGameScoreUI;

        Cage.OnAnimalCapturedInCorrectCage += UpdateScoreAfterAnimalCapture;
        Cage.OnAnimalCapturedInWrongCage += UpdateScoreAfterAnimalCapture;
        Animal.OnEatenByWolf += UpdateScoreAfterWolfEat;

        LevelManager.OnLevelComplete += DisplayLevelScorePanel;
        LevelManager.OnLevelComplete += UpdateLevelScoreEmojis;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelCountDownStart -= HideLevelScorePanel;
        LevelManager.OnLevelCountDownStart -= ResetLevelScore;
        //LevelManager.OnLevelCountDownStart -= HideInGameScoreUI;
        LevelManager.OnNewLevelStart -= DisplayInGameScoreUI;
        LevelManager.OnNewLevelStart -= SetMaxLevelScore;

        Cage.OnAnimalCapturedInCorrectCage -= UpdateScoreAfterAnimalCapture;
        Cage.OnAnimalCapturedInWrongCage -= UpdateScoreAfterAnimalCapture;
        Animal.OnEatenByWolf -= UpdateScoreAfterWolfEat;

        LevelManager.OnLevelComplete -= DisplayLevelScorePanel;
        LevelManager.OnLevelComplete -= UpdateLevelScoreEmojis;

    }

    void UpdateScoreAfterAnimalCapture(Animal animal, Cage cage)
    {

        if (animal.AnimalSO.animalType == cage.CageSO.animalCageType)
        {
            TotalLevelScore += pointsOnCorrectCapture;
        }
        else
        {
            TotalLevelScore += penaltyOnWrongCapture;
            TotalLevelScore = Mathf.Max(-maxLevelScore, TotalLevelScore);
        }


        UpdateScoreUI();

    }

    public void UpdateScoreAfterWolfEat()
    {
        TotalLevelScore += penaltyOnWolfEat;
        TotalLevelScore = Mathf.Max(-maxLevelScore, TotalLevelScore);

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        float fillAmount = (float)TotalLevelScore / maxLevelScore;

        scoreSlider.value = fillAmount;

        Debug.Log("Total level score:" + TotalLevelScore + " / " + "max level score: " + maxLevelScore);

        SelectScoreEmoji(fillAmount);
    }


    #region Level score
    void DisplayLevelScorePanel(int levelIndex)
    {
        levelScorePanel.GetComponent<UIPanelMove>().MoveImageTo(levelPanelDisplayPosition);
    }

    void HideLevelScorePanel()
    {
        
        levelScorePanel.GetComponent<UIPanelMove>().MoveImageTo(levelPanelHidePosition);

    }

    void UpdateLevelScoreEmojis(int levelIndex)
    {
        UpdateScoreUI();
        Debug.Log("Score emoji: " + scoreEmoji.sprite);
        levelWiseEmojiScores[levelIndex].gameObject.SetActive(true);
        levelWiseEmojiScores[levelIndex].sprite = scoreEmoji.sprite;
    }

    

    void ResetLevelScore()
    {
        Debug.Log("Total level score: " + TotalLevelScore);
        TotalLevelScore = 0;

    }

    void HideInGameScoreUI()
    {
        inGameScorePanel.GetComponent<UIPanelMove>().MoveImageTo(inGameScorePanelHidePosition);
    }

    void DisplayInGameScoreUI(int levelIndex)
    {
        inGameScorePanel.GetComponent<UIPanelMove>().MoveImageTo(inGameScorePanellDisplayPosition);
        UpdateScoreUI();
    }

    #endregion



    void SetMaxLevelScore(int levelIndex)
    {
        if(levelIndex< maxLevelScores.Count)
        {
            maxLevelScore = maxLevelScores[levelIndex];
        }

        UpdateScoreUI();

    }

    void SelectScoreEmoji(float proportionOfTotal)
    {
        if (proportionOfTotal > 0.8f)
        {
            scoreEmoji.sprite = scoreEmojis[4];
        }
        else if (proportionOfTotal > 0.2f)
        {
            scoreEmoji.sprite = scoreEmojis[3];

        }
        else if (proportionOfTotal > -0.2f)
        {
            scoreEmoji.sprite = scoreEmojis[2];

        }
        else if (proportionOfTotal > -0.6f)
        {
            scoreEmoji.sprite = scoreEmojis[1];

        }
        else
        {
            scoreEmoji.sprite = scoreEmojis[0];

        }

        Debug.Log("Score emoji : " + scoreEmoji.sprite);
    }


}
