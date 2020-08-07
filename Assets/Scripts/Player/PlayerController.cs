using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITurn
{
    private Grid grid;
    private PathFinding pathfinding;
    private GameManager gameManager;

    [SerializeField] private bool canMove = true;
    private bool moved = false;

    private Movement2D movement2D = new Movement2D();

    [SerializeField] private Transform target;

    [SerializeField] private int totalMovePoints = 5;
    [SerializeField] private int movePoints = 5;

    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
    }

    void Start()
    {
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f , grid.GetPosTransform(transform).y + 0.5f , transform.position.z) ;
        movePoints = totalMovePoints;
    }

    // Update is called once per frame  
    void Update()
    {

        Turn();

        if (Input.GetKeyDown(KeyCode.K))
        {
            List<Grid.GridCell> path = pathfinding.FindPath(transform.position, target.position);
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    grid.SetTile(path[i].gridPos, Grid.CellState.floor);
                }
            }
        }


    }

    public void Turn()
    {
        bool acted = false;

        //Moving
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || (Input.GetAxisRaw("Vertical") != 0))
            {
                if (moved == false)
                {

                    movement2D.MoveBy(transform.position, GetMovementDirection(), transform, grid);

                    moved = true;
                    movePoints--;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && (Input.GetAxisRaw("Vertical") == 0))
            {
                moved = false;
            }

            //if (movePoints <= 0) EndTurn();
        }



        if (movePoints <= 0) EndTurn();
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
