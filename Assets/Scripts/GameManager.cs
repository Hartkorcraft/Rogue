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

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    public void NextTurn()
    {
        ResetPositions();

        turnNum++;
        for (int i = 0; i < turnObjects.Count; i++)
        {
            turnObjects[i].Turn();
        }


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
}
