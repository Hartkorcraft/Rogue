using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{

    private Grid grid;
    //public PathFinding pathFinding;

    //public int roomNumber = 5;
    public Vector2Int minRoomSize;
    public Vector2Int maxRoomSize;

    public int numberOfDivisions = 6;

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
            grid.gridCells = new Grid.GridCell[grid.GetGridSize().x][];
            Grid.Sector initialSector = new Grid.Sector(new Vector2Int(0, 0), grid.GetGridSize());
            grid.sectors.Add(initialSector);
            for (int x = 0; x < grid.GetGridSize().x; x++)
            {
                grid.gridCells[x] = new Grid.GridCell[grid.GetGridSize().y];

                for (int y = 0; y < grid.GetGridSize().y; y++)
                {
                    grid.gridCells[x][y] = new Grid.GridCell(new Vector2Int(x, y), Grid.CellState.available);

                    initialSector.sectorCells.Add(grid.gridCells[x][y]);
                }

            }
            Debug.Log("Dungeon was initialized");
        }

        //Setting Up Sectors
        {


            /*
            for (int i = 0; i < grid.sectors[0].sectorCells.Count; i++)
            {
                grid.sectors[0].sectorCells[i].cellstate = Grid.CellState.wall;
            }
            */
        }

        //Setting Up Rooms
        {


            Debug.Log("Rooms set");
        }


        //Setting up Paths
        {

          
            Debug.Log("Paths set");
        }





        //DebugSectors
        {
            
        }

        //Setting Tiles

        {
            grid.SetTiles();
        }






        //grid.DrawGrid();

    }

  




}
