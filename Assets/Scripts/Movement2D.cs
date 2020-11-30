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


        if (grid.CellStateBlocking(grid.GetCellByPos(pos)) == false && grid.IsCellOccupied(grid.GetCellByPos(pos)) == false)
        {
            transform.position = new Vector2(pos.x+0.5f,pos.y+0.5f);
            gameManager.ResetOccupyingGameobjects();
            return true;
        }

        return false;
    }

    //MoveTo with Condition, movepoints and speed
    public bool MoveTo(List<GridCell> path, GameObject movingObject, int movePoints, float speed, Grid grid)
    {

        for (int i = 0; i < path.Count; i++)
        {

            if (grid.CellStateBlocking(path[i]) == true && grid.IsCellOccupied(path[i]))
                return false;
        }

        movingObject.GetComponent<MonoBehaviour>().StartCoroutine(Transition(path, movingObject.transform,movePoints,speed, grid));
        gameManager.ResetOccupyingGameobjects();

        return false;
    }

    public bool MoveTo(List<GridCell> path, GameObject movingObject, float speed, Grid grid)
    {
        movingObject.GetComponent<MonoBehaviour>().StartCoroutine(Transition(path, movingObject.transform, speed, grid));
        gameManager.ResetOccupyingGameobjects();

        return false;
    }

    private bool crTransitionRunning = false;
    public bool CrTransitionRunning { get { return crTransitionRunning; } }

    //Transition with movePoints
    public IEnumerator Transition(List<GridCell> path, Transform transform, int movePoints, float speed, Grid grid)
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

            if (gameManager.MovingObjects == false)
            {
                Vector2Int _pos = UtilsHart.ToInt2(transform.position);
                transform.position = new Vector2(_pos.x + 0.5f, _pos.y + 0.5f);
                gameManager.ResetOccupyingGameobjects();
                break;
            }

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

    //Transition with movePoints
    public IEnumerator Transition(List<GridCell> path, Transform transform, float speed, Grid grid)
    {
        gameManager.MovingObjects = true;
        crTransitionRunning = true;

        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] == null || grid.CellStateBlocking(path[i]) == true || grid.IsCellOccupied(path[i])) break;
            Vector3 target = new Vector3(path[i].gridPos.x + 0.5f, path[i].gridPos.y + 0.5f);

            if (gameManager.MovingObjects == false)
            {
                Vector2Int _pos = UtilsHart.ToInt2(transform.position);
                transform.position = new Vector2(_pos.x + 0.5f, _pos.y + 0.5f);
                gameManager.ResetOccupyingGameobjects();
                break;
            }

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

    public void StopMovement(Transform transform)
    {
        if (crTransitionRunning)
        {
            gameManager.MovingObjects = false;
        }
        Debug.Log("Stopped movement");
    }

    public IEnumerator WaitToMove(List<GridCell> path, GameObject movingObject, float speed, Grid grid)
    {
        gameManager.MovingObjects = false;
        yield return new WaitForSeconds(0.05f);

        if (crTransitionRunning == false) MoveTo(path, movingObject, speed, grid);
        else StartCoroutine(WaitToMove(path, movingObject, speed, grid));
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
