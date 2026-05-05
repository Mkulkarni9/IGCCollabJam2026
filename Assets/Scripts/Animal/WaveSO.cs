using System.Collections.Generic;
using UnityEngine;

public class WaveSO : ScriptableObject
{
    public List<GameObject> animalsInWave;
    public List<GameObject> cagesInWave;
    public List<AnimalSO.AnimalType> animalTypes;
    public float intervalBetweenAnimalSpawns;
    public float intervalBetweenCageSpawns;
    public float totalWaveDuration;
}
