using UnityEngine;

public class CustomerStopper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Customer customer = collision.GetComponent<Customer>();


        if (customer != null)
        {
            customer.StopCustomerMovement();
        }
    }
}
