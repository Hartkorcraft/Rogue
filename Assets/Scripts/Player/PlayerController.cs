using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : DynamicObject
{
    private PathFinding pathfinding;
    private Targeting targeting;
    SelectionManager selectionManager;

    cakeslice.Outline outline;
    private bool moved = false;
    [SerializeField] private bool canMove = true;
    [SerializeField] private int totalMovePoints = 5;
    [SerializeField] private int movePoints = 5;
    [SerializeField] private bool moveWithMouse = false;
    [SerializeField] private bool endTurnAfterMovePoint = false;
    [SerializeField] protected float speed = 40f;
    [SerializeField] private int range = 5;

    protected override void Awake()
    {

        base.Awake();

        pathfinding = grid.GetComponent<PathFinding>();
        targeting = gameObject.AddComponent<Targeting>();

        selectionManager = GameObject.FindGameObjectWithTag("SelectionManager").GetComponent<SelectionManager>();
        if (GetComponent<cakeslice.Outline>() == null)
        {
            outline = gameObject.AddComponent<cakeslice.Outline>();
            outline.enabled = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f , grid.GetPosTransform(transform).y + 0.5f , transform.position.z) ;
        movePoints = totalMovePoints;
    }

    // Update is called once per frame  
    void LateUpdate()
    {
        if (selected) outline.enabled = true;
        else
        {
            outline.enabled = false;
            targeting.IsTargeting = false;
        }
        Turn();


    }


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

    public bool Turn()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            targeting.IsTargeting = !targeting.IsTargeting;
        }

        if (targeting.IsTargeting == false)
        {
            Movement();
        }
        else if (selected)
        {
           GridCell target = targeting.Target(range);
            if(target != null) { Attack(target); }
        }



        if (Input.GetKeyDown(KeyCode.T) && movement2D.CrTransitionRunning == false)
        {
            EndTurn();
        }

        if (Input.GetKeyDown(KeyCode.X) && movePoints > 0 && movement2D.CrTransitionRunning == false)
        {
            movePoints--;
        }

        if (movePoints <= 0 && (moveWithMouse && endTurnAfterMovePoint)) EndTurn();


        return true;
    }

    public void Attack(GridCell target)
    {
        if (grid.GetCellByPos(target.gridPos).GetOccupiyingObject() != null && target.GetOccupiyingObject() != this.gameObject && target.GetOccupiyingObject().GetComponent<IDamagable>() != null)
        {
            Debug.Log("Hit!");
            IDamagable idamagableObject = target.GetOccupiyingObject().GetComponent<IDamagable>();
            DynamicObject dynamicObject = target.GetOccupiyingObject().GetComponent<DynamicObject>();
            idamagableObject.Damage(1);
            if (dynamicObject != null) dynamicObject.Push(UtilsHart.GetDir(gridpos, dynamicObject.gridpos), 1);
        }

        if (grid.GetCellByPos(target.gridPos) is GridCellDestructable)
        {
            GridCellDestructable newCell = (GridCellDestructable)grid.GetCellByPos(target.gridPos);
            newCell.Damage(1);
        }
    }
    public void Attack(GridCell target, Grid.CellDepth cellDepth)
    {
        if(cellDepth == Grid.CellDepth.both)
        {
            Attack(target);
            return;
        }

        if (cellDepth == Grid.CellDepth.cellObject && grid.GetCellByPos(target.gridPos).GetOccupiyingObject() != null && target.GetOccupiyingObject() != this.gameObject && target.GetOccupiyingObject().GetComponent<IDamagable>() != null)
        {
            Debug.Log("Hit!");
            IDamagable idamagableObject = target.GetOccupiyingObject().GetComponent<IDamagable>();
            DynamicObject dynamicObject = target.GetOccupiyingObject().GetComponent<DynamicObject>();
            idamagableObject.Damage(1);
            if (dynamicObject != null) dynamicObject.Push(UtilsHart.GetDir(gridpos, dynamicObject.gridpos), 1);
        }

        if (cellDepth == Grid.CellDepth.cell && grid.GetCellByPos(target.gridPos) is GridCellDestructable)
        {
            GridCellDestructable newCell = (GridCellDestructable)grid.GetCellByPos(target.gridPos);
            newCell.Damage(1);
        }
    }

    public void Movement()
    {
        if (moveWithMouse)
        {
            if (selected) outline.enabled = true; else outline.enabled = false;

            if (selected && selectionManager.IsHighlighting() == false)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                gameManager.ResetOccupyingGameobjects();
                List<GridCell> path = new List<GridCell>();

                //if (movePoints > 0) 
                path = pathfinding.FindPath(transform.position, pos);

                if (totalMovePoints > 0) grid.DrawPath(path, movePoints, grid.pathTile,grid.pathTileBlue);
                else grid.DrawPath(path, 50,grid.pathTile);

                if (Input.GetMouseButtonDown(0) && (movePoints > 0 || totalMovePoints <= 0) && gameManager.MovingObjects == false && canMove)
                {
                    int pathCellNum = 0;
                    while (movePoints > 0 || totalMovePoints <= 0)
                    {
                        if (path != null && path.Count > 0 && pathCellNum < path.Count)
                        {
                            //movePoints--;
                            //movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);

                            if (movement2D.CrTransitionRunning == false)
                            {
                                if (totalMovePoints > 0)
                                    movement2D.MoveTo(path, this.gameObject, movePoints, speed, grid);
                                else
                                    movement2D.MoveTo(path, this.gameObject, 50, speed, grid);
                            }

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

                            //Debug.Log("Remaining move points " + movePoints);

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
                if (movePoints <= 0 && endTurnAfterMovePoint) EndTurn();
            }
        }
        else
        {

            if (gameManager.MovingObjects == false && (movePoints > 0 || totalMovePoints <= 0) && (Input.GetAxisRaw("Horizontal") != 0 || (Input.GetAxisRaw("Vertical") != 0)))
            {
                if (moved == false)
                {

                    moved = movement2D.MoveBy(GetMovementDirection(), transform, grid);

                    if (moved)
                        movePoints--;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && (Input.GetAxisRaw("Vertical") == 0))
            {
                moved = false;
            }

        }

    }

    public void EndTurn()
    {
        movePoints = totalMovePoints;
        gameManager.NextTurn();
    }

    private Vector2Int GetMovementDirection()
    {
        
        Vector2Int moveDir = new Vector2Int();
        moveDir.x = (int)(Input.GetAxisRaw("Horizontal"));
        moveDir.y = (int)(Input.GetAxisRaw("Vertical"));
        return moveDir;
    }



}
