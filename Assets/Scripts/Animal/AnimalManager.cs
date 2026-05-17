using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalManager : MonoBehaviour
{
    public static event Action OnZeroSheepOnMap;


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
    [SerializeField] WolfSpawner wolfManager;

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;

    int maxAnimalsInLevel;
    int currentAnimalsInLevel;

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
        wolfManager.AssignPathfindingGrid(pathFindingGrid, pathfinding);

    }


    private void OnEnable()
    {
        WaveSpawner.OnEntitySpawned += SetPathfinding;
        LevelManager.OnLevelComplete += ResetWalkableLayers;
        LevelManager.OnNewLevelStart += SetNumberOfAnimalsInLevel;
        Animal.OnEatenByWolf += UpdateCurrentAnimalsInLevel;
        Cage.OnAnimalCapturedInCorrectCage += UpdateCurrentAnimalsInLevel;

    }

    private void OnDisable()
    {
        WaveSpawner.OnEntitySpawned -= SetPathfinding;
        LevelManager.OnLevelComplete -= ResetWalkableLayers;
        LevelManager.OnNewLevelStart -= SetNumberOfAnimalsInLevel;
        Animal.OnEatenByWolf -= UpdateCurrentAnimalsInLevel;
        Cage.OnAnimalCapturedInCorrectCage -= UpdateCurrentAnimalsInLevel;


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

        int randomNumberOfMapPositions = UnityEngine.Random.Range(minMapPositionsForEachAnimal, maxMapPositionsForEachAnimal);

        for (int i = 0; i < randomNumberOfMapPositions; i++)
        {
            int indexSelected = UnityEngine.Random.Range(0, mapPositionsCopy.Count);

            mapPositionSelected.Add(mapPositionsCopy[indexSelected]);
            mapPositionsCopy.RemoveAt(indexSelected);

            //Debug.Log("Position at: "+ i + ": " + mapPositionSelected[i]);
        }



        //Debug.Log("Setting pathfinding for " + animalSpawned.name);
        animalSpawned.GetComponent<Animal>().SetPathfindingVariables(pathFindingGrid, pathfinding, mapPositionSelected);

    }

    void ResetWalkableLayers(int levelIndex)
    {

        //Debug.Log("Resetting walkable layers");

        for (int i = 0; i < pathFindingGrid.GetWidth(); i++)
        {
            for (int j = 0; j < pathFindingGrid.GetHeight(); j++)
            {
                PathNode node = pathFindingGrid.GetGridObject(i, j);
                node.SetWalkable(walkableLayer.GetTile(walkableLayer.WorldToCell(pathFindingGrid.GetWorldPosition(i, j))) != null);


            }
        }

    }

    void SetNumberOfAnimalsInLevel(int levelIndex)
    {
        maxAnimalsInLevel = GetComponent<WaveSpawner>().entityWaves[levelIndex].animalTypes.Count;
        //Debug.Log("Total sheep in current wave: "+ maxAnimalsInLevel);
        currentAnimalsInLevel = maxAnimalsInLevel;
    }

    void UpdateCurrentAnimalsInLevel(Animal animal, Cage cage)
    {
        UpdateCurrentAnimalsInLevel();
    }



    void UpdateCurrentAnimalsInLevel()
    {
        currentAnimalsInLevel--;

        //Debug.Log("Sheep count: "+ currentAnimalsInLevel);

        if(CheckIfAnimalCountIsZero())
        {
            OnZeroSheepOnMap?.Invoke();
        }
    }

    bool CheckIfAnimalCountIsZero()
    {
        if(currentAnimalsInLevel == 0)
        {
            return true;
        }

        return false;
    }

}
