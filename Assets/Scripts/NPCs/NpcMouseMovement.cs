using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMouseMovement : Npc
{
    SelectionManager selectionManager;
    cakeslice.Outline outline;
    public bool selected 
    {
        get 
        {
            if (selectionManager.GetCurSelection() != null && selectionManager.GetCurSelection().Equals(this.transform))
            {
                return true;
            }
            else return false; ; 
        } 
    }

    protected override void Awake()
    {
        base.Awake();

        if (GetComponent<cakeslice.Outline>() == null)
        {
            outline = gameObject.AddComponent<cakeslice.Outline>();
            //outline.enabled = false;
        }

        selectionManager = GameObject.FindGameObjectWithTag("SelectionManager").GetComponent<SelectionManager>();
    }

    public override bool Turn()
    {
        movePoints = totalMovePoints;
        //Debug.Log("forg forg");
        //base.Turn();

        return true;
    }

    private void Update()
    {
        /*if (selected && movePoints <= 0) 
        {
            gameManager.ClearPath();
            selectionManager.ChangeCurSelection(null);
        }*/

        if (selected) outline.enabled = true; else outline.enabled = false;

        MouseMovement();
    }

    private void MouseMovement()
    {

        if (selected && selectionManager.IsHighlighting() == false)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gameManager.ResetOccupyingGameobjects();
            List<GridCell> path = new List<GridCell>();

            //if (movePoints > 0) 
            path = pathfinding.FindPath(transform.position, pos);

            gameManager.DrawPath(path, movePoints);

            if (Input.GetMouseButtonDown(0) && movePoints > 0 && gameManager.MovingObjects == false && canMove)
            {
                int pathCellNum = 0;
                while (movePoints > 0)
                {
                    if (path != null && path.Count > 0 && pathCellNum < path.Count)
                    {
                        //movePoints--;
                        //movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);

                        if (movement2D.CrTransitionRunning == false)
                            movement2D.MoveTo(path, this.gameObject, movePoints, speed, grid);

                        pathCellNum++;
                        if (UtilsHart.ToInt2(new Vector2(transform.position.x, transform.position.y)) == UtilsHart.ToInt2(new Vector2(pos.x, pos.y))) break;

                        int moves;
                        if (path.Count > movePoints)
                        {
                            moves = movePoints;
                        }
                        else
                        {
                            moves = path.Count;
                        }
                        movePoints = movePoints - moves;

                        if (movePoints <= 0)
                        {
                            selectionManager.RemoveSelection(this.transform);
                            grid.ClearPath();
                        }

                        break;
                    }
                    else
                    {
                        Debug.Log("No path");
                        movePoints = 0;
                        break;
                    }
                }

                Debug.Log("Moved");

            }
        }

    }
}
