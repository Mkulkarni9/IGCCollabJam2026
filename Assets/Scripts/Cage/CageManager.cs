using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class CageManager : MonoBehaviour
{


    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;

    private void OnEnable()
    {
        WaveSpawner.OnEntitySpawned += AssignPathfindingToCage;
    }

    private void OnDisable()
    {
        WaveSpawner.OnEntitySpawned -= AssignPathfindingToCage;


    }


    public void AssignPathfindingGrid(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding)
    {
        this.pathFindingGrid = pathFindingGrid;
        this.pathfinding = pathfinding;
    }

    void AssignPathfindingToCage(GameObject cageSpawned)
    {
        Cage cage = cageSpawned.GetComponent<Cage>();

        if (cage == null) { return; }

        //Debug.Log("Setting pathfinding for " + animalSpawned.name);
        cageSpawned.GetComponent<Cage>().SetPathfindingVariablesToCage(pathFindingGrid, pathfinding);

    }


    



}
