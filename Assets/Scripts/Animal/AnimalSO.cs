using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSO", menuName = "Scriptable Objects/AnimalSO")]
public class AnimalSO : ScriptableObject
{
    [Serializable]
    public enum AnimalType
    {
        Cat,
        Dog,
        Bird
    }

    public AnimalType animalType;
}
