using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Grid : MonoBehaviour
{

    //Debug Stuff
    [SerializeField] private bool debugChangeTile = false;
    [SerializeField] private CellState selectedChangeTile = CellState.wall;




    //Normal Stuff
    public enum Direction { up, down, left, right };
    [SerializeField] public enum CellState { available, floor, entrance, path, wall, darkness, debug, noCell };

    public Sprite floorSprite;
    public Tilemap floorTiles;
    private Tile floorTile;

    public Sprite darknessSprite;
    public Tilemap darknessTiles;
    private Tile darknessTile;

    public Sprite wallSprite;
    public Sprite wallBlockedSprite; //Temporary sprite
    public Tilemap wallTiles;
    private Tile wallTile;
    private Tile wallBlockedTile;

    public Sprite entranceSprite;
    private Tile entranceTile;

    public Sprite debugSprite;
    private Tile debugTile;

    public Sprite pathSprite;
    private Tile pathTile;


    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(20, 20);

    public GridCell[][] gridCells;

    public List<Sector> sectors = new List<Sector>();
    public List<Room> rooms = new List<Room>();

    private void Awake()
    {

        floorTile = ScriptableObject.CreateInstance<Tile>();
        floorTile.sprite = floorSprite;

        darknessTile = ScriptableObject.CreateInstance<Tile>();
        darknessTile.sprite = darknessSprite;

        wallTile = ScriptableObject.CreateInstance<Tile>();
        wallTile.sprite = wallSprite;

        wallBlockedTile = ScriptableObject.CreateInstance<Tile>();
        wallBlockedTile.sprite = wallBlockedSprite;

        entranceTile = ScriptableObject.CreateInstance<Tile>();
        entranceTile.sprite = entranceSprite;

        pathTile = ScriptableObject.CreateInstance<Tile>();
        pathTile.sprite = pathSprite;

        debugTile = ScriptableObject.CreateInstance<Tile>();
        debugTile.sprite = debugSprite;
    }

    private void Update()
    {
        if (debugChangeTile == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (gridCellsRangeCheck(pos) == true)
                    SetTile(GetCellPos(pos), selectedChangeTile);

            }
        }
    }

    public void SetTiles()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                switch (gridCells[x][y].cellstate)
                {
                    case CellState.available:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), debugTile);
                        break;

                    case CellState.darkness:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), darknessTile);
                        break;

                    case CellState.floor:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), floorTile);
                        break;

                    case CellState.wall:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), wallTile);
                        break;

                    case CellState.path:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), pathTile);
                        break;
                    case CellState.entrance:

                        darknessTiles.SetTile(new Vector3Int(x, y, 0), entranceTile);
                        break;

                    case CellState.debug:
                        darknessTiles.SetTile(new Vector3Int(x, y, 0), debugTile);
                        break;

                    default:
                        Debug.LogWarning("No cell state?");
                        break;
                }

            }

        }
    } 
    public void SetTile(Vector2Int pos, CellState cellState)
    {
            gridCells[pos.x][pos.y].cellstate = cellState;
            switch (gridCells[pos.x][pos.y].cellstate)
            {
                case CellState.available:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                    break;

                case CellState.darkness:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), darknessTile);
                    break;

                case CellState.floor:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), floorTile);
                    break;

                case CellState.wall:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), wallTile);
                    break;

                case CellState.path:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), pathTile);
                    break;
                case CellState.entrance:

                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), entranceTile);
                    break;

                case CellState.debug:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                    break;

                default:
                    Debug.LogWarning("No cell state?");
                    break;
            }
        
    }
    
    public void SetTile(Vector2Int pos)
    {
            switch (gridCells[pos.x][pos.y].cellstate)
            {
                case CellState.available:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                    break;

                case CellState.darkness:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), darknessTile);
                    break;

                case CellState.floor:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), floorTile);
                    break;

                case CellState.wall:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), wallTile);
                    break;

                case CellState.path:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), pathTile);
                    break;
                case CellState.entrance:

                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), entranceTile);
                    break;

                case CellState.debug:
                    darknessTiles.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                    break;

                default:
                    Debug.LogWarning("No cell state?");
                    break;
            }
        
    }

    public Vector2Int GetGridSize() { return gridSize; }

    public Vector2Int GetCellPos(Vector2 pos)
    {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y));
        return cellPos;
    }
    public CellState GetCellState(Vector2 pos)
    {
        Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
        if (gridCellsRangeCheck(cellPos))
        {
            return gridCells[cellPos.x][cellPos.y].cellstate;
        }
        else return CellState.noCell;
    }

    
    public bool gridCellsRangeCheck(Vector2Int index)
    {
        if ((index.x > 0 && index.x <= gridSize.x) && (index.y > 0 && index.y <= gridSize.y))
            return true;
        else return false;
    }
    public bool gridCellsRangeCheck(Vector2 pos)
    {
        if ((pos.x > 0 && pos.x <= gridSize.x) && (pos.y > 0 && pos.y <= gridSize.y))
            return true;
        else return false;
    }

    public bool gridCellsRangeCheck(Vector3 pos)
    {
        Debug.Log(pos);
        if ((pos.x > 0 && pos.x <= gridSize.x) && (pos.y > 0 && pos.y <= gridSize.y))
            return true;
        else return false;
    }

    public Vector2Int GetPosTransform(Transform transform)
    {
        Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x),Mathf.FloorToInt(transform.position.y));
        return pos;
    }

    //public float GetTileSize() { return tileSize = Mathf.Abs(gridCells[0][0].gridPos.y - gridCells[0][1].gridPos.y);  }
    public GridCell GetCellByPos(Vector2Int pos)
    {
        if (gridCellsRangeCheck(pos))
            return gridCells[pos.x][pos.y];
        else return null;
    }

    
    public Sector GetBiggestSector()
    {
        Sector sector = sectors[0];

        for (int i = 0; i < sectors.Count; i++)
        {
            if (sectors[i].size.x * sectors[i].size.y > sector.size.x * sector.size.y) sector = sectors[i];
        }
        return sector;
    }


    public void DivideSector(Sector sector, Vector2Int minSize)
    {
        if (sector.size.x > sector.size.y)
        {

        }
        else
        {

        }

    }

    public class GridCell
    {
        public Vector2Int gridPos;
        public Vector2 worldPos;
        public CellState cellstate;
        //public int index;

        public GridCell(Vector2Int gridPos, Vector2 worldPos, CellState cellState)
        {
            this.gridPos = gridPos;
            this.worldPos = worldPos;
            this.cellstate = cellState;
            //this.index = index;
        }


    }

    [System.Serializable]
    public class Room
    {
        public Vector2Int pos;
        public Vector2Int size;
        public List<GridCell> roomCells = new List<GridCell>();
        public List<GridCell> aroundCells = new List<GridCell>();

        public Room(Vector2Int pos, Vector2Int size)
        {
            this.pos = pos;
            this.size = size;
        }
    }

    [System.Serializable]
    public class Sector
    {
        public Vector2Int pos;
        public Vector2Int size;
        public Room room;

        public List<GridCell> sectorCells = new List<GridCell>();

        public Sector(Vector2Int pos, Vector2Int size)
        {
            this.pos = pos;
            this.size = size;
        }
    }
   



}
