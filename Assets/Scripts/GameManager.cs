using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private int turnNum = 0;
    private List<ITurn> turnObjects = new List<ITurn>();
    private Grid grid;
    private List<GameObject> dynamicObjects = new List<GameObject>();
    private HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    private GameObject playerObject;
    private SelectionManager selectionManager;

    [SerializeField] bool moveWithMouse = false;

    private bool movingObjects = false;
    public bool MovingObjects { get => movingObjects;  set => movingObjects = value;}


    private bool targeting = false;
    public bool Targeting { get => targeting; set => targeting = value; }

    private void Awake()
    {
        selectionManager = GameObject.FindGameObjectWithTag("SelectionManager").GetComponent<SelectionManager>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        Debug.Log("Next Turn");

        ResetOccupyingGameobjects();
        //grid.RemoveOccupiedObject(playerObject);

        turnNum++;
        for (int i = 0; i < turnObjects.Count; i++)
        {
            turnObjects[i].Turn();
        }

        for (int i = 0; i < grid.occupiedCells.Count; i++)
        {
            //grid.SetTile(grid.occupiedCells[i].gridPos, Grid.CellState.path);
        }
    }

    public void ResetOccupyingGameobjects()
    {
        grid.ResetOccupiedCells();

        for (int i = 0; i < dynamicObjects.Count; i++)
        {
            grid.OccupyCell(grid.GetCellByPos(dynamicObjects[i].transform.position), dynamicObjects[i]);
        }
    }
    /*
        public void ResetOccupyingGameobject(GameObject gameObject)
        {

        }
    */

    public bool TurnMapEdition()
    {
        return grid.TurnMapEdition();
    }

    public void DrawPath(List<Grid.GridCell> path, int distance)
    {
        grid.DrawPath(path, distance);
    }
    public void ClearPath()
    {
        grid.ClearPath();
    }

    public void AddITurn(ITurn interfaceComponent,GameObject gameObject)
    {
        turnObjects.Add(interfaceComponent);
        dynamicObjects.Add(gameObject);
    }
    public void AddDynamicObject(GameObject gameObject)
    {
        dynamicObjects.Add(gameObject);
    }
    public bool CheckPosition(Vector2Int pos)
    {
        if (positions.Contains(pos)) return true;
        else return false;
    }

    public void KillAllNpcs()
    {
        for (int i = dynamicObjects.Count - 1; i > 0; i--)
        {
            Destroy(dynamicObjects[i]);
            dynamicObjects.RemoveAt(i);
        }
        Debug.Log("killed all npcs");
    }


    /*
        public void ResetPositions()
        {
            positions.Clear();

            for (int i = dynamicObjects.Count - 1; i >= 0; i--)
            {
                if (dynamicObjects[i] == null)
                {
                    dynamicObjects.RemoveAt(i);
                    continue;
                }
                positions.Add(UtilsHart.ToInt2(dynamicObjects[i].transform.position));

            }

    *//*        for (int i = 0; i < dynamicObjects.Count; i++)
            {
                if(positions.Contains(UtilsHart.ToInt2(dynamicObjects[i].transform.position)))
                {
                    grid.SetTile(UtilsHart.ToInt2(dynamicObjects[i].transform.position), Grid.CellState.path);
                }
            }*//*
        }

        public void AddPos(Vector2Int pos)
        {
            positions.Add(pos);
        }

        public void RemovePos(Vector2Int pos)
        {
            positions.Remove(pos);
        }

        public HashSet<Vector2Int> GetPositions()
        {
            if (positions.Count <= 0) ResetPositions();
            return positions;
        }


        public void ChangePos(Vector2Int pos, Vector2Int newPos)
        {
            if (positions.Contains(pos))
            {
                positions.Remove(pos);
                positions.Add(newPos);
            }

        }
    */
}
