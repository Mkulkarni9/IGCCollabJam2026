using System;
using UnityEngine;

public class Customer : MonoBehaviour
{

    public static event Action OnDeliveredCage;

    [SerializeField] CustomerSO customerSO;

    bool isCustomerReached;
    bool isOrderComplete;
    float movementSpeed;

    Vector3 targetPoint;
    Vector3 movementDirection;

    SpriteRenderer spriteRenderer;
    Color baseColor;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }
    private void Start()
    {
        movementSpeed = customerSO.movementSpeed;
        float randomYValue = UnityEngine.Random.Range(-5f, 5f);
        targetPoint = new Vector3(0f, randomYValue,0f);
        movementDirection = (targetPoint - transform.position).normalized;
    }

    private void Update()
    {
        if(!isCustomerReached || isOrderComplete)
        {
            transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cage grabbedCageInPointer = collision.GetComponent<Cage>();


        if (grabbedCageInPointer != null)
        {
            Debug.Log("Grabbed cage in pointer: " + grabbedCageInPointer.CageSO.animalCageType);
            grabbedCageInPointer.SetCageDeliveryStatus(true);
            grabbedCageInPointer.SetTargetCustomer(this);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Cage grabbedCageInPointer = collision.GetComponent<Cage>();

        if (grabbedCageInPointer != null)
        {
            grabbedCageInPointer.SetCageDeliveryStatus(false);
            grabbedCageInPointer.SetTargetCustomer(null);
        }

        spriteRenderer.color = baseColor;
    }


    public void DeliverCage(Cage cage)
    {
        cage.transform.position = this.transform.position;
        cage.transform.SetParent(this.transform);
        Debug.Log("Delivered: " + cage.CageSO.animalCageType);
        cage.GetComponent<BoxCollider2D>().enabled = false;

        isOrderComplete = true;
        OnDeliveredCage?.Invoke();

        CalculateCustomerSatisfaction();
        ReverseDirection();

    }

    public void StopCustomerMovement()
    {
        isCustomerReached = true;
    }

    public void ReverseDirection()
    {
        movementSpeed *= -1;
       
    }

    #region Customer Satisfaction System
    void CalculateCustomerSatisfaction()
    {

    }


    #endregion
}
