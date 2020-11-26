using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
    public bool MoveTo(List<GridCell> path, GameObject movingObject, int movePoints, float speed, Grid grid)
    {

        for (int i = 0; i < path.Count; i++)
        {

            if (grid.GetCellState(path[i].gridPos) == Grid.CellState.wall && grid.IsCellOccupied(grid.GetCellByPos(path[i].gridPos)))
                return false;
        }

        movingObject.GetComponent<MonoBehaviour>().StartCoroutine(Transition(path, movingObject.transform,movePoints,speed));
        gameManager.ResetOccupyingGameobjects();

        return false;
    }

    bool crTransitionRunning = false;
    public bool CrTransitionRunning { get { return crTransitionRunning; } }

    public IEnumerator Transition(List<GridCell> path, Transform transform, int movePoints, float speed)
    {
        gameManager.MovingObjects = true;
        crTransitionRunning = true;

        int moves;

        if (path.Count > movePoints)
        {
            moves = movePoints;
        }
        else
        {
            moves = path.Count;
        }

        for (int i = 0; i < moves; i++)
        {
            Vector3 target = new Vector3(path[i].gridPos.x + 0.5f, path[i].gridPos.y + 0.5f);
            while (Vector3.Distance(transform.position, target) > 0.001f)
            {
                transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);

                yield return null;
            }

        }

        Debug.Log("CrEnded");
        crTransitionRunning = false;
        gameManager.MovingObjects = false;
        gameManager.ResetOccupyingGameobjects();
        yield return null;

    }


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
