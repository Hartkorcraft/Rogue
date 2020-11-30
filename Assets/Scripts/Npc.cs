﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : DynamicObject, ITurn, IDamagable
{

    [SerializeField] protected int totalMovePoints = 3;
    [SerializeField] protected int movePoints = 1;
    [SerializeField] protected float speed = 40f;

    protected PathFinding pathFinding;


    [SerializeField] protected Transform target;
    [SerializeField] protected bool canMove = true;

    protected override void Awake()
    {
        base.Awake();
        pathFinding = grid.GetComponent<PathFinding>();

    }

    protected override void Start()
    {
        base.Start();
        gameManager.AddITurn(this,this.gameObject);
        //transform.position = new Vector3(grid.GetPosTransform(transform).x + 0.5f, grid.GetPosTransform(transform).y + 0.5f, transform.position.z);

    }

    public virtual bool Turn()
    {
        gameManager.ResetOccupyingGameobjects();
        movePoints = totalMovePoints;

        if (target != null)
        {
            List<GridCell> path = new List<GridCell>();
            if (movePoints > 0) path = pathFinding.FindPath(transform.position, target.position);
            int pathCellNum = 0;
            while (movePoints > 0)
            {
                if (path != null && path.Count > 0 && pathCellNum < path.Count && canMove)
                {
                    movePoints--;
                    movement2D.MoveTo(path[pathCellNum].gridPos, transform, grid);
                    //movement2D.MoveTo(path, this, movePoints, speed, grid);

                    if (UtilsHart.ToInt2(new Vector2(transform.position.x, transform.position.y)) == UtilsHart.ToInt2(new Vector2(target.position.x, target.position.y))) break;
                    pathCellNum++;
                }
                else
                {
                    movePoints = 0;
                    break;
                }
            }
        }

        return true;
    }



    /* 
    public override void  Damage(float damage)
    {
        if (healthSystem.Destructable == false)
        {
            Debug.Log("Indestructable");
            return;
        }

        HealthPoints =  HealthPoints - damage;
        if (HealthPoints <= 0) Kill(); 
    }    

    public override void Kill()
    {
        if (healthSystem.Destructable == false)
        {
            Debug.Log("Indestructable");
            return;
        }

        gameManager.DestroyDynamicObject(this.gameObject);
        if(healthSystem.DeathParticle != null) { Instantiate(healthSystem.DeathParticle, transform.position, healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Killed");
    }
    public override void ForceKill()
    {
        gameManager.DestroyDynamicObject(this.gameObject);
        if (healthSystem.DeathParticle != null) { Instantiate(healthSystem.DeathParticle, transform.position, healthSystem.DeathParticle.transform.rotation); }
        Debug.Log("Killed");
    }
    */



}
