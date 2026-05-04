using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [Serializable]
    public enum SpawnMode
    {
        Sequential,
        Random
    }
    [SerializeField] List<WaveSO> entityWaves = new List<WaveSO>();
    [SerializeField] List<Transform> entitySpawnPoints = new List<Transform>();
    [SerializeField] SpawnMode spawnMode;

    WaveSO currentWave;
    int currentWaveIndex;
    int currentSpawnPositionIndex;
    int entitiesSpawnedInCurrentWave;

    Coroutine waveSpawnRoutine;

    private void Start()
    {
        currentWaveIndex = 0;
        currentWave = entityWaves[currentWaveIndex];
        SpawnEntities();
    }



    void SpawnEntities()
    {
        if(waveSpawnRoutine !=null)
        {
            StopCoroutine(waveSpawnRoutine);
        }

        waveSpawnRoutine = StartCoroutine(SpawnEntitiesRoutine());
    }

    IEnumerator SpawnEntitiesRoutine()
    {
        entitiesSpawnedInCurrentWave = 0;
        currentSpawnPositionIndex = 0;



        while (entitiesSpawnedInCurrentWave < currentWave.numberOfEntitiesInWave)
        {
            entitiesSpawnedInCurrentWave++;
            int randomEntityIndex = UnityEngine.Random.Range(0, currentWave.entitiesInWave.Count);
            int spawnPointIndex = GetSpawnPointIndex();

            GameObject entitySpawned = Instantiate(currentWave.entitiesInWave[randomEntityIndex], entitySpawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
            entitySpawned.transform.SetParent(this.transform);

            yield return new WaitForSeconds(currentWave.intervalBetweenSpawns);
        }

        if(currentWaveIndex < entityWaves.Count - 1)
        {
            currentWaveIndex++;
            currentWave = entityWaves[currentWaveIndex];
            SpawnEntities();
        }

    }


    public int GetSpawnPointIndex()
    {
        switch (spawnMode)
        {
            case SpawnMode.Sequential:

                int spawnPositionIndex = currentSpawnPositionIndex;
                currentSpawnPositionIndex++;

                if(spawnPositionIndex >= entitySpawnPoints.Count-1)
                {
                    currentSpawnPositionIndex = 0;
                }

                return spawnPositionIndex;

            case SpawnMode.Random:
                return UnityEngine.Random.Range(0, entitySpawnPoints.Count);

            default:
                Debug.LogError("Invalid spawn mode selected.");
                return 0;
        }
    }
}
