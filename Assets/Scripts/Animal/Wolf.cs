using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class Wolf : NPC
{
    public static event Action OnWolfStunned;

    [SerializeField] float wolfSpeed;
    [SerializeField] float wolfStunDuration;

    [SerializeField] float timeIntervalToSearchClosestSheep;

    [SerializeField] ParticleSystem wolfStunVFX;

    public bool CanBeStunned { get; private set; } = true;

    Animal closestSheep;

    AnimalManager animalManager;
    CinemachineImpulseSource cinemachineImpulseSource;

    Coroutine wolfStunRoutine;
    private void Awake()
    {
        canMove = true;
        movementSpeed = wolfSpeed;

        animalManager = FindAnyObjectByType<AnimalManager>();
        cinemachineImpulseSource =GetComponent<CinemachineImpulseSource>();

        lastPosition = transform.position;
    }

    private void OnEnable()
    {
        LevelManager.OnLevelComplete += DestroyWolf;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelComplete -= DestroyWolf;


    }

    protected override void Start()
    {
        StartCoroutine(FindClosestSheepRoutine());
        
        PathNode startNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
        PathNode endNode = pathfindingNPC.grid.GetGridObject(closestSheep.transform.position);


        /*Debug.Log("Start Node: " + startNode.x + ", " + startNode.y);
        Debug.Log("End Node: " + endNode.x + ", " + endNode.y);*/

        SetMovementPath(pathfindingNPC.GetPath(startNode.x, startNode.y, endNode.x, endNode.y));


    }

    protected override void Update()
    {
        if (closestSheep == null) return;
        if (movementPath == null || movementPath.Count==0) return;

        if (canMove)
        {
            

            Vector3 offset = new Vector3(pathFindingGridNPC.GetCellSize(), pathFindingGridNPC.GetCellSize()) * 0.5f;
            Vector3 targetWorldPosition = pathFindingGridNPC.GetWorldPosition((int)movementPath[currentPathIndex].x, (int)movementPath[currentPathIndex].y) + offset;

            /*Debug.Log("current position: "+ this.transform.position);
            Debug.Log("target position: "+ targetWorldPosition);*/

            transform.position = Vector2.MoveTowards(this.transform.position, targetWorldPosition, movementSpeed * Time.deltaTime);

            if (this.transform.position == targetWorldPosition && currentPathIndex < movementPath.Count - 1)
            {
                currentPathIndex++;
            }
            else if (this.transform.position == targetWorldPosition && currentPathIndex == movementPath.Count - 1)
            {

                PathNode newStartNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
                PathNode newEndNode = pathfindingNPC.grid.GetGridObject(closestSheep.transform.position);

                List<PathNode> newMovementPath = pathfindingNPC.GetPath(newStartNode.x, newStartNode.y, newEndNode.x, newEndNode.y);

                if (newMovementPath != null && newMovementPath.Count > 0)
                {
                    SetMovementPath(newMovementPath);
                }
                
            }

            FlipGameObjectBasedOnMovement();

        }



    }



    IEnumerator FindClosestSheepRoutine()
    {

        while(true)
        {
            Animal[] animals = animalManager.GetComponentsInChildren<Animal>();

            //Debug.Log("Animals: "+ animals.Length);

            float minDistance = float.MaxValue;
            int minDistanceIndex = 0;
            
            for (int i = 0; i < animals.Length; i++)
            {
                if (!animals[i].IsGrabbed)
                {
                    float squaredDistance = 0f;
                    squaredDistance = Mathf.Pow((animals[i].gameObject.transform.position.x - this.transform.position.x), 2) + Mathf.Pow((animals[i].gameObject.transform.position.y - this.transform.position.y), 2);
                    //Debug.Log("Squared distance: " + squaredDistance);

                    if (squaredDistance < minDistance)
                    {
                        minDistance = squaredDistance;
                        minDistanceIndex = i;
                    }
                }
                
            }

            if(animals.Length > 0)
            {
                closestSheep = animals[minDistanceIndex];

                /*Debug.Log("Min distance: " + minDistance + " || min distance Index: " + minDistanceIndex);
                Debug.Log("Position of closest sheep: " + closestSheep.transform.position);*/

                if(closestSheep!=null)
                {
                    PathNode newStartNode = pathfindingNPC.grid.GetGridObject(this.transform.position);
                    PathNode newEndNode = pathfindingNPC.grid.GetGridObject(closestSheep.transform.position);


                    List<PathNode> newMovementPath = pathfindingNPC.GetPath(newStartNode.x, newStartNode.y, newEndNode.x, newEndNode.y);

                    if(newMovementPath!=null && newMovementPath.Count>0)
                    {
                        SetMovementPath(newMovementPath);
                    }
                        

                    
                }
                
            }
            else
            {
                closestSheep = null;

                //Debug.Log("No closest sheep found");
            }

            
            


            yield return new WaitForSeconds(timeIntervalToSearchClosestSheep);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animal animal = collision.GetComponent<Animal>();


        if (animal != null)
        {
            if(!animal.IsGrabbed)
            {
                Debug.Log("Eating sheep");
                cinemachineImpulseSource.GenerateImpulse(0.5f);
                animal.GetEatenByWolf();
            }
            
        }
    }

    public void StunWolf()
    {
        if (wolfStunRoutine != null)
        {
            StopCoroutine(wolfStunRoutine);
        }

        wolfStunRoutine = StartCoroutine(StunWolfRoutine());
    }

    IEnumerator StunWolfRoutine()
    {
        CanBeStunned = false;
        canMove = false;
        //Debug.Log("Wolf stunned: canMove is "+canMove);
        OnWolfStunned?.Invoke();

        wolfStunVFX.Play();
        yield return new WaitForSeconds(wolfStunDuration);
        
        canMove = true;
        CanBeStunned = true;
    }


    void DestroyWolf(int levelIndex)
    {
        Destroy(gameObject);
    }


}
