using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VectorGraphics;
using UnityEngine;

public class Cage : MonoBehaviour
{

    public static event Action<Animal, Cage> OnAnimalCapturedInCorrectCage;
    public static event Action<Animal, Cage> OnAnimalCapturedInWrongCage;
    [SerializeField] CageSO cageSO;

    [SerializeField] Material cageBaseMaterial;
    [SerializeField] Material cageHoverMaterial;
    [SerializeField] ParticleSystem wrongCageVFX;


    public CageSO CageSO => cageSO;

    public Vector2 CagePosition { get; private set; }

    bool canBeDeliveredToCustomer;

    Customer targetCustomer;

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;


    SpriteRenderer[] spriteRenderers;
    Animator[] animators;
    CageComponent[] cageComponents;
    CinemachineImpulseSource cinemachineImpulseSource;

    private void Start()
    {
        CagePosition = transform.position;
        UpdateWalkableTile();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        animators = GetComponentsInChildren<Animator>();
        cageComponents = GetComponentsInChildren<CageComponent>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    public void SetPathfindingVariablesToCage(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding)
    {
        this.pathFindingGrid = pathFindingGrid;
        this.pathfinding = pathfinding;
    }


    void UpdateWalkableTile()
    {
        PathNode currentNode = pathFindingGrid.GetGridObject(this.transform.position);
        //Debug.Log("Cage node for cage: "+this.cageSO.animalCageType +": "+ currentNode);
        currentNode.SetWalkable(false);
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        Animal grabbedAnimalInPointer = collision.GetComponent<Animal>();


        if (grabbedAnimalInPointer != null)
        {
            //Debug.Log("Grabbed animal in pointer entered cage area: " + grabbedAnimalInPointer.AnimalSO.animalType);
            grabbedAnimalInPointer.SetCaptureStatus(true);
            grabbedAnimalInPointer.SetTargetCage(this);


            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.material = cageHoverMaterial;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Animal grabbedAnimalInPointer = collision.GetComponent<Animal>();

        if (grabbedAnimalInPointer != null)
        {
            //Debug.Log("Grabbed animal in pointer exited cage area: " + grabbedAnimalInPointer.AnimalSO.animalType);
            grabbedAnimalInPointer.SetCaptureStatus(false);
            grabbedAnimalInPointer.SetTargetCage(null);

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.material = cageBaseMaterial;
            }
        }
    }


    public void CaptureAnimal(Animal animal)
    {
        animal.transform.position = this.transform.position;
        animal.transform.SetParent(this.transform);
        Debug.Log("Captured: " + animal.name);
        //animal.ToggleAnimalMovement(false);

        animal.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;

        OnAnimalCapturedInCorrectCage?.Invoke(animal, this);

        foreach (CageComponent cageComponent in cageComponents)
        {
            cageComponent.PlayCorrectCageVFX();
        }

        Destroy(animal.gameObject);

        foreach (Animator animator in animators)
        {
            animator.SetBool("IsClosing", true);
        }

    }

    public void DestroyCage()
    {
        Destroy(this.gameObject);
    }


    public void DestroyAnimal(Animal animal)
    {

        //Debug.Log("Destroying animal after putting in wrong cage: " + animal.name);

        wrongCageVFX.Play();
        cinemachineImpulseSource.GenerateImpulse(1f);
        OnAnimalCapturedInWrongCage?.Invoke(animal,this);


        Destroy(animal.gameObject);
    }


    public void SetCagePosition(Vector2 position)
    {
        CagePosition = position;
    }

    public void SetCageDeliveryStatus(bool status)
    {
        canBeDeliveredToCustomer = status;
    }

    public void SetTargetCustomer(Customer customer)
    {
        targetCustomer = customer;

        if(customer!=null)
        {
            customer.GetComponent<SpriteRenderer>().color = Color.green;
        }
        
    }

    public void DeliverCageToCustomer()
    {
        if (canBeDeliveredToCustomer)
        {
            Debug.Log("Cage delivered to customer: " + targetCustomer);
            targetCustomer.DeliverCage(this);


        }
        else
        {
            this.transform.position = CagePosition;
            Debug.Log("Cage cannot be placed here" );
        }

    }
}
