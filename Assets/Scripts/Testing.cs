using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GameManager gameManager;
    Grid grid;

    [SerializeField] private GridCellDestructable newCell = null;


    public GameObject pushingObject;
    public DynamicObject dynamicObject;

    private void Awake()
    {
        gameObject.GetComponent<GameManager>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

    }


    void Start()
    {
        //grid.SetTile(new Vector2Int(4, 4), Grid.CellState.debug);

        Vector2Int pos = new Vector2Int(4, 4);

        newCell.grid = grid;
        if(newCell != null) grid.SetTile(newCell);

        grid.SetTile(new Vector2Int(5, 1), Grid.CellState.wall);

        dynamicObject.selectable = false;
        dynamicObject.HealthPoints = 5;
        Instantiate(dynamicObject.gameObject, new Vector3(6, 6), new Quaternion(0,0,0,0));
    }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Pushed");
            Movement2D movement2d = pushingObject.GetComponent<DynamicObject>().GetComponent<Movement2D>();
            pushingObject.GetComponent<DynamicObject>().Push(Grid.Direction.up, 3);
        }
    }
}
