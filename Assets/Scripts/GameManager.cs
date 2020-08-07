using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private int turnNum = 0;
    private List<ITurn> turnObjects = new List<ITurn>(); 

    
    

    public void NextTurn()
    {
        turnNum++;

        for (int i = 0; i < turnObjects.Count; i++)
        {
            turnObjects[i].Turn();
        }


    }

    public void AddITurn(ITurn interfaceComponent)
    {
        Debug.Log(interfaceComponent);
        turnObjects.Add(interfaceComponent);
    }


}
