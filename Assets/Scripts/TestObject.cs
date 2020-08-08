using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour, ITurn
{
    private Vector2Int gridPos = new Vector2Int();

    [SerializeField] private int totalMovePoints = 1;
    [SerializeField] private int movePoints = 1;

    private PathFinding pathFinding;
    private Grid grid;
    private PathFinding pathfinding;
    private GameManager gameManager;
    private Movement2D movement2D;
    [SerializeField] private Transform playerPos;
    
    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
        movement2D = new Movement2D(gameManager);
    }

    private void Start()
    {
        gameManager.AddITurn(this,this.gameObject);
        

        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);
    }
    public bool Turn()
    {
        movePoints = totalMovePoints;
        while (movePoints > 0)
        {
            movePoints--;
            List<Grid.GridCell> path = pathfinding.FindPath(transform.position, playerPos.position);
            if (path != null && path.Count>1) movement2D.MoveTo(path[0].gridPos, transform, grid);
        }
        return true;
    }

    public Vector2Int GetGridPos()
    {
        gridPos = new Vector2Int(grid.GetCellPos(transform.position).x, grid.GetCellPos(transform.position).y);
        return gridPos;
    }
}
