using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceInstances;

public interface IThreat
{
    public ThreatType Type {get; }
    public HashSet<BoosterType> BoosterTypes {get; }
    public int ActionTime {get; set;}
    public int DamagePower {get; set;}
    public int MaxStressLevel {get; set;}
    public ThreatStatus Status {get; set;}

    public void Move();

    public void OnCollisionEnter2D(Collision2D other);
    
}
