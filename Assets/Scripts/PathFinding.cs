﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    GameManager gameManager;
    public Transform seeker, target;

    void Awake()
    {
        grid = GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {

    }

    public List<GridCell> FindPath(Vector2 startPos, Vector2 endPos)
    {
        // gameManager.ResetPositions();
        //HashSet<Vector2Int> positions = gameManager.GetPositions();
        //positions.Remove(grid.GetCellPos(endPos));


        //Check if Cells are on grid
        if (grid.GridCellsRangeCheck(startPos) == false || grid.GridCellsRangeCheck(endPos) == false || startPos.Equals(endPos) == true) return null;


        //Debug.Log("Finding Path");
        GridCell startCell = grid.GetCellByPos(startPos);
        GridCell endCell = grid.GetCellByPos(endPos);

        List<GridCell> openSet = new List<GridCell>();
        HashSet<GridCell> closedSet = new HashSet<GridCell>();
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            GridCell currentCell = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentCell.fCost || openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost)
                {
                    currentCell = openSet[i];
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if (currentCell == endCell)
            {
                List<GridCell> path = RetracePath(startCell, endCell);
                return path;
            }

            foreach (GridCell neigbour in grid.GetNeigbours(currentCell))
            {

                if (closedSet.Contains(neigbour) || (neigbour.HasCellObject() == true && neigbour != endCell))
                {
                    continue;
                }
                else
                {
                    //Put something here to block pathfinding
                    bool blockPathfinding = false;


                    blockPathfinding = neigbour.CheckCellObject();


                    switch (neigbour.cellstate)
                    {
                        case Grid.CellState.wall:
                            blockPathfinding = true;
                            break;


                        default:
                            break;
                    }
                    if (blockPathfinding) continue;
                }

                int newCostToNeighbour = currentCell.gCost + GetDistance(currentCell, neigbour);
                if (newCostToNeighbour < neigbour.gCost || !openSet.Contains(neigbour))
                {
                    neigbour.gCost = newCostToNeighbour;
                    neigbour.hCost = GetDistance(neigbour, endCell);
                    neigbour.parent = currentCell;

                    if (openSet.Contains(neigbour) == false)
                    {
                        openSet.Add(neigbour);
                    }
                }
            }

        }
        //Debug.Log("No path");
        return null;
    }

    public List<GridCell> FindCellsBetweenTwoPoints(Vector2 startPos, Vector2 endPos)
    {
        GridCell startCell = grid.GetCellByPos(startPos);
        GridCell endCell = grid.GetCellByPos(endPos);

        List<GridCell> path = new List<GridCell>();


        return path;
    }


    private List<GridCell> RetracePath(GridCell startNode, GridCell endNode)
    {
        List<GridCell> path = new List<GridCell>();
        GridCell curCell = endNode;

        while(curCell != startNode)
        {
            path.Add(curCell);
            curCell = curCell.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(GridCell cellA, GridCell cellB)
    {
        int distantX = Mathf.Abs(cellA.gridPos.x - cellB.gridPos.x);
        int distantY = Mathf.Abs(cellA.gridPos.y - cellB.gridPos.y);

        if (distantX > distantY)
            return 14 * distantY + 10 * (distantX - distantY);
        else return 14 * distantX + 10 * (distantY - distantX);
    }



}
