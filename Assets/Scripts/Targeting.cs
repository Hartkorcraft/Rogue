using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    GameManager gameManager;
    PathFinding pathFinding;
    Grid grid;
    DynamicObject targetingObject;
    bool isTargeting = false;
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
            if(gameManager == null) gameManager = gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

    public void Target()
    {
        if (IsTargeting == false) Debug.LogWarning("Not targeting?");
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameManager.ResetOccupyingGameobjects();
        List<GridCell> path = new List<GridCell>();

        path = pathFinding.FindPath(targetingObject.transform.position, pos, true);
        path = pathFinding.ReturnPathWithNoBlockin(path);

        grid.DrawPath(path, path.Count);

        if (Input.GetMouseButtonDown(0) && path != null && path.Count > 0 && path[path.Count - 1].gridPos == UtilsHart.ToInt2(pos))
        {
            Debug.Log("Attacked!");


            if (grid.GetCellByPos(pos).GetOccupiyingObject() != null && grid.GetCellByPos(pos).GetOccupiyingObject() != this.gameObject && grid.GetCellByPos(pos).GetOccupiyingObject().GetComponent<IDamagable>() != null)
            {
                Debug.Log("Hit!");
                IDamagable idamagableObject = grid.GetCellByPos(pos).GetOccupiyingObject().GetComponent<IDamagable>();
                DynamicObject dynamicObject = grid.GetCellByPos(pos).GetOccupiyingObject().GetComponent<DynamicObject>();
                idamagableObject.Damage(1);
                if (dynamicObject != null) dynamicObject.Push(UtilsHart.GetDir(targetingObject.gridpos, dynamicObject.gridpos), 1);
            }

            if (grid.GetCellByPos(pos) is GridCellDestructable)
            {
                GridCellDestructable newCell = (GridCellDestructable)grid.GetCellByPos(pos);
                newCell.Damage(1);
            }


        }

    }






}
