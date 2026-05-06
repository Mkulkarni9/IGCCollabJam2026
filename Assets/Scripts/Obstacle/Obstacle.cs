using UnityEngine;

public class Obstacle : MonoBehaviour
{

    Grid<PathNode> pathFindingGrid;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointerGrabber pointerGrabber = collision.GetComponent<PointerGrabber>();

        if(pointerGrabber !=null)
        {
            if(pointerGrabber.IsGrabbingAnimal)
            {
                Debug.Log("Dropping grabbed animal");
                pointerGrabber.ReleaseAnimal();
            }
        }
    }


    public void UpdateWalkableTile(Grid<PathNode> pathFindingGrid)
    {
        this.pathFindingGrid = pathFindingGrid;

        PathNode currentNode = pathFindingGrid.GetGridObject(this.transform.position);
        Debug.Log("Obstacle node for obstacle: " + currentNode);
        currentNode.SetWalkable(false);
    }
}
