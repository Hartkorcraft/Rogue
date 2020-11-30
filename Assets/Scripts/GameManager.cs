using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private int turnNum = 0;
    private List<ITurn> turnObjects = new List<ITurn>();
    private Grid grid;
    [SerializeField] private HashSet<GameObject> dynamicObjects = new HashSet<GameObject>();
    private HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    private GameObject playerObject;
    private SelectionManager selectionManager;


    [SerializeField] private bool movingObjects = false;
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

        foreach(GameObject dynamicObject in dynamicObjects)
        { 
            grid.OccupyCell(grid.GetCellByPos(dynamicObject.transform.position), dynamicObject);
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

    public void DrawPath(List<GridCell> path, int distance)
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

    public void DestroyAllDynamicObjects()
    {
        foreach(GameObject dynamicObject in dynamicObjects)
        { 
            Destroy(dynamicObject);
            dynamicObjects.Remove(dynamicObject);
        }
        Debug.Log("killed all dynamicObjects");
    }

    public void KillAllDynamicObjects()
    {
        foreach (GameObject dynamicObject in dynamicObjects)
        {
            dynamicObject.GetComponent<DynamicObject>().ForceKill();
            dynamicObjects.Remove(dynamicObject);
        }
        Debug.Log("killed all dynamicObjects");
    }
    public void KillAllNpcs()
    {
        foreach (GameObject dynamicObject in dynamicObjects)
        {
            Npc npc = dynamicObject.GetComponent<Npc>();
            if(npc != null)
            {
                npc.ForceKill();
                dynamicObjects.Remove(dynamicObject);
            }

        }
        Debug.Log("killed all dynamicObjects");
    }

    public void Kill(IDamagable damagable)
    {
        damagable.ForceKill();
    }

    public void DestroyDynamicObject(GameObject dynamicObject)
    {
        if (dynamicObjects.Contains(dynamicObject))
        {
            dynamicObjects.Remove(dynamicObject);
            Destroy(dynamicObject);
            Debug.Log("Destroyed Object ");
        }
        else
        {
            Debug.LogWarning("No dynamic object ");
            Destroy(dynamicObject);
        }
    }
}
