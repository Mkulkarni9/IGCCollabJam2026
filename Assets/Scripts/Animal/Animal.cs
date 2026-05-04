using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] AnimalSO animalSO;

    public AnimalSO AnimalSO => animalSO;


    bool canBeCapturedInCage;
    bool canMove;
    public Vector2 PickupPosition { get; private set; }
    public bool IsInCage { get; private set; }

    Cage targetCage;

    private void Start()
    {
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.up * animalSO.speed * Time.deltaTime);
        }

    }

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
                IsInCage = true;
                targetCage.CaptureAnimal(this);
            }
            else
            {
                this.transform.position = PickupPosition;
                ToggleAnimalMovement(true);
                Debug.Log("Animal cannot be put in cage: " + name + " because cage type is " + targetCage.CageSO.animalCageType + " and animal type is " + animalSO.animalType);
            }
                
        }
        else
        {
            ToggleAnimalMovement(true);
            Debug.Log("Animal can be put in cage: "+ canBeCapturedInCage);
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

    public void ToggleAnimalMovement(bool status)
    {
        canMove = status;
    }


}
