using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GameManager gameManager;
    Grid grid;

    [SerializeField] private GridCellDestructable newCell = null;

    public GameObject testObject;

    public GameObject pushingObject;

    private void Awake()
    {
        gameObject.GetComponent<GameManager>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

    }


    void Start()
    {
        //grid.SetTile(new Vector2Int(4, 4), Grid.CellState.debug);

        Vector2Int pos = new Vector2Int(4, 4);

        //GridCellDestructable newCell = new GridCellDestructable(pos, Grid.CellState.wall, grid, 4, 4, Grid.CellState.ruins);
        newCell.grid = grid;
        if(newCell != null) grid.SetTile(newCell);

       
        GameObject _testObject = Instantiate(testObject, new Vector3(6, 6), new Quaternion(0,0,0,0));
        _testObject.GetComponent<DynamicObject>().selectable = false;
    }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pushingObject.GetComponent<DynamicObject>().Push(Grid.Direction.up, 3);
        }
    }
}
