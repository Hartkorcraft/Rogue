using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour, ITurn
{
    protected Vector2Int gridPos = new Vector2Int();

    [SerializeField] protected int totalMovePoints = 3;
    [SerializeField] protected int movePoints = 1;

    protected PathFinding pathFinding;
    protected Grid grid;
    protected PathFinding pathfinding;
    protected GameManager gameManager;
    protected Movement2D movement2D;
    [SerializeField] protected Transform playerPos;
    
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
        grid.OccupyCell(grid.GetCellByPos(transform.position), gameObject);

        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);
    }
    public virtual bool Turn()
    {
        gameManager.ResetOccupyingGameobjects();
        movePoints = totalMovePoints;
        List<Grid.GridCell> path = new List<Grid.GridCell>();
        if (movePoints>0)  path = pathfinding.FindPath(transform.position, playerPos.position);

        int pathCellNum = 0;
        while (movePoints >= 0)
        {
            if (path != null && path.Count > 0 && pathCellNum < path.Count)
            {
                movePoints--;
                movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);
                pathCellNum++;
            }
            else
            {
                Debug.Log("No path");
                movePoints = 0;
                break;
            }
        }
        return true;
    }

    public Vector2Int GetGridPos()
    {
        gridPos = new Vector2Int(grid.GetCellPos(transform.position).x, grid.GetCellPos(transform.position).y);
        return gridPos;
    }
}
