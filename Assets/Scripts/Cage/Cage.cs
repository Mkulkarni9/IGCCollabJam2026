using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] CageSO cageSO;


    public CageSO CageSO => cageSO;

    public Vector2 CagePosition { get; private set; }

    bool canBeDeliveredToCustomer;

    Customer targetCustomer;


    private void Start()
    {
        CagePosition = transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Animal grabbedAnimalInPointer = collision.GetComponent<Animal>();


        if (grabbedAnimalInPointer != null)
        {
            //Debug.Log("Grabbed animal in pointer entered cage area: " + grabbedAnimalInPointer.AnimalSO.animalType);
            grabbedAnimalInPointer.SetCaptureStatus(true);
            grabbedAnimalInPointer.SetTargetCage(this);

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
        }
    }


    public void CaptureAnimal(Animal animal)
    {
        animal.transform.position = this.transform.position;
        animal.transform.SetParent(this.transform);
        Debug.Log("Captured: " + animal.name);
        //animal.ToggleAnimalMovement(false);

        animal.GetComponent<BoxCollider2D>().enabled = false;
        //Destroy(animal.gameObject);
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
