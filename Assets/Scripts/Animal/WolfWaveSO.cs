using UnityEngine;

[CreateAssetMenu(fileName = "WolfWaveSO", menuName = "Scriptable Objects/WolfWaveSO")]
public class WolfWaveSO : ScriptableObject
{
    public GameObject wolfPrefab;
    public int numberOfWolves;
    public float intervalBetweenWolfSpawns;
}
