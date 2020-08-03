using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{

    private float time = 0.5f;
    private IEnumerator coroutine;



    public bool MoveTo(Vector2Int pos, Transform transform)
    {
        transform.position = Vector2.Lerp(pos, transform.position, Time.deltaTime * time);
        return true;
    }

    //MoveTo with Condition
    public bool MoveTo(Vector2Int pos, Transform transform, Grid grid)
    {
        if (grid.GetCellState(pos) != Grid.CellState.wall)
        {
            transform.position = Vector2.Lerp(pos, transform.position, Time.deltaTime * time);
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
