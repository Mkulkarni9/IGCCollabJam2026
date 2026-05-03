using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CageSO", menuName = "Scriptable Objects/CageSO")]
public class CageSO : ScriptableObject
{
    public AnimalSO.AnimalType animalCageType;
}
