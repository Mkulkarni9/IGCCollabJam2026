using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] AnimalSO animalSO;

    public AnimalSO AnimalSO => animalSO;


    bool canBeCapturedInCage;
    bool isInCage;
    public Vector2 PickupPosition { get; private set; }

    Cage targetCage;


    public void SetCaptureStatus(bool status)
    {
        canBeCapturedInCage = status;
    }

    public void PutAnimalInCage()
    {
        if (canBeCapturedInCage )
        {
            if(targetCage.CageSO.animalCageType == animalSO.animalType)
            {
                Debug.Log("Animal put in cage: " + name);
                isInCage = true;
                targetCage.CaptureAnimal(this);
            }
            else
            {
                this.transform.position = PickupPosition;
                Debug.Log("Animal cannot be put in cage: " + name + " because cage type is " + targetCage.CageSO.animalCageType + " and animal type is " + animalSO.animalType);
            }
                
        }
        
    }

    public void SetTargetCage(Cage cage)
    {
        targetCage = cage;
    }

    public void SetPickupPosition(Vector2 position)
    {
        PickupPosition = position;
    }

}
