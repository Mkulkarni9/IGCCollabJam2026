using UnityEngine;
using UnityEngine.InputSystem;

public class PointerGrabber : MonoBehaviour
{

    public bool IsGrabbingAnimal { get; private set; }


    bool canGrabAnimal;
    bool canGrabCrate;


    Vector2 mousePos;
    Camera mainCam;
    float initialScreenZ;
    PlayerController playerController;

    Animal grabbedAnimal;
    Animal grabbableAnimal;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        mainCam = Camera.main;
        initialScreenZ = mainCam.WorldToScreenPoint(transform.position).z;
        playerController =GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {

        mousePos = Mouse.current.position.ReadValue();
        Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, initialScreenZ);
        Vector3 targetWorldPos = mainCam.ScreenToWorldPoint(screenPoint);

        
        transform.position = targetWorldPos;


        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a Rigidbody
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Grab the object by making it a child of the pointer
                    rb.isKinematic = true; // Make it kinematic to prevent physics interference
                    rb.transform.SetParent(transform);
                    Debug.Log("Grabbed: " + hit.collider.name);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Release the object by unparenting it and making it non-kinematic
            foreach (Transform child in transform)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // Allow physics to affect the object again
                    child.SetParent(null); // Unparent the object
                    Debug.Log("released: " );
                }
            }
        }*/
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();

        if(animal!=null)
        {
            canGrabAnimal = true;
            grabbableAnimal = animal;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

            Debug.Log("Can grab animal: " + grabbableAnimal.name);

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

            
    }

    public void GrabAnimal()
    {
        if(canGrabAnimal)
        {            
            Rigidbody2D rb = grabbableAnimal.GetComponent<Rigidbody2D>();
            Debug.Log("Rigid body of grabbed animal: "+ grabbableAnimal.AnimalSO.animalType + " = " + rb);
            if (rb != null)
            {
                // Grab the object by making it a child of the pointer
                //rb.is= true; // Make it kinematic to prevent physics interference
                rb.transform.SetParent(transform);
                rb.transform.position = transform.position;
                grabbedAnimal = grabbableAnimal;
                IsGrabbingAnimal = true;
                grabbedAnimal.SetPickupPosition(transform.position);
                Debug.Log("Grabbed: " + grabbedAnimal.AnimalSO.animalType);
            }
            
        }
        else
        {
            Debug.Log("Tried to grab animal but was not in range");
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
}
