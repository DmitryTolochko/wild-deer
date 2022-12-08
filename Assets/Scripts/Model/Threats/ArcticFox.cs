using UnityEngine;
using ServiceInstances;

public class ArcticFox : BaseThreat
{
    public override void Start()
    {
        base.Start();

        Type = ThreatType.ArcticFox;
        StressTime = 120;
        StressLevel = 0.25f;
    }

    public override void PlaceThreat()
    {
        var distanceToTarget = Vector2.Distance(transform.position, SpawnPoint);
        if (distanceToTarget > 0.5f)
            Move(SpawnPoint, 3);
        else
        {
            Status = ThreatStatus.Spawned;
            GameModel.Threats.Add(gameObject);
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
        SpawnPoint = ThreatSpawner.GenerateNewPosition();
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