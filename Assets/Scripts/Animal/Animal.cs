using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : NPC
{
    public static event Action OnEatenByWolf;

    [SerializeField] AnimalSO animalSO;

    public AnimalSO AnimalSO => animalSO;

    //Animal capture and cage related variables
    bool canBeCapturedInCage;
    public Vector2 PickupPosition { get; private set; }
    public bool IsInCage { get; private set; }

    public bool IsGrabbed { get; private set; }
    Cage targetCage;


   


    //movement modification variables
    bool canSpeedUpAnimal = true;
    Coroutine boostCoroutine;


    private void Awake()
    {
        canMove = true;

        movementSpeed = animalSO.speed;
        lastPosition = transform.position;

    }


    #region movement variations

    public void SpeedUpAnimal()
    {
        if(canSpeedUpAnimal)
        {
            Debug.Log("Speeding up animal: " + name);
            canSpeedUpAnimal = false;
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue <= animalSO.chanceToBoostSpeed)
            {
                movementSpeed = animalSO.speed * (1 + animalSO.boostSpeedProportion);

                if (boostCoroutine != null)
                {
                    StopCoroutine(boostCoroutine);
                }

                boostCoroutine = StartCoroutine(BoostSpeedRoutine());
            }
        }

    }


    IEnumerator BoostSpeedRoutine()
    {
        yield return new WaitForSeconds(animalSO.boostDuration);
        movementSpeed = animalSO.speed;
    }


    public void ResetSpeedUpStatus()
    {
        canSpeedUpAnimal = true;
    }


    #endregion

    #region Animal capture and cage related methods
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
                Debug.Log("Animal put in correct cage: " + name);
                IsInCage = true;
                targetCage.CaptureAnimal(this);
            }
            else
            {
                Debug.Log("Animal put in wrong cage: " + name);
                IsInCage = true;
                targetCage.DestroyAnimal(this);
                
            }
                
        }
        else
        {
            ToggleAnimalMovement(true);
            SetGrabbedStatus(false);
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

    public void SetGrabbedStatus(bool status)
    {
        IsGrabbed = status;
    }


    public void GetEatenByWolf()
    {
        OnEatenByWolf?.Invoke();
        Destroy(this.gameObject);
    }

    #endregion

    
}
