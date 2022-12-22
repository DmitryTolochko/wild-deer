using System.Linq;
using ServiceInstances;
using UnityEngine;

public class KillerPoacher : BaseThreat
{
    public override void Start()
    {
        base.Start();

        Type = ThreatType.KillerPoacher;
        StressTime = 60;
        StressLevel = 0.5f;
    }

    public override void PlaceThreat()
    {
        var distanceToTarget = Vector2.Distance(transform.position, SpawnPoint);
        if (distanceToTarget > 0.65f)
        {
            Move(SpawnPoint, 3);
            TargetPoint.transform.position = SpawnPoint;
        }
        else
        {
            Status = ThreatStatus.Spawned;
            GameModel.CurrentThreat = gameObject;
        }
    }

    public override void GetBoosterTypes()
    {
        int[] numbers = {0, 1, 2, 3, 4, 5, 6};
        foreach (var i in numbers)
            BoosterTypes.Add((BoosterType) i);
    }

    public override void Act()
    {
        TargetDeer.GetComponent<Deer>().CurrentAge = Age.Dead;
        Status = ThreatStatus.Won;
    }

    public override void ActAfterWin()
    {
        var distanceToTarget = Vector2.Distance(transform.position, SpawnPoint);
        if (distanceToTarget > 0.5f)
            Move(SpawnPoint, 2);
        else
            Status = ThreatStatus.Defeated;
    }
}