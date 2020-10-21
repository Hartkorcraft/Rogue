using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : Npc
{


    public override bool Turn()
    {
        Debug.Log("forg forg");
        base.Turn();

        return true;
    }




}
