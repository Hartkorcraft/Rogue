using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Grid : MonoBehaviour
{


    [HideInInspector] public bool debugChangeTile = false;
    [HideInInspector] public bool destructableDebugCell = false;

    public GridCell debCell = null;
    public GridCellDestructable debDestructableCell = null;

    [HideInInspector] public bool initializeTileMaps = false;


    public enum CellDepth { cell, cellObject, both}
    //Normal Stuff
    public enum Direction { up, down, left, right };
    [SerializeField] public enum CellState 
    { 
        available,
        floor,
        entrance,
        path,
        wall,
        floorPlank,
        darkness,
        debug,
        noCell,
        ruins,
        boundry
    };


    [HideInInspector] public Tilemap tilemap;

    [HideInInspector]
    public Tilemap
        floorTiles,
        darknessTiles,
        wallTiles,
        pathTiles,
        pathTilesDown;

    [HideInInspector] public Sprite 
        floorSprite = null,
        floorPlankSprite = null,
        darknessSprite = null,
        wallSprite = null,
        wallBlockedSprite = null,
        entranceSprite = null,
        debugSprite = null,
        pathSprite = null,
        pathSpriteRed = null,
        pathSpriteBlue = null,
        pathSpriteFull = null,
        ruinSprite = null,
        boundrySprite = null;

    [HideInInspector] public Tile 
        floorTile,
        floorPlankTile,
        darknessTile,
        wallTile,
        wallBlockedTile,
        entranceTile,
        debugTile,
        pathTile,
        pathTileRed,
        pathTileBlue,
        pathTileFull,
        ruinTile,
        boundryTile;


    public Vector2Int gridSize = new Vector2Int(20, 20);

    public GridCell[][] gridCells;
    public List<Sector> sectors = new List<Sector>();
    public List<Room> rooms = new List<Room>();

    //public List<GridCell> path = new List<GridCell>();

    public List<GridCell> occupiedCells = new List<GridCell>();

    public void TileInitialize()
    {

        floorTile = ScriptableObject.CreateInstance<Tile>();
        floorTile.sprite = floorSprite;

        floorPlankTile = ScriptableObject.CreateInstance<Tile>();
        floorPlankTile.sprite = floorPlankSprite;

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

        pathTileRed = ScriptableObject.CreateInstance<Tile>();
        pathTileRed.sprite = pathSpriteRed;

        pathTileBlue = ScriptableObject.CreateInstance<Tile>();
        pathTileBlue.sprite = pathSpriteBlue;

        pathTileFull = ScriptableObject.CreateInstance<Tile>();
        pathTileFull.sprite = pathSpriteFull;

        debugTile = ScriptableObject.CreateInstance<Tile>();
        debugTile.sprite = debugSprite;

        ruinTile = ScriptableObject.CreateInstance<Tile>();
        ruinTile.sprite = ruinSprite;

        boundryTile = ScriptableObject.CreateInstance<Tile>();
        boundryTile.sprite = boundrySprite;

    }

    private void TileSwitch(Vector2Int pos)
    {
        switch (gridCells[pos.x][pos.y].cellstate)
        {
            case CellState.available:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                break;

            case CellState.darkness:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), darknessTile);
                break;

            case CellState.floor:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), floorTile);
                break;

            case CellState.floorPlank:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), floorPlankTile);
                break;

            case CellState.entrance:

                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), entranceTile);
                break;

            case CellState.wall:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), wallTile);
                break;

            case CellState.ruins:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), ruinTile);
                break;

            case CellState.path:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), pathTile);
                break;

            case CellState.boundry:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), boundryTile);
                break;

            case CellState.debug:
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), debugTile);
                break;

            default:
                Debug.LogWarning("No cell state?");
                break;
        }
    }

    //Put here to block movement/pathfinding etc
    public bool CellStateBlocking(GridCell cell)
    {
        //Put something here to block pathfinding
        bool block = false;


        block = IsCellOccupied(cell);


        switch (cell.cellstate)
        {
            case Grid.CellState.wall:
                block = true;
                break;
            case Grid.CellState.boundry:
                block = true;
                break;

            default:
                break;
        }
        return block;
    }

    private void Update()
    {
        if (debugChangeTile == true)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int pos = UtilsHart.ToInt2(Camera.main.ScreenToWorldPoint(Input.mousePosition));
               
                if (GridCellsRangeCheck(pos) == true)
                {
                    if (destructableDebugCell)
                    {
                        GridCellDestructable newCell = new GridCellDestructable(pos, debDestructableCell.cellstate, this, debDestructableCell.TotalHealthPoints, debDestructableCell.HealthPoints, debDestructableCell.stateAfterDestruction);

                        SetTile(newCell);
                    }
                    else
                    {
                        GridCell newCell = new GridCell(pos, debCell.cellstate, this);


                        SetTile(newCell);
                    }
                }
            }
            
        }
    }
    public bool TurnMapEdition()
    {
        return debugChangeTile = !debugChangeTile;
    }

    public void ClearPath()
    {
        pathTiles.ClearAllTiles();
        pathTilesDown.ClearAllTiles();
    }

    public void ClearPath(bool up)
    {
        if(up)pathTiles.ClearAllTiles();
        else pathTilesDown.ClearAllTiles();
    }

    public void DrawPath(List<GridCell> path, int distance, bool up)
    {
        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (distance > 0)
                    tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), pathTile);
                else tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), pathTileRed);
                distance--;
            }
        }
    }

    public void DrawPath(List<GridCell> path, bool up)
    {
        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                    tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), pathTile);
            }
        }
    }


    public void DrawPath(List<GridCell> path, Tile tile, bool up)
    {
        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if(path[i] != null)
                tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), tile);
            }
        }
    }

    public void DrawPath(List<GridCell> path, int distance, Tile tile1, Tile tile2, bool up)
    {
        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (distance > 0)
                    tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), tile1);
                else tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), tile2);
                distance--;
            }
        }
    }

    public void DrawPath(List<GridCell> path, int distance, bool onlyDistance, bool up)
    {

        if(onlyDistance == false)
        {
            DrawPath(path, distance,up);
            return;
        }

        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);

        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (distance > 0)
                    tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), pathTile);
                distance--;
            }
        }
    }


    public void DrawPath(List<GridCell> path, int distance, bool onlyDistance, Tile tile, bool up)
    {
        if (onlyDistance == false)
        {
            DrawPath(path, distance, up );
            return;
        }

        Tilemap tiles;
        if (up) tiles = pathTiles; else tiles = pathTilesDown;

        ClearPath(up);
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (distance > 0)
                    tiles.SetTile(new Vector3Int(path[i].gridPos.x, path[i].gridPos.y, 0), tile);
                distance--;
            }
        }
    }

    public void SetTiles()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                TileSwitch(new Vector2Int(x, y));

            }

        }
    }

    public void SetTile(GridCell gridCell)
    {
            gridCells[gridCell.gridPos.x][gridCell.gridPos.y] = gridCell;
        TileSwitch(gridCell.gridPos);
        
    }
    public void SetTile(GridCellDestructable gridCell)
    {
        gridCells[gridCell.gridPos.x][gridCell.gridPos.y] = gridCell;
        TileSwitch(gridCell.gridPos);
    }

    public void SetTile(Vector2Int pos, CellState cellState)
    {
        gridCells[pos.x][pos.y].cellstate = cellState;
        TileSwitch(pos);

    }
    public void SetTile(Vector2Int pos, CellState cellState, GridCell gridCell)
    {
        gridCells[pos.x][pos.y] = gridCell;
        gridCells[pos.x][pos.y].cellstate = cellState;
        TileSwitch(pos);

    }

    public void SetTile(Vector2Int pos, CellState cellState, CellState stateAfterDestruction)
    {
        GridCellDestructable newCell = new GridCellDestructable(gridCells[pos.x][pos.y].gridPos, gridCells[pos.x][pos.y].cellstate, this, 1, 1, stateAfterDestruction);
        gridCells[pos.x][pos.y] = newCell;
        gridCells[pos.x][pos.y].cellstate = cellState;
        TileSwitch(pos);

    }

    public void SetTile(Vector2Int pos, CellState cellState, bool destructable, int totalHealthPoints, int healthPoints, CellState stateAfterDestruction)
    {
        if (destructable)
        {
            GridCellDestructable newCell = new GridCellDestructable(gridCells[pos.x][pos.y].gridPos, gridCells[pos.x][pos.y].cellstate, this, totalHealthPoints, healthPoints, stateAfterDestruction);
            gridCells[pos.x][pos.y] = newCell;
            gridCells[pos.x][pos.y].cellstate = cellState;
            TileSwitch(pos);
        }
        else
        {
            SetTile(pos, cellState);
        }
    }


    public void SetTile(Vector2Int pos)
    {
        TileSwitch(pos);    
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
        if (GridCellsRangeCheck(cellPos))
        {
            return gridCells[cellPos.x][cellPos.y].cellstate;
        }
        else return CellState.noCell;
    }

    
    public bool GridCellsRangeCheck(Vector2Int index)
    {
        if ((index.x >= 0 && index.x < gridSize.x) && (index.y >= 0 && index.y < gridSize.y))
            return true;
        else return false;
    }
    public bool GridCellsRangeCheck(Vector2 pos)
    {
        if ((pos.x >= 0 && pos.x < gridSize.x) && (pos.y >= 0 && pos.y < gridSize.y))
            return true;
        else return false;
    }

    public bool GridCellsRangeCheck(Vector3 pos)
    {
        if ((pos.x >= 0 && pos.x < gridSize.x) && (pos.y >= 0 && pos.y < gridSize.y))
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
        if (GridCellsRangeCheck(pos))
            return gridCells[pos.x][pos.y];
        else return null;
    }
    public GridCell GetCellByPos(Vector2 pos)
    {
        if (GridCellsRangeCheck(pos))
            return gridCells[Mathf.FloorToInt(pos.x)][Mathf.FloorToInt(pos.y)];
        else return null;
    }
        public List<GridCell> GetCellsByPos(List<Vector2Int> pos)
    {
        List<GridCell> cells = new List<GridCell>();
        for (int i = 0; i < pos.Count; i++)
        {
            cells.Add(GetCellByPos(pos[i]));
        }
        return cells;
    }

    public List<GridCell> GetNeigbours(GridCell cell)
    {
        List<GridCell> neigbours = new List<GridCell>();
        for (int x = -1; x<= 1 ; x++)
        {
            for (int y = -1; y <= 1;y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int checkXY = new Vector2Int();
                checkXY.x = cell.gridPos.x + x;
                checkXY.y = cell.gridPos.y +y;

                if (checkXY.x >= 0 && checkXY.y >= 0 && checkXY.x < gridSize.x && checkXY.y < gridSize.y)
                {
                    neigbours.Add(gridCells[checkXY.x][checkXY.y]);
                }
            }
        }
        return neigbours;
    }

    public List<GridCell> ReturnCellsWithBlocking(List<GridCell> cells)
    {
        List<GridCell> newCells = new List<GridCell>();

        if (newCells != null)
            for (int i = 0; i < cells.Count; i++)
            {
                if (CellStateBlocking(cells[i]) == true || cells[i].HasCellObject() == true)
                {

                }
                else newCells.Add(cells[i]);


            }

        return newCells;
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

    public void ResetOccupiedCells()
    {
        for (int i = occupiedCells.Count - 1; i >= 0; i--)
        {
            if (occupiedCells[i].CheckCellObject() == false) occupiedCells.RemoveAt(i);
        }
    }

    public void OccupyCell(GridCell cell ,GameObject _object)
    {
        if (cell != null)
        {
            cell.occupyingObject = _object;
            if (!occupiedCells.Contains(cell))
            {
                occupiedCells.Add(cell);
            }
        }
    }

    public bool IsCellOccupied(GridCell cell)
    {
        if (cell.occupyingObject != null) return true;
        else return false;
    }
    public void RemoveOccupiedObject(GameObject _object)
    {
        GridCell cell = GetCellByPos(_object.transform.position);
        if (cell != null)
        {
            cell.RemoveOccupiyingObject();
            occupiedCells.Remove(cell);
        }
        else return;
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

[System.Serializable]
public class GridCell
{
    public Grid grid;
    public Vector2Int gridPos;
    public Grid.CellState cellstate;
    public GameObject occupyingObject;

    public List<Item> items = new List<Item>();
    
    //PathFindingStuff:
    [HideInInspector] public GridCell parent;
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    } 

    public void RemoveOccupiyingObject()
    {
        occupyingObject = null;
    }

    public GameObject GetOccupiyingObject()
    {
        //Debug.Log(occupyingObject);
        return occupyingObject;
    }

    public bool CheckCellObject()
    {
        Collider2D hit = Physics2D.OverlapCircle(new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f), 0.5f, 1 << LayerMask.NameToLayer("Blocked"));
        if (hit)
        {
            occupyingObject = hit.gameObject;
            return true;
        }
        else
        {
            occupyingObject = null;
            return false;
        }
    }

    public bool HasCellObject()
    {
        if (occupyingObject == null) return false;
        else return true;
    }

    public GridCell(Vector2Int gridPos, Grid.CellState cellState, Grid grid)
    {
        this.gridPos = gridPos;
        this.cellstate = cellState;
        this.grid = grid;
    }

}

[System.Serializable]
public class GridCellDestructable : GridCell, IDamagable
{
    public GridCellDestructable(Vector2Int gridPos, Grid.CellState cellState, Grid grid, float totalHealthPoints, float healthPoints, Grid.CellState stateAfterDestruction) : base(gridPos, cellState, grid)
    {
        healthSystem = new HealthSystem(totalHealthPoints, healthPoints);
        this.stateAfterDestruction = stateAfterDestruction;
    }

    [SerializeField] private HealthSystem healthSystem;
    public float TotalHealthPoints { get => healthSystem.TotalHealthPoints; set => healthSystem.TotalHealthPoints = value; }
    public float HealthPoints { get => healthSystem.HealthPoints; set => healthSystem.HealthPoints = value; }
    
    public Grid.CellState stateAfterDestruction;

    public void Damage(float damage)
    {
        if (healthSystem.Destructable == false)
        {
            Debug.Log("Indestructable");
            return;
        }

        HealthPoints = HealthPoints - damage;

        if (HealthPoints <= 0) { Kill(); }
    }

    public void Kill()
    {
        if (healthSystem.Destructable == false)
        {
            Debug.Log("Indestructable");
            return;
        }

        GridCell cell = new GridCell(this.gridPos,stateAfterDestruction,this.grid);
     
        grid.SetTile(gridPos, stateAfterDestruction, cell);
        if (healthSystem.DeathParticle != null) { MonoBehaviour.Instantiate(healthSystem.DeathParticle, new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0), healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Destroyed Tile");
    }
    public void ForceKill()
    {
        GridCell cell = new GridCell(this.gridPos, stateAfterDestruction, this.grid);

        grid.SetTile(gridPos, stateAfterDestruction, cell);
        if (healthSystem.DeathParticle != null) { MonoBehaviour.Instantiate(healthSystem.DeathParticle, new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0), healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Destroyed Tile");
    }
}