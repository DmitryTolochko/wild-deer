using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ServiceInstances;
using UnityEngine;

public class Wolverine : BaseThreat
{
    public override void Start() 
    {
        base.Start();
        
        Type = ThreatType.Wolverine;
        StressTime = 60;
        StressLevel = 50;
    }
}
