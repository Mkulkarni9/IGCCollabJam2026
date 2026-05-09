using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [Serializable]
    public enum SpawnMode
    {
        Sequential,
        Random
    }

    [Serializable]
    public enum TypeOfEntity
    {
        Animal,
        Cage
    }


    public static event Action<GameObject> OnEntitySpawned; 

    public List<WaveSO> entityWaves = new List<WaveSO>();
    [SerializeField] List<Transform> entitySpawnPoints = new List<Transform>();
    [SerializeField] TypeOfEntity typeOfEntity;
    [SerializeField] SpawnMode spawnMode;

    WaveSO currentWave;
    int currentWaveIndex;
    int currentSpawnPositionIndex;
    int entitiesSpawnedInCurrentWave;
    float intervalBetweenSpawns;
    List<Transform> shuffledEntitySpawnPoints = new List<Transform>();

    Coroutine waveSpawnRoutine;

    private void Start()
    {
        currentWaveIndex = 0;
        currentWave = entityWaves[currentWaveIndex];
        shuffledEntitySpawnPoints = entitySpawnPoints.OrderBy(x => UnityEngine.Random.value).ToList();


        //SpawnEntities(0);
    }

    private void OnEnable()
    {
        LevelManager.OnNewLevelStart += SpawnEntities;
    }

    private void OnDisable()
    {
        LevelManager.OnNewLevelStart -= SpawnEntities;

    }



    void SpawnEntities(int levelIndex)
    {
        if(waveSpawnRoutine !=null)
        {
            StopCoroutine(waveSpawnRoutine);
        }

        switch (typeOfEntity)
        {
            case TypeOfEntity.Animal:
                intervalBetweenSpawns = currentWave.intervalBetweenAnimalSpawns;
                break;
            case TypeOfEntity.Cage:
                intervalBetweenSpawns = currentWave.intervalBetweenCageSpawns;
                break;
            default:
                break;
        }

        Debug.Log("Starting wave: " + currentWaveIndex);


        waveSpawnRoutine = StartCoroutine(SpawnEntitiesRoutine());
    }

    IEnumerator SpawnEntitiesRoutine()
    {
        entitiesSpawnedInCurrentWave = 0;
        currentSpawnPositionIndex = 0;



        while (entitiesSpawnedInCurrentWave < currentWave.animalTypes.Count)
        {
            
            //int randomEntityIndex = UnityEngine.Random.Range(0, currentWave.entitiesInWave.Count);
            int spawnPointIndex = GetSpawnPointIndex();

            GameObject entityToBeSpawned = GetMatchingGameObject(currentWave.animalTypes[entitiesSpawnedInCurrentWave]);

            GameObject entitySpawned = Instantiate(entityToBeSpawned, shuffledEntitySpawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
            entitySpawned.transform.SetParent(this.transform);

            entitiesSpawnedInCurrentWave++;
            OnEntitySpawned?.Invoke(entitySpawned);

            yield return new WaitForSeconds(intervalBetweenSpawns);
        }

        if(currentWaveIndex < entityWaves.Count - 1)
        {
            currentWaveIndex++;
            currentWave = entityWaves[currentWaveIndex];


        }

    }

    public WaveSO GetCurrentWave()
    {
        return currentWave;
    }
    public int GetSpawnPointIndex()
    {
        switch (spawnMode)
        {
            case SpawnMode.Sequential:

                int spawnPositionIndex = currentSpawnPositionIndex;
                currentSpawnPositionIndex++;

                if(spawnPositionIndex >= shuffledEntitySpawnPoints.Count-1)
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

    GameObject GetMatchingGameObject(AnimalSO.AnimalType animalType)
    {
        
        switch(typeOfEntity)
        {
            case TypeOfEntity.Animal:
                foreach (GameObject entity in currentWave.animalsInWave)
                {
                    if (entity.GetComponent<Animal>().AnimalSO.animalType == animalType)
                    {
                        return entity;
                    }
                }
                break;
            case TypeOfEntity.Cage:
                foreach (GameObject entity in currentWave.cagesInWave)
                {
                    if (entity.GetComponent<Cage>().CageSO.animalCageType == animalType)
                    {
                        return entity;
                    }
                }
                break;
            default:
                Debug.Log("No matching entity");
                break;
        }

        

        Debug.Log("No matching entity found");

        return null;
    }
    
}
