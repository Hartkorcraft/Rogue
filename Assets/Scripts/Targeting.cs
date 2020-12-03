using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    GameManager gameManager;
    PathFinding pathFinding;
    Grid grid;
    DynamicObject targetingObject;
    public bool isTargeting = false;
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
        targetingObject = gameObject.GetComponent<DynamicObject>();
    }


    public GridCell Target(int range)
    {
        if (IsTargeting == false) Debug.LogWarning("Not targeting?");
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameManager.ResetOccupyingGameobjects();
        List<GridCell> path = new List<GridCell>();

        path = pathFinding.FindPath(targetingObject.transform.position, pos, true);
        path = pathFinding.ReturnPathWithNoBlockin(path);

        grid.DrawPath(path, range, grid.pathTileRed, grid.pathTileBlue);

        if (Input.GetMouseButtonDown(0) && path != null && path.Count > 0 && path[path.Count - 1].gridPos == UtilsHart.ToInt2(pos) && path.Count<= range)
        {
            Debug.Log("Targeted");
            return grid.GetCellByPos(pos);
        }
        return null;
    }

}
