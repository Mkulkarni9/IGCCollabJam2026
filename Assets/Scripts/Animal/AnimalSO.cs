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
        Sheep_G,
        Sheep_O,
        Sheep_GG
    }

    public AnimalType animalType;
    public float speed;
    public float boostSpeedProportion;
    public float boostDuration;
    public float chanceToBoostSpeed;
}
