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

    public int TotalPlayerScore { get; private set; }
    public int TotalLevelScore { get; private set; }

    int maxLevelScore;
    private void OnEnable()
    {
        LevelManager.OnNewLevelStart += SetMaxLevelScore;
        LevelManager.OnNewLevelStart += HideLevelScorePanel;
        Cage.OnAnimalCapturedInCorrectCage += UpdateScoreAfterAnimalCapture;
        Cage.OnAnimalCapturedInWrongCage += UpdateScoreAfterAnimalCapture;
        LevelManager.OnLevelComplete += DisplayLevelScorePanel;
        LevelManager.OnLevelComplete += UpdateLevelScoreEmojis;
        LevelManager.OnLevelComplete += ResetLevelScore;
    }

    private void OnDisable()
    {
        LevelManager.OnNewLevelStart -= SetMaxLevelScore;
        LevelManager.OnNewLevelStart -= HideLevelScorePanel;
        Cage.OnAnimalCapturedInCorrectCage -= UpdateScoreAfterAnimalCapture;
        Cage.OnAnimalCapturedInWrongCage -= UpdateScoreAfterAnimalCapture;
        LevelManager.OnLevelComplete -= DisplayLevelScorePanel;
        LevelManager.OnLevelComplete -= UpdateLevelScoreEmojis;
        LevelManager.OnLevelComplete -= ResetLevelScore;

    }

    void UpdateScoreAfterAnimalCapture(Animal animal, Cage cage)
    {

        if (animal.AnimalSO.animalType == cage.CageSO.animalCageType)
        {
            TotalLevelScore += pointsOnCorrectCapture;
            TotalPlayerScore += pointsOnCorrectCapture;
        }
        else
        {
            TotalLevelScore += penaltyOnWrongCapture;
            TotalPlayerScore += penaltyOnWrongCapture;
        }


        UpdateScoreUI();

    }

    public void UpdateScoreAfterWolfEat()
    {
        TotalLevelScore += penaltyOnWolfEat;
        TotalPlayerScore += penaltyOnWolfEat;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        float fillAmount = (float)TotalLevelScore / maxLevelScore;

        scoreSlider.value = fillAmount;

        SelectScoreEmoji(fillAmount);
    }


    #region Level score
    void DisplayLevelScorePanel(int levelIndex)
    {
        levelScorePanel.gameObject.SetActive(true);
    }

    void HideLevelScorePanel(int levelIndex)
    {
        levelScorePanel.gameObject.SetActive(false);

    }

    void UpdateLevelScoreEmojis(int levelIndex)
    {
        levelWiseEmojiScores[levelIndex].gameObject.SetActive(true);
        levelWiseEmojiScores[levelIndex].sprite = scoreEmoji.sprite;
    }

    void ResetLevelScore(int levelIndex)
    {
        Debug.Log("Total level score: " + TotalLevelScore + " & Total player score: " + TotalPlayerScore);
        TotalLevelScore = 0;

        UpdateScoreUI();
    }

    #endregion



    void SetMaxLevelScore(int levelIndex)
    {
        maxLevelScore = maxLevelScores[levelIndex];
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
    }


}
