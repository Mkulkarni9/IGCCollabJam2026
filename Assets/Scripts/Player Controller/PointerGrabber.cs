using UnityEngine;
using UnityEngine.InputSystem;

public class PointerGrabber : MonoBehaviour
{

    public bool IsGrabbingAnimal { get; private set; }
    public bool IsGrabbingCage { get; private set; }


    bool canGrabAnimal;
    bool canGrabCage;


    Vector2 mousePos;
    Camera mainCam;
    float initialScreenZ;
    PlayerController playerController;

    Animal grabbedAnimal;
    Animal grabbableAnimal;
    Cage grabbableCage;
    Cage grabbedCage;
    SpriteRenderer spriteRenderer;

    CircleCollider2D pointerCollider;
    Vector3 pointerOffset;


    private void Awake()
    {
        mainCam = Camera.main;
        initialScreenZ = mainCam.WorldToScreenPoint(transform.position).z;
        playerController =GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pointerCollider = GetComponent<CircleCollider2D>();
        pointerOffset = pointerCollider.offset;
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
            canGrabAnimal = true;
            grabbableAnimal = animal;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            //Debug.Log("Can grab animal: " + grabbableAnimal.name);
        }

        Cage cage = collision.GetComponent<Cage>();

        if (cage != null)
        {
            canGrabCage = true;
            grabbableCage = cage;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            //Debug.Log("Can grab cage: " + cage.CageSO.animalCageType);
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();

        if (animal != null)
        {
            canGrabAnimal = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);
            grabbableAnimal = null;
        }

        Cage cage = collision.GetComponent<Cage>();


        if (cage != null)
        {
            canGrabCage = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);
            grabbableCage = null;
        }


    }

    

    public void GrabAnimal()
    {
        if(canGrabAnimal && !grabbableAnimal.IsInCage)
        {            
            Rigidbody2D rb = grabbableAnimal.GetComponent<Rigidbody2D>();
            Debug.Log("Rigid body of grabbed animal: "+ grabbableAnimal.AnimalSO.animalType + " = " + rb);
            if (rb != null)
            {
                // Grab the object by making it a child of the pointer
                //rb.is= true; // Make it kinematic to prevent physics interference
                rb.transform.SetParent(transform);
                rb.transform.position = transform.position + pointerOffset;
                grabbedAnimal = grabbableAnimal;
                IsGrabbingAnimal = true;
                grabbedAnimal.ToggleAnimalMovement(false);
                grabbedAnimal.SetPickupPosition(transform.position);
                Debug.Log("Grabbed: " + grabbedAnimal.AnimalSO.animalType);
            }
            
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
                Debug.Log("Grabbed: " + grabbedCage.CageSO.animalCageType);
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
                rb.transform.SetParent(null);
                Debug.Log("Released: " + grabbedAnimal.AnimalSO.animalType);

                grabbedAnimal.PutAnimalInCage();
                IsGrabbingAnimal = false;

                grabbedAnimal = null;
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
                Debug.Log("Released: " + grabbedCage.CageSO.animalCageType);

                grabbedCage.DeliverCageToCustomer();
                IsGrabbingCage = false;

                grabbedCage = null;
            }
        }
    }
}
