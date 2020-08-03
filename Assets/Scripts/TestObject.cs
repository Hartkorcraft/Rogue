using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{

    //private PathFinding pathFinding;
    private Grid grid;

    [SerializeField]
    private Vector2Int pathTo;


    private void Start()
    {
        //pathFinding = new PathFinding();
            grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        /*
      List<PathFinding.PathNode> pathNodes = new List<PathFinding.PathNode>();

      
      if (Input.GetKeyDown(KeyCode.L))
      {
          pathNodes = pathFinding.CreatePath(new Vector2Int((int)transform.position.x, (int)transform.position.y), pathTo);

          if (pathNodes != null)
          {
              for (int i = 0; i < pathNodes.Count; i++)
              {
                  grid.DrawTile(Grid.CellState.path, pathNodes[i].index);

              }
          }
      }
      */
    }
}
