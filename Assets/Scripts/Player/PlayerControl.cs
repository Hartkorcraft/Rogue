using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Grid grid;

    [SerializeField] private bool canMove = true;
    private bool moved = false;

    private Movement2D movement2D = new Movement2D();
    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

    }

    void Start()
    {
        transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f , grid.GetPosTransform(transform).y + 0.5f , transform.position.z) ;
    }

    // Update is called once per frame  
    void Update()
    {

        if (Input.GetAxisRaw("Horizontal") != 0 || (Input.GetAxisRaw("Vertical") != 0))
        {
            if (moved == false)
            {

                movement2D.MoveBy(transform.position, new Vector2(transform.position.x + GetMovementDirection().x, transform.position.y + GetMovementDirection().y),transform);

                moved = true;
            }
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && (Input.GetAxisRaw("Vertical") == 0))
        {
            moved = false;
        }
    }

    private Vector2Int GetMovementDirection()
    {
        
        Vector2Int moveDir = new Vector2Int();
        moveDir.x = (int)(Input.GetAxisRaw("Horizontal"));
        moveDir.y = (int)(Input.GetAxisRaw("Vertical"));
        return moveDir;
    }



}
