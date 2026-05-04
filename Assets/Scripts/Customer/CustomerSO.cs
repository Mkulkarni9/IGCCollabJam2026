using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSO", menuName = "Scriptable Objects/CustomerSO")]
public class CustomerSO : ScriptableObject
{
    public List<AnimalSO.AnimalType> desiredAnimalType;
    public float movementSpeed;
}
