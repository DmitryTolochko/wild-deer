using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceInstances;

public interface IThreat
{
    public ThreatType Type {get; set;}
    public HashSet<BoosterType> BoosterTypes {get; }
    public int StressTime {get; set;}
    public float StressLevel {get; set;}
    public ThreatStatus Status {get; set;}

    public GameObject TargetDeer {get; set;}
    public Vector3 SpawnPoint {get; set;}

    public void Move(Vector3 target, float moveSpeed);

    public void OnCollisionEnter2D(Collision2D other);

    public void PlaceThreat();

    public void Act();
}
