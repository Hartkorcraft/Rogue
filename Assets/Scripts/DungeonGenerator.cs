using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{

    private Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();

        StartDungeonGeneration();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartDungeonGeneration();
        }

    }

    public void StartDungeonGeneration()
    {
        Debug.Log("Started Dungeon Generation");
        grid.TileInitialize();

        grid.floorTiles.ClearAllTiles();
        grid.darknessTiles.ClearAllTiles();
        grid.wallTiles.ClearAllTiles();


        //Initializing Dungeon
        {
            grid.gridCells = new GridCell[grid.GetGridSize().x][];
            Grid.Sector initialSector = new Grid.Sector(new Vector2Int(0, 0), grid.GetGridSize());
            grid.sectors.Add(initialSector);
            for (int x = 0; x < grid.GetGridSize().x; x++)
            {
                grid.gridCells[x] = new GridCell[grid.GetGridSize().y];

                for (int y = 0; y < grid.GetGridSize().y; y++)
                {
                    grid.gridCells[x][y] = new GridCell(new Vector2Int(x, y), Grid.CellState.available, grid);

                    initialSector.sectorCells.Add(grid.gridCells[x][y]);
                }

            }
            Debug.Log("Dungeon was initialized");
        }

        
        for (int x = 0; x < grid.GetGridSize().x; x++)
        {
            for (int y = 0; y < grid.GetGridSize().y; y++)
            {
                grid.gridCells[x][y].cellstate = Grid.CellState.floor;
            }
        }

        //Setting Tiles

        {
            grid.SetTiles();
        }


    }

  




}
