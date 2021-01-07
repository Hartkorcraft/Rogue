using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : DynamicObject, ITurn, IDamagable
{

    [SerializeField] protected int totalMovePoints = 3;
    [SerializeField] protected int movePoints = 1;
    [SerializeField] protected float speed = 40f;

    protected PathFinding pathFinding;


    [SerializeField] protected Transform target;
    [SerializeField] protected bool canMove = true;

    private int inventorySize = 5;
    public int InventorySize 
    {
        get { return inventorySize;}
        set
        {
            if (value > inventorySize)
            {
                inventorySize = value;
                System.Array.Resize<Item>(ref inventory, inventorySize);
            }
            else Debug.LogWarning("smaller inventory size");
        }
            
    }

    [SerializeField] protected Item[] inventory;


    protected override void Awake()
    {
        inventory = new Item[inventorySize];

        base.Awake();
        pathFinding = grid.GetComponent<PathFinding>();
        gameManager.AddITurn(this, this.gameObject);

    }

    protected override void Start()
    {
        base.Start();
        //transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);

    }

    public virtual bool Turn()
    {
        gameManager.ResetOccupyingGameobjects();
        movePoints = totalMovePoints;

        Vector2Int pos = new Vector2Int(4, 3);
        pos = UtilsHart.ToInt2(target.position);

        if (target != null)
        {
            List<GridCell> path = new List<GridCell>();
            if (movePoints > 0) path = pathFinding.FindPath(transform.position, pos);
            int pathCellNum = 0;

            grid.DrawPath(path, movePoints, grid.pathTile, grid.pathTileBlue, true);

            while (movePoints > 0 || totalMovePoints <= 0)
            {

                if (path != null && path.Count > 0 && pathCellNum < path.Count && canMove)
                {

                    if (path.Count < 1)
                    {
                        if (grid.CellStateBlocking(grid.GetCellByPos(pos)) == true || grid.GetCellByPos(pos).HasCellObject() == true)
                        {
                            break;
                        }
                    }

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
                    Debug.Log("no path krulik");
                    movePoints = 0;
                    break;
                }
            }
        }

        return true;
    }


}
