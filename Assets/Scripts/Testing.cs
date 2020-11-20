using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GameManager gameManager;
    Grid grid;

    private void Awake()
    {
        gameObject.GetComponent<GameManager>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

    }


    void Start()
    {
        //grid.SetTile(new Vector2Int(4, 4), Grid.CellState.debug);

        Vector2Int pos = new Vector2Int(4, 4);
        Grid.GridCellDestructable newCell = new Grid.GridCellDestructable(pos, Grid.CellState.wall, grid, 1, 1, Grid.CellState.debug);
        grid.SetTile(newCell);
        newCell.helo();

        if (newCell is Grid.GridCellDestructable)
        {
            Debug.Log("SEX");
        }
    }


}
