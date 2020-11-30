using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthSystem
{
    [SerializeField] private float totalHealthPoints = 1;
    [SerializeField] private float healthPoints = 1;
    [SerializeField] private bool destructable = true;
    public bool Destructable { get { if (totalHealthPoints < 0) return false; else return destructable; } set => destructable = value; }
    [SerializeField] private GameObject deathParticle = null;
    public GameObject DeathParticle { get => deathParticle; }
    public float TotalHealthPoints { get => totalHealthPoints; set => totalHealthPoints = value; }
    public float HealthPoints {get => healthPoints; set => healthPoints = value;}



    public HealthSystem(int totalHealthPoints, int healthPoints)
    {
        this.healthPoints = healthPoints;
        this.totalHealthPoints = totalHealthPoints;
    }
}
