using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthSystem
{
    [SerializeField] private float totalHealthPoints = 1;
    [SerializeField] private float healthPoints = 1;

    public float TotalHealthPoints { get => totalHealthPoints; set => totalHealthPoints = value; }
    public float HealthPoints {get => healthPoints; set => healthPoints = value;}



    public HealthSystem(int totalHealthPoints, int healthPoints)
    {
        this.healthPoints = healthPoints;
        this.totalHealthPoints = totalHealthPoints;
    }
}
