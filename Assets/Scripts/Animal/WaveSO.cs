using System.Collections.Generic;
using UnityEngine;

public class WaveSO : ScriptableObject
{
    public List<GameObject> entitiesInWave;
    public int numberOfEntitiesInWave;
    public float intervalBetweenSpawns;
}
