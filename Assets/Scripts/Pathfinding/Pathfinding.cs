using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pathfinding
{
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;


    List<PathNode> openList;
    List<PathNode> closedList;

    bool isDebugMode = true;

    public Grid<PathNode> grid;

    public Pathfinding(Grid<PathNode> pathFindingGrid)
    {
        
        this.grid = pathFindingGrid;
    }

    public List<PathNode> GetPath(int startX, int startY, int endX, int endY)
    {

        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        /*Debug.Log("Start Node: " + startNode);
        Debug.Log("End Node: " + endNode);*/

        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                PathNode node = grid.GetGridObject(i, j);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetNodeWithMinFCost(openList);

            //Debug.Log("Current Node: " + currentNode);

            if (currentNode == endNode)
            {
                List<PathNode> path = CalculatePath(endNode);

                TracePath(path);

                return path;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbourlist = GetNeighbours(currentNode);
            foreach (PathNode neighbourNode in neighbourlist)
            {
                if (closedList.Contains(neighbourNode)) continue;
                if(!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativegCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativegCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativegCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }

        }

        Debug.Log("No path found!");

        return null;        
    }

    void TracePath(List<PathNode> path)
    {
        if (isDebugMode)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 offsetStart = new Vector3(path[i].g.GetCellSize(), path[i].g.GetCellSize()) * 0.5f;
                Vector3 offsetEnd = new Vector3(path[i].g.GetCellSize(), path[i].g.GetCellSize()) * 0.5f;
                Debug.DrawLine(path[i].g.GetWorldPosition(path[i].x, path[i].y) + offsetStart, path[i+1].g.GetWorldPosition(path[i+1].x, path[i+1].y) + offsetEnd, Color.red, 100f);
                
            }
        }
    }

    List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        return path;
    }


    int CalculateDistanceCost(PathNode startNode, PathNode endNode)
    {

        int horizontalDistance = Mathf.Abs(endNode.x - startNode.x);
        int verticalDistance = Mathf.Abs(endNode.y - startNode.y);
        int remaining = Mathf.Abs(horizontalDistance - verticalDistance);

        return MOVE_STRAIGHT_COST * remaining + MOVE_DIAGONAL_COST *Mathf.Min (horizontalDistance, verticalDistance);
    }

    List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (grid.CheckIfValueWithinBounds(currentNode.x - 1, currentNode.y)) neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));
        if (grid.CheckIfValueWithinBounds(currentNode.x - 1, currentNode.y +1)) neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));
        if (grid.CheckIfValueWithinBounds(currentNode.x - 1, currentNode.y -1)) neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
        if (grid.CheckIfValueWithinBounds(currentNode.x, currentNode.y - 1)) neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1));
        if (grid.CheckIfValueWithinBounds(currentNode.x, currentNode.y + 1)) neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1));
        if (grid.CheckIfValueWithinBounds(currentNode.x + 1, currentNode.y)) neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));
        if (grid.CheckIfValueWithinBounds(currentNode.x + 1, currentNode.y + 1)) neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
        if (grid.CheckIfValueWithinBounds(currentNode.x + 1, currentNode.y - 1)) neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));

        return neighbourList;
    }

    PathNode GetNodeWithMinFCost(List<PathNode> nodesList)
    {
        int minFCostIndex=0;
        for (int i = 0; i < nodesList.Count; i++)
        {
            if (nodesList[i].fCost < nodesList[minFCostIndex].fCost) minFCostIndex = i;

        }

        return nodesList[minFCostIndex];

    }


}
