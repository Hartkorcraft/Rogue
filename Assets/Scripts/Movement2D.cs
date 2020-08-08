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
        gameManager.ChangePos(UtilsHart.ToInt2(transform.position), pos);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        return true;
    }

    //MoveTo with Condition
    public bool MoveTo(Vector2Int pos, Transform transform, Grid grid)
    {
        Collider2D hit = Physics2D.OverlapCircle(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.5f, 1 << LayerMask.NameToLayer("Blocked"));
        Vector2Int _pos = UtilsHart.ToInt2(transform.position);


        if (grid.GetCellState(pos) != Grid.CellState.wall && hit == false && gameManager.CheckPosition(pos) == false)
        {
            gameManager.ChangePos(UtilsHart.ToInt2(transform.position), pos);
            transform.position = new Vector2(pos.x+0.5f,pos.y+0.5f);
            return true;
        }

        return false;
    }

    public bool MoveBy(Vector2 curPos, Vector2 dir, Transform transform)
    {

        Vector2 newPos = new Vector2(curPos.x + dir.x, curPos.y + dir.y);
        transform.position = newPos;

        return true;

    }

    //MoveBy with Condition
    public bool MoveBy(Vector2 curPos, Vector2 dir, Transform transform, Grid grid)
    {

        Vector2 newPos = new Vector2(curPos.x + dir.x, curPos.y + dir.y);


        Collider2D hit = Physics2D.OverlapCircle(new Vector2(curPos.x + dir.x, curPos.y + dir.y), 0.5f, 1 << LayerMask.NameToLayer("Blocked"));

        if (hit == false)
        {
            if (grid.gridCellsRangeCheck(newPos) == false)
            {
                transform.position = newPos;
                return true;
            }
            else if (grid.GetCellState(newPos) != Grid.CellState.wall)
            {
                transform.position = newPos;
                return true;
            }
        }
        return false;
    }
}
