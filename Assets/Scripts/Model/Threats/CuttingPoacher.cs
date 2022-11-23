using System.Collections;
using System.Collections.Generic;
using ServiceInstances;
using UnityEngine;
using System.Linq;

public class CuttingPoacher : MonoBehaviour, IThreat
{
    public ThreatType Type => ThreatType.CuttingPoacher;
    public HashSet<BoosterType> BoosterTypes => new HashSet<BoosterType>();
    public int ActionTime {get; set;}
    public int DamagePower {get; set;}
    public int MaxStressLevel {get; set;}
    public ThreatStatus Status {get; set;}

    private GameObject TargetDeer;
    private float speed;

    private void Start()
    {
        speed = 2.5f;
        Status = ThreatStatus.Spawned;
        MaxStressLevel = 10;
        DamagePower = 100;
        TargetDeer = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "Deer(Clone)");
        
        int[] numbers = {0, 1, 2, 3, 4, 5, 6};
        foreach(var i in numbers)
            BoosterTypes.Add((BoosterType)i);        
    }

    private void Update()
    {
        var distanceToTarget = Vector3.Distance(transform.position, TargetDeer.transform.position);
        if (distanceToTarget <= 0.5f)
        {
            TargetDeer.GetComponent<Deer>().IsWaiting = true;
            TargetDeer.GetComponent<Deer>().CurrentAge = Age.Dead;
            Status = ThreatStatus.Won;
        }
        else
            Move();
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetDeer.transform.position, speed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.name == "Booster(Clone)" 
        && BoosterTypes.Contains(other.gameObject.GetComponent<BoosterWorld>().type))
        {
            Status = ThreatStatus.Defeated;
        }*/
    }
}
