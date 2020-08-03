using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{

    private float time = 0.5f;
    private IEnumerator coroutine;



    public void MoveTo(Vector2Int pos, Transform transform)
    {
        transform.position = Vector2.Lerp(pos, transform.position, Time.deltaTime * time);
    }
    public void MoveBy(Vector2 curPos, Vector2 dir, Transform transform)
    {

        Vector2 newPos = new Vector2(curPos.x + dir.x, curPos.y + dir.y);
        transform.position = newPos;



    }

}
