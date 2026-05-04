using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] AnimalSO animalSO;

    public AnimalSO AnimalSO => animalSO;

    //Animal capture and cage related variables
    bool canBeCapturedInCage;
    bool canMove;
    public Vector2 PickupPosition { get; private set; }
    public bool IsInCage { get; private set; }
    Cage targetCage;


    //Pathfinding vaiables
    Rigidbody2D rb;
    List<Vector3> movementPath;
    int currentPathIndex;
    int currentMapPositionIndex;

    Grid<PathNode> pathFindingGridNPC;
    Pathfinding pathfindingNPC;
    List<Transform> mapPositionsNPC = new List<Transform>();
    float movementSpeed;
    bool canSpeedUpAnimal = true;

    Coroutine boostCoroutine;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        canMove = true;

        movementSpeed = animalSO.speed;

        currentMapPositionIndex = 0;
        //PathNode startNode = pathfindingNPC.grid.GetGridObject(mapPositionsNPC[currentMapPositionIndex].position);
        PathNode startNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
        PathNode endNode = pathfindingNPC.grid.GetGridObject(mapPositionsNPC[currentMapPositionIndex + 1].position);


        //this.transform.position = mapPositionsNPC[currentMapPositionIndex].position;

        /*Debug.Log("Start Node: " + startNode.x + ", " + startNode.y);
        Debug.Log("End Node: " + endNode.x + ", " + endNode.y);*/
        SetMovementPath(pathfindingNPC.GetPath(startNode.x, startNode.y, endNode.x, endNode.y));
    }

    private void Update()
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

    }

    public void SpeedUpAnimal()
    {
        if(canSpeedUpAnimal)
        {
            Debug.Log("Speeding up animal: " + name);
            canSpeedUpAnimal = false;
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= animalSO.chanceToBoostSpeed)
            {
                movementSpeed = animalSO.speed * (1 + animalSO.boostSpeedProportion);

                if (boostCoroutine != null)
                {
                    StopCoroutine(boostCoroutine);
                }

                boostCoroutine = StartCoroutine(BoostSpeedRoutine());
            }
        }

    }

    public void ResetSpeedUpStatus()
    {
        canSpeedUpAnimal = true;
    }

    IEnumerator BoostSpeedRoutine()
    {
        yield return new WaitForSeconds(animalSO.boostDuration);
        movementSpeed = animalSO.speed;
    }


    #region Animal capture and cage related methods
    public void SetCaptureStatus(bool status)
    {
        canBeCapturedInCage = status;
    }

    public void PutAnimalInCage()
    {
        if (canBeCapturedInCage )
        {
            if(targetCage.CageSO.animalCageType == animalSO.animalType)
            {
                Debug.Log("Animal put in cage: " + name);
                IsInCage = true;
                targetCage.CaptureAnimal(this);
            }
            else
            {
                this.transform.position = PickupPosition;
                ToggleAnimalMovement(true);
                Debug.Log("Animal cannot be put in cage: " + name + " because cage type is " + targetCage.CageSO.animalCageType + " and animal type is " + animalSO.animalType);
            }
                
        }
        else
        {
            ToggleAnimalMovement(true);
            Debug.Log("Animal can be put in cage: "+ canBeCapturedInCage);
        }
        
    }

    public void SetTargetCage(Cage cage)
    {
        targetCage = cage;
    }

    public void SetPickupPosition(Vector2 position)
    {
        PickupPosition = position;
    }

    public void ToggleAnimalMovement(bool status)
    {
        canMove = status;
    }
    #endregion

    # region Pathfinding
    public void SetPathfindingVariables(Grid<PathNode> pathFindingGrid, Pathfinding pathfinding, List<Transform> mapPositions)
    {
        pathFindingGridNPC = pathFindingGrid;
        pathfindingNPC = pathfinding;
        mapPositionsNPC = mapPositions;
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
