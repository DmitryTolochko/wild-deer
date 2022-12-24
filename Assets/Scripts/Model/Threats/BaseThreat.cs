using System;
using System.Collections;
using System.Collections.Generic;
using ServiceInstances;
using UnityEngine;
using System.Linq;
using Model.Boosters;
using Unity.VisualScripting;

public abstract class BaseThreat : MonoBehaviour, IThreat
{
    public virtual ThreatType Type { get; set; }
    public virtual HashSet<BoosterType> BoosterTypes => new();
    public virtual int StressTime { get; set; }
    public virtual float StressLevel { get; set; }
    public float DistanceToTarget;

    public ThreatStatus Status
    {
        get => status;
        set
        {
            status = value;
            if (status == ThreatStatus.Defeated)
            {
                ThreatDefeated?.Invoke();
            }
        }
    }

    public virtual GameObject TargetDeer { get; set; }
    public virtual Vector2 SpawnPoint { get; set; }

    private bool onGameField;
    private ThreatStatus status;
    public static event Action ThreatDefeated;

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Booster(Clone)"
            && BoosterTypes.Contains(other.gameObject.GetComponent<IBooster>().Type))
            Status = ThreatStatus.Defeated;
    }

    public virtual void PlaceThreat()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Act()
    {
        throw new System.NotImplementedException();
    }

    public virtual void GetBoosterTypes()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 target, float moveSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    public void FindNewTargetDeer()
    {
        TargetDeer = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(y => y.activeInHierarchy)
            .FirstOrDefault(x => x.name == "Deer(Clone)"
                                 && !x.GetComponent<Deer>().IsAVictim);
    }

    public virtual void ActAfterWin()
    {
        throw new System.NotImplementedException();
    }

    public virtual IEnumerator AddStress()
    {
        //GameModel.StressLevel += StressLevel;
        GameModel.ChangeStressAsync(StressLevel);
        yield return new WaitForSecondsRealtime(StressTime);
        GameModel.ChangeStressAsync(-StressLevel);
        //GameModel.StressLevel -= StressLevel;
    }

    public virtual void CheckIntersectionOnGameField()
    {
        if (GameModel.GameField.bounds.Contains(transform.position)
            && !onGameField)
        {
            onGameField = true;
            StartCoroutine(AddStress());
        }
    }

    public GameObject TargetPoint;

    public virtual void Start()
    {
        onGameField = false;
        Status = ThreatStatus.Spawning;
        GetBoosterTypes();
        FindNewTargetDeer();
        SpawnPoint = ThreatSpawner.GenerateNewPosition();
        TargetPoint.transform.position = SpawnPoint;
        transform.position = new Vector2(SpawnPoint.x, SpawnPoint.y + 11);
    }

    private void Update()
    {
        DistanceToTarget = Vector2.Distance(transform.position, TargetDeer.transform.position);

        if (Status == ThreatStatus.Spawning)
            PlaceThreat();
        else if (Status == ThreatStatus.Spawned && DistanceToTarget > 0.65f)
            Move(TargetDeer.transform.position, 0.65f);
        else if (Status == ThreatStatus.Won)
            ActAfterWin();
        else if (Status == ThreatStatus.Spawned && DistanceToTarget <= 0.65f)
            Act();
        CheckIntersectionOnGameField();
    }

    private void OnBecameInvisible()
    {
        Status = ThreatStatus.Won;
    }
}