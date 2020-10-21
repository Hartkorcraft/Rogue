﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Grid grid;
    private PathFinding pathfinding;
    private GameManager gameManager;

    [SerializeField] private bool canMove = true;
    private bool moved = false;

    private Movement2D movement2D;

    [SerializeField] private Transform target;

    [SerializeField] private int totalMovePoints = 5;
    [SerializeField] private int movePoints = 5;

    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
        movement2D = new Movement2D(gameManager);

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

        Turn();

/*        if (Input.GetKeyDown(KeyCode.K))
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
*/

    }

    public bool Turn()
    {
        //bool acted = false;

        //Moving
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || (Input.GetAxisRaw("Vertical") != 0))
            {
                if (moved == false)
                {

                    movement2D.MoveBy(GetMovementDirection(), transform, grid);

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            movePoints--;
        }

        if (movePoints <= 0) EndTurn();

        return true;
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
