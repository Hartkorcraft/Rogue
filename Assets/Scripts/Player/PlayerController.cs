using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Grid grid;
    private PathFinding pathfinding;
    private GameManager gameManager;
    private Movement2D movement2D;
    SelectionManager selectionManager;

    cakeslice.Outline outline;
    private bool moved = false;
    [SerializeField] private bool canMove = true;
    [SerializeField] private int totalMovePoints = 5;
    [SerializeField] private int movePoints = 5;
    [SerializeField] private bool moveWithMouse = false;
    [SerializeField] private bool endTurnAfterMovePoint = false;
    [SerializeField] protected float speed = 40f;
    [SerializeField] private bool targeting = false;

    void Awake()
    {

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
        movement2D = gameObject.AddComponent<Movement2D>();
        //movement2D = new Movement2D(gameManager);

        selectionManager = GameObject.FindGameObjectWithTag("SelectionManager").GetComponent<SelectionManager>();
        if (GetComponent<cakeslice.Outline>() == null)
        {
            outline = gameObject.AddComponent<cakeslice.Outline>();
            outline.enabled = false;
        }
    }

    void Start()
    {
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f , grid.GetPosTransform(transform).y + 0.5f , transform.position.z) ;
        movePoints = totalMovePoints;
        gameManager.AddDynamicObject(gameObject);
        grid.OccupyCell(grid.GetCellByPos(transform.position), gameObject);
    }

    // Update is called once per frame  
    void LateUpdate()
    {
        if (selected) outline.enabled = true;
        else
        {
            outline.enabled = false;
            targeting = false;
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
            targeting = !targeting;
        }

        if (targeting == false)
        {
            gameManager.Targeting = false;
            Movement();
        }
        else if (selected)
        {
            gameManager.Targeting = true;
            Targeting();
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

                if (totalMovePoints > 0) gameManager.DrawPath(path, movePoints);
                else gameManager.DrawPath(path, 50);

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

    public void Targeting()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameManager.ResetOccupyingGameobjects();
        List<GridCell> path = new List<GridCell>();

        path = pathfinding.FindPath(transform.position, pos);
        gameManager.DrawPath(path, movePoints);

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attacked!");
            Debug.Log(grid.GetCellByPos(pos).GetOccupiyingObject());
            if (grid.GetCellByPos(pos).GetOccupiyingObject() != null && grid.GetCellByPos(pos).GetOccupiyingObject().GetComponent<IDamagable>() != null)
            {
                Debug.Log("Hit!");
                IDamagable idamagableObject = grid.GetCellByPos(pos).GetOccupiyingObject().GetComponent<IDamagable>();
                idamagableObject.Damage(1);
            }

            if(grid.GetCellByPos(pos) is GridCellDestructable)
            {
                GridCellDestructable newCell = (GridCellDestructable)grid.GetCellByPos(pos);
                newCell.Damage(1);
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
