using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] CageSO cageSO;

    public CageSO CageSO => cageSO;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animal grabbedAnimalInPointer = collision.GetComponent<Animal>();
        Debug.Log("Pointer grabber in cage trigger");



        if (grabbedAnimalInPointer != null)
        {
            Debug.Log("Grabbed animal in pointer: " + grabbedAnimalInPointer.AnimalSO.animalType);
            grabbedAnimalInPointer.SetCaptureStatus(true);
            grabbedAnimalInPointer.SetTargetCage(this);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Animal grabbedAnimalInPointer = collision.GetComponent<Animal>();

        if (grabbedAnimalInPointer != null)
        {
            grabbedAnimalInPointer.SetCaptureStatus(false);
            grabbedAnimalInPointer.SetTargetCage(null);
        }
    }


    public void CaptureAnimal(Animal animal)
    {
        animal.transform.position = this.transform.position;
        Debug.Log("Captured: " + animal.name);
        //Destroy(animal.gameObject);
    }
}
