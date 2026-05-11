using System;
using Unity.Cinemachine;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public static event Action OnHitFence;
    [SerializeField] GameObject obstacleCollisionVFX;

    Grid<PathNode> pathFindingGrid;
    CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointerGrabber pointerGrabber = collision.GetComponent<PointerGrabber>();

        if(pointerGrabber !=null)
        {
            if(pointerGrabber.IsGrabbingAnimal)
            {
                //Debug.Log("Dropping grabbed animal");
                Instantiate(obstacleCollisionVFX, pointerGrabber.transform.position,Quaternion.identity);
                cinemachineImpulseSource.GenerateImpulse(0.1f);

                OnHitFence?.Invoke();
                pointerGrabber.ReleaseAnimal();
            }
        }
    }


    public void UpdateWalkableTile(Grid<PathNode> pathFindingGrid)
    {
        this.pathFindingGrid = pathFindingGrid;

        PathNode currentNode = pathFindingGrid.GetGridObject(this.transform.position);
        //Debug.Log("Obstacle node for obstacle: " + currentNode);
        currentNode.SetWalkable(false);
    }
}
