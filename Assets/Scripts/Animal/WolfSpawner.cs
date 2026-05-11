using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    public static event Action OnWolfSpawned;

    [SerializeField] List<WolfWaveSO> wolfWaves = new List<WolfWaveSO>();
    [SerializeField] List<Transform> wolfSpawnPositions = new List<Transform>();

    [SerializeField] float delayBeforeWolfSpawn;

    Coroutine wolfRoutine;

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;

   
    private void OnEnable()
    {
        LevelManager.OnNewLevelStart += SpawnWolf;

    }

    private void OnDisable()
    {
        LevelManager.OnNewLevelStart -= SpawnWolf;


    }



    void SpawnWolf(int levelIndex)
    {
        if (wolfRoutine != null)
        {
            StopCoroutine(wolfRoutine);
        }

        wolfRoutine = StartCoroutine(SpawnWolfROutine(levelIndex));

    }



    IEnumerator SpawnWolfROutine(int levelIndex)
    {
        yield return new WaitForSeconds(delayBeforeWolfSpawn);

        for (int i = 0; i < wolfWaves[levelIndex].numberOfWolves; i++)
        {

            int randomWolfSpawnPointIndex = UnityEngine.Random.Range(0, wolfSpawnPositions.Count);

            GameObject wolfSpawned = Instantiate(wolfWaves[levelIndex].wolfPrefab, wolfSpawnPositions[randomWolfSpawnPointIndex].position, Quaternion.identity);
            wolfSpawned.GetComponent<Wolf>().SetPathfindingVariables(pathFindingGrid, pathfinding);
            wolfSpawned.transform.SetParent(this.transform);

            OnWolfSpawned?.Invoke();

            yield return new WaitForSeconds(wolfWaves[levelIndex].intervalBetweenWolfSpawns);
            
        }


    }


    public void AssignPathfindingGrid(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding)
    {
        this.pathFindingGrid = pathFindingGrid;
        this.pathfinding = pathfinding;
    }

    
}
