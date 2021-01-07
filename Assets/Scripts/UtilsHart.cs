using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsHart
{

    static public Vector2Int GetSize(Vector2Int pos1, Vector2Int pos2)
    {
        Vector2Int size = new Vector2Int();

        size.x = Mathf.Abs(pos1.x - pos2.x);
        size.y = Mathf.Abs(pos2.y - pos2.y);
        return size;
    }

    static public Vector2Int GetPosDistanceInt(Vector2Int pos1, Vector2Int pos2)
    {

        Vector2Int distance = new Vector2Int(pos2.x - pos1.x, pos2.y - pos1.y);
        return distance;

    }

    static public Vector2 GetPosDistance(Vector2Int pos1, Vector2Int pos2)
    {

        Vector2 distance = new Vector2(pos2.x - pos1.x, pos2.y - pos1.y);
        return distance;

    }
    static public float GetDistance(Vector2Int pos1, Vector2Int pos2)
    {

        float distance = Mathf.Sqrt((pos2.x - pos1.x) * (pos2.x - pos1.x) + (pos2.y - pos1.y) * (pos2.y - pos1.y));
        return distance;

    }

    static public Vector2Int ToInt2(Vector2 v)
    {
        return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
    }

    static public Grid.Direction GetDir(Vector2Int pos1, Vector2Int pos2)
    {
       if(Mathf.Abs(pos1.x-pos2.x) < Mathf.Abs(pos1.y - pos2.y))
        {
            if (pos1.y < pos2.y) return Grid.Direction.up;
            else return Grid.Direction.down;
        }
        else
        {
            if (pos1.x < pos2.x) return Grid.Direction.right;
            else return Grid.Direction.left;
        }
    }

    static public Vector2Int ToInt2(Vector3 v)
    {
        return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
    }
    
    static public bool isNumeric(string s)
    {
        return int.TryParse(s, out int n);
    }
}
