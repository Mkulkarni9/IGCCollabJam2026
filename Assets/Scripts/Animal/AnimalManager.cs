using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] List<Transform> mapPositions = new List<Transform>();
    [SerializeField] Tilemap walkableLayer;


    [SerializeField] int gridLength;
    [SerializeField] int gridHeight;
    [SerializeField] float gridSize;
    [SerializeField] Vector2 gridOrigin;

    Grid<PathNode> pathFindingGrid;
    Pathfinding pathfinding;

    private void Awake()
    {
        pathFindingGrid = new Grid<PathNode>(gridLength, gridHeight, gridSize, gridOrigin, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        pathfinding = new Pathfinding(pathFindingGrid);

        for (int i = 0; i < pathFindingGrid.GetWidth(); i++)
        {
            for (int j = 0; j < pathFindingGrid.GetHeight(); j++)
            {
                PathNode node = pathFindingGrid.GetGridObject(i, j);
                node.SetWalkable(walkableLayer.GetTile(walkableLayer.WorldToCell(pathFindingGrid.GetWorldPosition(i, j))) != null);
            }
        }



    }


    private void OnEnable()
    {
        WaveSpawner.OnEntitySpawned += SetPathfinding;

    }

    private void OnDisable()
    {
        WaveSpawner.OnEntitySpawned -= SetPathfinding;

    }

    void SetPathfinding(GameObject animalSpawned)
    {
        Animal animal = animalSpawned.GetComponent<Animal>();
        if (animal!=null)
        {
            Debug.Log("Setting pathfinding for " + animalSpawned.name);
            animalSpawned.GetComponent<Animal>().SetPathfindingVariables(pathFindingGrid, pathfinding, mapPositions);
        }
        
    }
}
