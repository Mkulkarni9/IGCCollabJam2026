using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] List<Transform> mapPositions = new List<Transform>();
    [SerializeField] Tilemap walkableLayer;
    [SerializeField] int minMapPositionsForEachAnimal;
    [SerializeField] int maxMapPositionsForEachAnimal;


    [SerializeField] int gridLength;
    [SerializeField] int gridHeight;
    [SerializeField] float gridSize;
    [SerializeField] Vector2 gridOrigin;

    [SerializeField] CageManager cageManager;
    [SerializeField] ObstacleManager obstacleManager;

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;

    private void Awake()
    {
        pathFindingGrid = new Grid<PathNode>(gridLength, gridHeight, gridSize, gridOrigin, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        pathfinding = new Pathfinding(pathFindingGrid);

        /*for (int i = 0; i < pathFindingGrid.GetWidth(); i++)
        {
            for (int j = 0; j < pathFindingGrid.GetHeight(); j++)
            {
                PathNode node = pathFindingGrid.GetGridObject(i, j);
                node.SetWalkable(walkableLayer.GetTile(walkableLayer.WorldToCell(pathFindingGrid.GetWorldPosition(i, j))) != null);
            }
        }*/


        ResetWalkableLayers(0);

        cageManager.AssignPathfindingGrid(pathFindingGrid, pathfinding);
        obstacleManager.AssignPathfindingGrid(pathFindingGrid, pathfinding);

    }


    private void OnEnable()
    {
        WaveSpawner.OnEntitySpawned += SetPathfinding;
        LevelManager.OnLevelComplete += ResetWalkableLayers;

    }

    private void OnDisable()
    {
        WaveSpawner.OnEntitySpawned -= SetPathfinding;
        LevelManager.OnLevelComplete -= ResetWalkableLayers;


    }

    void SetPathfinding(GameObject animalSpawned)
    {
        Animal animal = animalSpawned.GetComponent<Animal>();

        if(animal==null) { return; }

        List<Transform> mapPositionsCopy = new List<Transform>();
        List<Transform> mapPositionSelected = new List<Transform>();

        for (int i = 0; i < mapPositions.Count; i++)
        {
            mapPositionsCopy.Add(mapPositions[i]);
        }

        int randomNumberOfMapPositions = Random.Range(minMapPositionsForEachAnimal, maxMapPositionsForEachAnimal);

        for (int i = 0; i < randomNumberOfMapPositions; i++)
        {
            int indexSelected = Random.Range(0, mapPositionsCopy.Count);

            mapPositionSelected.Add(mapPositionsCopy[indexSelected]);
            mapPositionsCopy.RemoveAt(indexSelected);

            //Debug.Log("Position at: "+ i + ": " + mapPositionSelected[i]);
        }



        //Debug.Log("Setting pathfinding for " + animalSpawned.name);
        animalSpawned.GetComponent<Animal>().SetPathfindingVariables(pathFindingGrid, pathfinding, mapPositionSelected);

    }

    void ResetWalkableLayers(int levelIndex)
    {

        Debug.Log("Resetting walkable layers");

        for (int i = 0; i < pathFindingGrid.GetWidth(); i++)
        {
            for (int j = 0; j < pathFindingGrid.GetHeight(); j++)
            {
                PathNode node = pathFindingGrid.GetGridObject(i, j);
                node.SetWalkable(walkableLayer.GetTile(walkableLayer.WorldToCell(pathFindingGrid.GetWorldPosition(i, j))) != null);


            }
        }

    }


}
