using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;

    public Transform seeker, target;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {

    }

    public List<Grid.GridCell> FindPath(Vector2 startPos, Vector2 endPos)
    {

        //Check if Cells are on grid
        if (grid.GridCellsRangeCheck(startPos) == false || grid.GridCellsRangeCheck(endPos) == false) return null;


        //Debug.Log("Finding Path");
        Grid.GridCell startCell = grid.GetCellByPos(startPos);
        Grid.GridCell endCell = grid.GetCellByPos(endPos);

        List<Grid.GridCell> openSet = new List<Grid.GridCell>();
        HashSet<Grid.GridCell> closedSet = new HashSet<Grid.GridCell>();
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            Grid.GridCell currentCell = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentCell.fCost || openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost)
                {
                    currentCell = openSet[i];
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if(currentCell == endCell)
            {
                List<Grid.GridCell> path = RetracePath(startCell, endCell);
                return path;
            }

            foreach(Grid.GridCell neigbour in grid.GetNeigbours(currentCell))
            {
                if (neigbour.cellstate == Grid.CellState.wall || closedSet.Contains(neigbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentCell.gCost + GetDistance(currentCell, neigbour);
                if(newCostToNeighbour < neigbour.gCost || !openSet.Contains(neigbour))
                {
                    neigbour.gCost = newCostToNeighbour;
                    neigbour.hCost = GetDistance(neigbour, endCell);
                    neigbour.parent = currentCell;

                    if(openSet.Contains(neigbour) == false)
                    {
                        openSet.Add(neigbour);
                    }
                }
            }

        }

        return null;
    }

    private List<Grid.GridCell> RetracePath(Grid.GridCell startNode, Grid.GridCell endNode)
    {
        List<Grid.GridCell> path = new List<Grid.GridCell>();
        Grid.GridCell curCell = endNode;

        while(curCell != startNode)
        {
            path.Add(curCell);
            curCell = curCell.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Grid.GridCell cellA, Grid.GridCell cellB)
    {
        int distantX = Mathf.Abs(cellA.gridPos.x - cellB.gridPos.x);
        int distantY = Mathf.Abs(cellA.gridPos.y - cellB.gridPos.y);

        if (distantX > distantY)
            return 14 * distantY + 10 * (distantX - distantY);
        else return 14 * distantX + 10 * (distantY - distantX);
    }



}
