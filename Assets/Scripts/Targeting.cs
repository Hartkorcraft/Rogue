using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Targeting
{
    GameManager gameManager;
    PathFinding pathFinding;
    Grid grid;
    DynamicObject targetingObject;
    [SerializeField] public int targetingRange = 2;
    private bool isTargeting = false;
    public bool IsTargeting 
    {

        get
        {
            if(isTargeting)
            {
                gameManager.Targeting = true;
                return isTargeting;
            }
            else
            {
                return isTargeting;
            }
        }
        set
        {
            gameManager.Targeting = value;
            isTargeting = value;
        }
    }

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathFinding = grid.GetComponent<PathFinding>();
   }


    public GridCell Target(int range, bool direct)
    {
        if (IsTargeting == false) Debug.LogWarning("Not targeting?");
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameManager.ResetOccupyingGameobjects();
        List<GridCell> path = new List<GridCell>();

        path = pathFinding.FindPath(targetingObject.transform.position, pos, true);
        path = pathFinding.ReturnPathWithNoBlockin(path);

        grid.DrawPath(path, range, grid.pathTileRed, grid.pathTileBlue,true);

        if (Input.GetMouseButtonDown(0) && path != null && path.Count > 0 && path.Count <= range)
        {
            if (path[path.Count - 1].gridPos == UtilsHart.ToInt2(pos))
            {
                Debug.Log("Targeted");
                return grid.GetCellByPos(pos);
            }
            else if(direct == false)
            {
                return path[path.Count - 1];
            }
        }
        
        return null;
    }

    public Targeting(int targetingRange, DynamicObject targetingObject, GameManager gameManager, Grid grid, PathFinding pathFinding)
    {
        this.grid = grid;
        this.pathFinding = pathFinding;
        this.gameManager = gameManager;
        this.targetingRange = targetingRange;
        this.targetingObject = targetingObject;
    }



}
