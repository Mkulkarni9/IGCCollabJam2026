using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    public static event Action<GameObject> OnObstacleReveal;

    [SerializeField] List<GameObject> obstacleContainers = new List<GameObject>();

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;


    private void OnEnable()
    {
        LevelManager.OnNewLevelStart += RevealObstaclesForLevel;
        OnObstacleReveal += AssignPathfindingToObstacle;
    }

    private void OnDisable()
    {
        LevelManager.OnNewLevelStart -= RevealObstaclesForLevel;
        OnObstacleReveal -= AssignPathfindingToObstacle;

    }

    public void AssignPathfindingGrid(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding)
    {
        this.pathFindingGrid = pathFindingGrid;
        this.pathfinding = pathfinding;
    }



    void RevealObstaclesForLevel(int levelIndex)
    {
        for (int i = 0; i < obstacleContainers.Count; i++)
        {
            obstacleContainers[i].gameObject.SetActive(false);
        }

        obstacleContainers[levelIndex].SetActive(true);

        OnObstacleReveal?.Invoke(obstacleContainers[levelIndex]);

    }

    void AssignPathfindingToObstacle(GameObject obstacleContainerSpawned)
    {

        Obstacle[] obstacles = obstacleContainerSpawned.GetComponentsInChildren<Obstacle>();


        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.UpdateWalkableTile(pathFindingGrid);
        }

    }
}
