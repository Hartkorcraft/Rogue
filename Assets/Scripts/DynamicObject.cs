using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour, IDamagable
{
    //protected Vector2Int gridPos = new Vector2Int();
    public Vector2Int gridpos { get => new Vector2Int(grid.GetCellPos(transform.position).x, grid.GetCellPos(transform.position).y); }

    protected Grid grid;
    protected GameManager gameManager;
    protected Movement2D movement2D;
    public bool selectable = true;

    [SerializeField] protected HealthSystem healthSystem = new HealthSystem(1, 1);
    public float TotalHealthPoints { get => healthSystem.TotalHealthPoints; set => healthSystem.TotalHealthPoints = value; }
    public float HealthPoints { get => healthSystem.HealthPoints; set => healthSystem.HealthPoints = value; }

    protected virtual void Awake()
    {
        movement2D = gameObject.AddComponent<Movement2D>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    protected virtual void Start()
    {
        grid.OccupyCell(grid.GetCellByPos(transform.position), gameObject);
        gameManager.AddDynamicObject(this.gameObject);
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);
    }

    public virtual void Damage(float damage)
    {
        HealthPoints = HealthPoints - damage;
        if (HealthPoints <= 0) Kill();
    }

    public virtual void Kill()
    {
        gameManager.Kill(this.gameObject);
        if (healthSystem.DeathParticle != null) { Instantiate(healthSystem.DeathParticle, transform.position, healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Killed");
    }

    public virtual void Push(Grid.Direction direction, int strength)
    {
        float pushSpeed = 100f;
        List<GridCell> path = new List<GridCell>();
        Vector2Int dir;

        for (int i = 1; i < strength + 1; i++)
        {
            GridCell cell;
            switch (direction)
            {
                
                case Grid.Direction.up:
                    dir = new Vector2Int(0, i);
                    break;
                case Grid.Direction.down:
                    dir = new Vector2Int(0, -i);
                    break;
                case Grid.Direction.left:
                    dir = new Vector2Int(-i, 0);
                    break;
                case Grid.Direction.right:
                    dir = new Vector2Int(i, 0);
                    break;
                default:
                    dir = new Vector2Int();
                    break;
            }

            cell = grid.GetCellByPos(new Vector2Int(gridpos.x + dir.x, gridpos.y + dir.y));
            if (cell != null && grid.CellStateBlocking(cell) == true && grid.IsCellOccupied(cell)) break;
            else
            {
                path.Add(cell);
            }
        }

        movement2D.MoveTo(path, this.gameObject, pushSpeed, grid);
    }


}
