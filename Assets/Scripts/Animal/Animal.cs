using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animal : NPC
{
    public static event Action OnEatenByWolf;
    public static event Action OnHoverPointer;

    [SerializeField] AnimalSO animalSO;

    [SerializeField] Material baseMaterial;
    [SerializeField] Material grabbedMaterial;

    [SerializeField] GameObject hoverHighlight;
    [SerializeField] GameObject animalShadow;
    [SerializeField] ParticleSystem sheepGrabVFX;

    public AnimalSO AnimalSO => animalSO;

    //Animal capture and cage related variables
    bool canBeCapturedInCage;
    public Vector2 PickupPosition { get; private set; }
    public bool IsInCage { get; private set; }

    public bool IsGrabbed { get; private set; }
    Cage targetCage;

    Animator[] animators;
    SpriteRenderer[] spriteRenderers;
   


    //movement modification variables
    bool canSpeedUpAnimal = true;
    Coroutine boostCoroutine;


    private void Awake()
    {
        canMove = true;

        movementSpeed = animalSO.speed;
        lastPosition = transform.position;

        animators = GetComponentsInChildren<Animator>().Where(c => c.gameObject != gameObject).ToArray();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>().Where(c => c.gameObject != gameObject).ToArray();

    }


    #region movement variations

    public void SpeedUpAnimal()
    {
        if(canSpeedUpAnimal)
        {
            //Debug.Log("Speeding up animal: " + name);
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

            OnHoverPointer?.Invoke();
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
            //Debug.Log("Animal can be put in cage: "+ canBeCapturedInCage);
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

        foreach (Animator animator in animators)
        {
            if(animator.gameObject.activeSelf)
            {
                animator.SetBool("IsGrabbed", status);
            }
            
        }

        //Setting material after changing grab status
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if(status)
            {
                spriteRenderer.material = grabbedMaterial;
            }
            else
            {
                spriteRenderer.material = baseMaterial;
            }
            
        }


    }


    public void GetEatenByWolf()
    {
        OnEatenByWolf?.Invoke();
        Destroy(this.gameObject);
    }

    #endregion


    #region effects

    public void PlaySheepGrabVFX()
    {
        sheepGrabVFX.Play();
    }

    #endregion

}
