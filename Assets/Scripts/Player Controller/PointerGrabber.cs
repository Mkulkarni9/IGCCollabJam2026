using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System;

public class PointerGrabber : MonoBehaviour
{

    public static event Action<bool> OnGrabbedSheep;
    [SerializeField] Sprite pickupSprite;
    [SerializeField] Sprite hammerSprite;
    public bool IsGrabbingAnimal { get; private set; }
    public bool IsGrabbingCage { get; private set; }


    bool canGrabAnimal;
    bool canGrabCage;
    bool canHitWolf;


    Vector2 mousePos;
    Camera mainCam;
    float initialScreenZ;
    PlayerController playerController;

    Animal grabbedAnimal;
    Animal grabbableAnimal;
    Cage grabbableCage;
    Cage grabbedCage;
    Wolf hittableWolf;
    SpriteRenderer spriteRenderer;
    AnimalManager animalManager;

    CircleCollider2D pointerCollider;
    Vector3 pointerOffset;

    Animator animator;
    CinemachineImpulseSource cinemachineImpulseSource;


    private void Awake()
    {
        mainCam = Camera.main;
        initialScreenZ = mainCam.WorldToScreenPoint(transform.position).z;
        playerController =GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pointerCollider = GetComponent<CircleCollider2D>();
        pointerOffset = pointerCollider.offset;
        animalManager = FindAnyObjectByType<AnimalManager>();
        animator = GetComponent<Animator>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {

        mousePos = Mouse.current.position.ReadValue();
        Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, initialScreenZ);
        Vector3 targetWorldPos = mainCam.ScreenToWorldPoint(screenPoint);

        
        transform.position = targetWorldPos;

    }


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();

        if(animal!=null)
        {
            canGrabAnimal = true;
            grabbableAnimal = animal;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

            Debug.Log("Can grab animal: " + grabbableAnimal.name);

        }
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();
        if (animal != null)
        {
            if(!IsGrabbingAnimal)
            {
                canGrabAnimal = true;
                grabbableAnimal = animal;
                spriteRenderer.sprite = pickupSprite;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
                grabbableAnimal.SpeedUpAnimal();


                //Debug.Log("Can grab animal: " + grabbableAnimal.name);
            }

        }

        /*Cage cage = collision.GetComponent<Cage>();

        if (cage != null)
        {
            canGrabCage = true;
            grabbableCage = cage;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            //Debug.Log("Can grab cage: " + cage.CageSO.animalCageType);
        }*/

        Wolf wolf = collision.GetComponent<Wolf>();

        if (wolf != null && !IsGrabbingAnimal)
        {
            if (wolf.CanBeStunned)
            {
                canHitWolf = true;
                hittableWolf = wolf;
                spriteRenderer.sprite = hammerSprite;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }

        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();

        if (animal != null)
        {
            canGrabAnimal = false;
            spriteRenderer.sprite = null;
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);

            if(grabbableAnimal!=null)
            {
                grabbableAnimal.ResetSpeedUpStatus();
                

            }

            grabbableAnimal = null;
        }

        /*Cage cage = collision.GetComponent<Cage>();


        if (cage != null)
        {
            canGrabCage = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);
            grabbableCage = null;
        }*/

        Wolf wolf = collision.GetComponent<Wolf>();

        if (wolf != null)
        {
            
            canHitWolf = false;
            spriteRenderer.sprite = null;
            hittableWolf = null;
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);

        }


    }

    

    public void GrabAnimal()
    {
        if(canGrabAnimal && !grabbableAnimal.IsInCage)
        {            
            Rigidbody2D rb = grabbableAnimal.GetComponent<Rigidbody2D>();
            //Debug.Log("Rigid body of grabbed animal: "+ grabbableAnimal.AnimalSO.animalType + " = " + rb);
            if (rb != null)
            {
                // Grab the object by making it a child of the pointer
                //rb.is= true; // Make it kinematic to prevent physics interference
                rb.transform.SetParent(transform);
                rb.transform.position = transform.position + pointerOffset;
                grabbedAnimal = grabbableAnimal;
                IsGrabbingAnimal = true;
                grabbedAnimal.ToggleAnimalMovement(false);
                grabbedAnimal.SetGrabbedStatus(true);
                grabbedAnimal.SetPickupPosition(transform.position);
                grabbedAnimal.GetComponentInChildren<SheepHoverHighlight>().ToggleHighlight(false);
                grabbedAnimal.GetComponentInChildren<AnimalShadow>().ToggleHighlight(false);
                grabbedAnimal.PlaySheepGrabVFX();
                cinemachineImpulseSource.GenerateImpulse(0.8f);

                OnGrabbedSheep?.Invoke(true);

                //Debug.Log("Grabbed: " + grabbedAnimal.AnimalSO.animalType);
            }

        }
        

        

    }

    public void HitWolf()
    {
        if (canHitWolf)
        {
            hittableWolf.StunWolf();
        }
    }

    public void GrabCage()
    {
        if (canGrabCage)
        {
            Rigidbody2D rb = grabbableCage.GetComponent<Rigidbody2D>();
            Debug.Log("Rigid body of grabbed cage: " + grabbableCage.CageSO.animalCageType + " = " + rb);
            if (rb != null)
            {
                rb.transform.SetParent(transform);
                rb.transform.position = transform.position;
                grabbedCage = grabbableCage;
                IsGrabbingCage = true;
                //grabbedCage.SetCagePosition(transform.position);
                //Debug.Log("Grabbed: " + grabbedCage.CageSO.animalCageType);
            }
        }
    }

    public void ReleaseAnimal()
    {
        if (grabbedAnimal != null)
        {
            Rigidbody2D rb = grabbedAnimal.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.transform.SetParent(animalManager.transform);
                Debug.Log("Released: " + grabbedAnimal.AnimalSO.animalType);
                grabbedAnimal.GetComponentInChildren<AnimalShadow>().ToggleHighlight(true);

                grabbedAnimal.PutAnimalInCage();
                IsGrabbingAnimal = false;

                grabbedAnimal = null;
                OnGrabbedSheep?.Invoke(false);

            }
        }
    }

    public void ReleaseCage()
    {
        if (grabbedCage != null)
        {
            Rigidbody2D rb = grabbedCage.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.transform.SetParent(null);
                //Debug.Log("Released: " + grabbedCage.CageSO.animalCageType);

                grabbedCage.DeliverCageToCustomer();
                IsGrabbingCage = false;

                grabbedCage = null;
            }
        }
    }
}
