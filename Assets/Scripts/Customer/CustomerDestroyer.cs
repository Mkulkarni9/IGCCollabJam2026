using UnityEngine;

public class CustomerDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Customer customer = collision.GetComponent<Customer>();


        if (customer != null)
        {
            Destroy(customer.gameObject);
        }
    }
}
