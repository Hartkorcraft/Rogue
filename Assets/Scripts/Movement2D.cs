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
    public void MoveBy(Vector2 curPos, Vector2 desPos, Transform transform)
    {
        Debug.Log(curPos);
        Debug.Log(desPos);

        bool shouldLerp = true;
        while (shouldLerp)
        {
            transform.position = Vector2.Lerp(curPos, desPos, time * Time.deltaTime);
            if(Mathf.Abs(transform.position.x-desPos.x) <= 0.1f && Mathf.Abs(transform.position.y - desPos.y) <= 0.1f)
            {
                transform.position = desPos;
                shouldLerp = false;
                break;
            }
        }
    }

}
