using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSO", menuName = "Scriptable Objects/AnimalSO")]
public class AnimalSO : ScriptableObject
{
    [Serializable]
    public enum AnimalType
    {
        Sheep_R,
        Sheep_G,
        Sheep_B,
        Sheep_RR,
        Sheep_GG,
        Sheep_BB,
        Sheep_RG,
        Sheep_GB,
        Sheep_RB,
        Sheep_RRR,
        Sheep_GGG,
        Sheep_BBB,
        Sheep_RRG,
        Sheep_RRB,
        Sheep_GGR,
        Sheep_GGB,
        Sheep_BBR,
        Sheep_BBG,
        Sheep_RGB,

    }

    public AnimalType animalType;
    public float speed;
    public float boostSpeedProportion;
    public float boostDuration;
    public float chanceToBoostSpeed;
}
