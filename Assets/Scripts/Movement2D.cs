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
        gameManager.ResetOccupyingGameobjects();
        return true;
    }

    //MoveTo with Condition
    public bool MoveTo(Vector2Int pos, Transform transform, Grid grid)
    {
        Vector2Int _pos = UtilsHart.ToInt2(transform.position);


        if (grid.GetCellState(pos) != Grid.CellState.wall && grid.IsCellOccupied(grid.GetCellByPos(pos)) == false)
        {
            transform.position = new Vector2(pos.x+0.5f,pos.y+0.5f);
            gameManager.ResetOccupyingGameobjects();
            return true;
        }

        return false;
    }

    //MoveTo with Condition and speed
    public bool MoveTo(List<Grid.GridCell> path, Npc npc, int movePoints, float speed, Grid grid)
    {

        for (int i = 0; i < path.Count; i++)
        {

            if (grid.GetCellState(path[i].gridPos) != Grid.CellState.wall && grid.IsCellOccupied(grid.GetCellByPos(path[i].gridPos)) == false)
            {

            }
            else return false;
        }

        npc.StartCoroutine(npc.Transition(path, npc.transform,movePoints,speed));
        gameManager.ResetOccupyingGameobjects();

        return false;
    }


    ////////Debil ze mnie :) 
    /*
    //MoveTo with Condition and speed
    public bool MoveTo(Transform transform, Grid grid, List<Grid.GridCell> path, float speed)
    {
        Vector2Int _pos = UtilsHart.ToInt2(transform.position);

        for (int i = 0; i < path.Count; i++)
        {
            bool endedMovement = false;

            while (endedMovement == false)
            {
                Vector3 pos = new Vector3(path[i].gridPos.x, path[i].gridPos.y, transform.position.z);
                if (grid.GetCellState(path[i].gridPos) != Grid.CellState.wall && grid.IsCellOccupied(grid.GetCellByPos(path[i].gridPos)) == false)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pos, speed);

                }
                else return false;

                if (transform.position == pos) endedMovement = true;
            }
        }


        return true;
    }
    */
    public bool MoveBy(Vector2Int dir, Transform transform)
    {

        Vector2Int newPos = new Vector2Int(UtilsHart.ToInt2(transform.position).x + dir.x, UtilsHart.ToInt2(transform.position).y + dir.y);
        transform.position = (Vector2)newPos;
        gameManager.ResetOccupyingGameobjects();
        return true;
    }

    //MoveBy with Condition
    public bool MoveBy(Vector2Int dir, Transform transform, Grid grid)
    {
        grid.RemoveOccupiedObject(transform.gameObject);

        Vector2Int newPos = new Vector2Int(UtilsHart.ToInt2(transform.position).x + dir.x, UtilsHart.ToInt2(transform.position).y + dir.y);

        Collider2D hit = Physics2D.OverlapCircle(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y), 0.5f, 1 << LayerMask.NameToLayer("Blocked"));

        bool hasCellObject = false;
        if (grid.GetCellByPos(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y)) != null) hasCellObject = grid.GetCellByPos(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y)).HasCellObject();

        if (hit == false && hasCellObject == false)
        {
            if (grid.GridCellsRangeCheck((Vector2)(newPos)) == false || grid.GetCellState(newPos) != Grid.CellState.wall)
            {
                grid.OccupyCell(grid.GetCellByPos(newPos),transform.gameObject);

                transform.position = new Vector2(newPos.x + 0.5f, newPos.y + 0.5f);
                gameManager.ResetOccupyingGameobjects();
                return true;
            }
        }
        grid.OccupyCell(grid.GetCellByPos(transform.position), transform.gameObject);


        return false;
    }
}
