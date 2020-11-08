using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D
{
    private GameManager gameManager;
    public Movement2D(GameManager _gameManager)
    {
        gameManager = _gameManager;
    }


    public bool MoveTo(Vector2Int pos, Transform transform)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        return true;
    }

    //MoveTo with Condition
    public bool MoveTo(Vector2Int pos, Transform transform, Grid grid)
    {
        Vector2Int _pos = UtilsHart.ToInt2(transform.position);


        if (grid.GetCellState(pos) != Grid.CellState.wall && grid.IsCellOccupied(grid.GetCellByPos(pos)) == false)
        {
            transform.position = new Vector2(pos.x+0.5f,pos.y+0.5f);
            return true;
        }

        return false;
    }

    public bool MoveBy(Vector2Int dir, Transform transform)
    {

        Vector2Int newPos = new Vector2Int(UtilsHart.ToInt2(transform.position).x + dir.x, UtilsHart.ToInt2(transform.position).y + dir.y);
        transform.position = (Vector2)newPos;

        return true;
    }

    //MoveBy with Condition
    public bool MoveBy(Vector2Int dir, Transform transform, Grid grid)
    {
        grid.RemoveOccupiedObject(transform.gameObject);

        Vector2Int newPos = new Vector2Int(UtilsHart.ToInt2(transform.position).x + dir.x, UtilsHart.ToInt2(transform.position).y + dir.y);

        Collider2D hit = Physics2D.OverlapCircle(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y), 0.5f, 1 << LayerMask.NameToLayer("Blocked"));

        if (hit == false)
        {
            if (grid.GridCellsRangeCheck((Vector2)(newPos)) == false || grid.GetCellState(newPos) != Grid.CellState.wall)
            {
                grid.OccupyCell(grid.GetCellByPos(newPos),transform.gameObject);

                transform.position = new Vector2(newPos.x + 0.5f, newPos.y + 0.5f);
                return true;
            }
        }
        grid.OccupyCell(grid.GetCellByPos(transform.position), transform.gameObject);


        return false;
    }
}
