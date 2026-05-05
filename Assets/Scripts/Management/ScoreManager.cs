using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] int pointsOnCorrectCapture;
    [SerializeField] int penaltyOnWrongCapture;


    public int TotalPlayerScore {  get; private set; }
    public int TotalLevelScore {  get; private set; }


    private void OnEnable()
    {
        Cage.OnAnimalCapturedInCorrectCage += UpdateScore;
        Cage.OnAnimalCapturedInWrongCage += UpdateScore;
        LevelManager.OnLevelComplete += ResetLevelScore;
    }

    private void OnDisable()
    {
        Cage.OnAnimalCapturedInCorrectCage -= UpdateScore;
        Cage.OnAnimalCapturedInWrongCage -= UpdateScore;
        LevelManager.OnLevelComplete -= ResetLevelScore;

    }

    void UpdateScore(Animal animal, Cage cage)
    {

        if(animal.AnimalSO.animalType == cage.CageSO.animalCageType)
        {
            TotalLevelScore += pointsOnCorrectCapture;
            TotalPlayerScore += pointsOnCorrectCapture;
        }
        else
        {
            TotalLevelScore += penaltyOnWrongCapture;
            TotalPlayerScore += penaltyOnWrongCapture;
        }

            
    }


    void ResetLevelScore(int levelIndex)
    {
        Debug.Log("Total level score: "+ TotalLevelScore + " & Total player score: "+ TotalPlayerScore);
        TotalLevelScore = 0;
    }


}
