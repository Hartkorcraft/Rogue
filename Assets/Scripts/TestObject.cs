using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour, ITurn
{
    [SerializeField] private int totalMovePoints = 1;
    [SerializeField] private int movePoints = 1;

    private PathFinding pathFinding;
    private Grid grid;
    private PathFinding pathfinding;
    private GameManager gameManager;

    [SerializeField] private Transform playerPos;
    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
    }

    private void Start()
    {
        Debug.Log(gameManager);
        gameManager.AddITurn(this);
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);
    }
    public void Turn()
    {
        movePoints = totalMovePoints;
        while (movePoints > 0)
        {
            movePoints--;
            List<Grid.GridCell> path = pathfinding.FindPath(transform.position, playerPos.position);
            if (path != null)  transform.position = new Vector2(path[0].gridPos.x +0.5f, path[0].gridPos.y+0.5f);
        }

    }
}
