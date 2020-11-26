using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour, ITurn, IDamagable
{
    protected Vector2Int gridPos = new Vector2Int();

    [SerializeField] protected int totalMovePoints = 3;
    [SerializeField] protected int movePoints = 1;
    [SerializeField] protected float speed = 40f;

    protected PathFinding pathFinding;
    protected Grid grid;
    protected PathFinding pathfinding;
    protected GameManager gameManager;


    [SerializeField] protected HealthSystem healthSystem = new HealthSystem(1,1);
    public float TotalHealthPoints { get => healthSystem.TotalHealthPoints; set => healthSystem.TotalHealthPoints = value; }
    public float HealthPoints { get => healthSystem.HealthPoints; set => healthSystem.HealthPoints = value; }

    protected Movement2D movement2D;
    [SerializeField] protected Transform playerPos;
    [SerializeField] protected bool canMove = true;

    protected virtual void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pathfinding = grid.GetComponent<PathFinding>();
        movement2D = gameObject.AddComponent<Movement2D>();
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
        List<GridCell> path = new List<GridCell>();
        if (movePoints>0)  path = pathfinding.FindPath(transform.position, playerPos.position);

        int pathCellNum = 0;
        while (movePoints > 0)
        {
            if (path != null && path.Count > 0 && pathCellNum < path.Count && canMove)
            {
                movePoints--;
                movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);
                //movement2D.MoveTo(path, this, movePoints, speed, grid);

                if (UtilsHart.ToInt2(new Vector2(transform.position.x, transform.position.y)) == UtilsHart.ToInt2(new Vector2(playerPos.position.x, playerPos.position.y))) break;
                pathCellNum++;
            }
            else
            {
                movePoints = 0;
                break;
            }
        }
        return true;
    }

    public void Damage(float damage)
    {
        HealthPoints =  HealthPoints - damage;
        if (HealthPoints <= 0) Kill(); 
    }    

    public void Kill()
    {
        gameManager.Kill(this.gameObject);
        if(healthSystem.DeathParticle != null) { Instantiate(healthSystem.DeathParticle, transform.position, healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Killed");
    }

    public Vector2Int GetGridPos()
    {
        gridPos = new Vector2Int(grid.GetCellPos(transform.position).x, grid.GetCellPos(transform.position).y);
        return gridPos;
    }

    

}
