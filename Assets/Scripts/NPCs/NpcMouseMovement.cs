using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMouseMovement : Npc
{
    SelectionManager selectionManager;
    [SerializeField] float speed;
    public bool selected { get { if (selectionManager.GetCurSelection() != null && selectionManager.GetCurSelection().Equals(this.transform)) return true; else return false; ; } }

    protected override void Awake()
    {
        base.Awake();
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

        if (selected && selectionManager.IsHighlighting() == false)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gameManager.ResetOccupyingGameobjects();
            List<Grid.GridCell> path = new List<Grid.GridCell>();

            //if (movePoints > 0) 
            path = pathfinding.FindPath(transform.position, pos);

            gameManager.DrawPath(path, movePoints);

            if (Input.GetMouseButtonDown(0) && movePoints > 0 && gameManager.MovingObjects == false)
            {
                int pathCellNum = 0;
                while (movePoints > 0)
                {
                    if (path != null && path.Count > 0 && pathCellNum < path.Count)
                    {
                        //movePoints--;
                        //movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);

                        if (base.CrRunning == false)
                        movement2D.MoveTo(path,this,movePoints,speed,grid);

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
