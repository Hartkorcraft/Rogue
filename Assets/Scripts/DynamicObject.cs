using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour, IDamagable
{
    protected Vector2Int gridPos = new Vector2Int();
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

        Debug.Log(grid.GetCellByPos(transform.position).occupyingObject);
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


}
