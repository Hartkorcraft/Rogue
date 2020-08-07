using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{

    public bool MoveTo(Vector2Int pos, Transform transform)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        return true;
    }

    //MoveTo with Condition
    public bool MoveTo(Vector2Int pos, Transform transform, Grid grid)
    {
        if (grid.GetCellState(pos) != Grid.CellState.wall)
        {
            transform.position = new Vector3(pos.x,pos.y, transform.position.z);
            return true;
        }
        else return false;
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

        if (grid.gridCellsRangeCheck(newPos) == false)
        {
            transform.position = newPos;
            return true;
        }
        else if(grid.GetCellState(newPos) != Grid.CellState.wall)
        {
            transform.position = newPos;
            return true;
        }
        else return false;
    }
}
