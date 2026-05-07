using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    

    //Pathfinding vaiables
    protected List<Vector3> movementPath;
    protected int currentPathIndex;
    protected int currentMapPositionIndex;

    protected Grid<PathNode> pathFindingGridNPC;
    protected Pathfinding pathfindingNPC;
    List<Transform> mapPositionsNPC = new List<Transform>();
    protected float movementSpeed;

    protected bool canMove;

    // sprite/rotation flip helpers
    protected Vector3 lastPosition;


    protected virtual void Start()
    {
        

        currentMapPositionIndex = 0;
        //PathNode startNode = pathfindingNPC.grid.GetGridObject(mapPositionsNPC[currentMapPositionIndex].position);
        PathNode startNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
        PathNode endNode = pathfindingNPC.grid.GetGridObject(mapPositionsNPC[currentMapPositionIndex + 1].position);


        //this.transform.position = mapPositionsNPC[currentMapPositionIndex].position;

        /*Debug.Log("Start Node: " + startNode.x + ", " + startNode.y);
        Debug.Log("End Node: " + endNode.x + ", " + endNode.y);*/
        SetMovementPath(pathfindingNPC.GetPath(startNode.x, startNode.y, endNode.x, endNode.y));
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            //transform.Translate(Vector2.up * animalSO.speed * Time.deltaTime);

            Vector3 offset = new Vector3(pathFindingGridNPC.GetCellSize(), pathFindingGridNPC.GetCellSize()) * 0.5f;
            Vector3 targetWorldPosition = pathFindingGridNPC.GetWorldPosition((int)movementPath[currentPathIndex].x, (int)movementPath[currentPathIndex].y) + offset;


            transform.position = Vector2.MoveTowards(this.transform.position, targetWorldPosition, movementSpeed * Time.deltaTime);

            if (this.transform.position == targetWorldPosition && currentPathIndex < movementPath.Count - 1)
            {
                currentPathIndex++;
            }
            else if (this.transform.position == targetWorldPosition && currentPathIndex == movementPath.Count - 1)
            {
                currentMapPositionIndex++;

                if (currentMapPositionIndex == mapPositionsNPC.Count)
                {
                    currentMapPositionIndex = 0;
                }

                PathNode newStartNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
                PathNode newEndNode = pathfindingNPC.grid.GetGridObject(mapPositionsNPC[currentMapPositionIndex + 1 == mapPositionsNPC.Count ? 0 : currentMapPositionIndex + 1].position);

                SetMovementPath(pathfindingNPC.GetPath(newStartNode.x, newStartNode.y, newEndNode.x, newEndNode.y));
            }
        }

        FlipGameObjectBasedOnMovement();


    }

    protected void FlipGameObjectBasedOnMovement()
    {
        float dx = transform.position.x - lastPosition.x;

        Vector3 euler = transform.localEulerAngles;
        euler.y = dx < 0f ? 180f : 0f;
        transform.localEulerAngles = euler;

        lastPosition = transform.position;
    }


    # region Pathfinding
    public void SetPathfindingVariables(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding, List<Transform> mapPositions)
    {
        pathFindingGridNPC = pathFindingGrid;
        pathfindingNPC = pathfinding;
        mapPositionsNPC = mapPositions;
    }

    public void SetPathfindingVariables(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding)
    {
        pathFindingGridNPC = pathFindingGrid;
        pathfindingNPC = pathfinding;
    }


    public void SetMovementPath(List<PathNode> movementPathNodes)
    {

        currentPathIndex = 0;
        movementPath = new List<Vector3>();


        for (int i = 0; i < movementPathNodes.Count; i++)
        {
            movementPath.Add(new Vector3(movementPathNodes[i].x, movementPathNodes[i].y));

            //Debug.Log("Path Node: " + movementPath[i]);
        }

        //Debug.Log("Count of path nodes: " + movementPath.Count);
    }

    #endregion
}
