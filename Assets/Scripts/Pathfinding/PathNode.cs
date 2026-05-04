using UnityEngine;
using static UnityEngine.UI.Image;

public class PathNode
{
    public Grid<PathNode> g;
    public int x;
    public int y;

    public int fCost;
    public int gCost;
    public int hCost;

    public PathNode cameFromNode;
    public bool isWalkable;

    public PathNode(Grid<PathNode> g, int x, int y)
    {

        this.g = g;
        this.x = x;
        this.y = y;
        isWalkable = true;




    }

    public override string ToString()
    {
        return x + " , " + y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

   
    public void SetWalkable(bool walkable)
    {
        isWalkable = walkable;
    }
    


}
