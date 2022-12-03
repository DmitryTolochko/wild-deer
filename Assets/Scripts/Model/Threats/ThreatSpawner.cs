using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreatSpawner : MonoBehaviour
{
    public static HashSet<GameObject> threats = new HashSet<GameObject>();

    private static Collider2D spawnField_2; 
    private static Collider2D spawnField_1; 
    private bool CanArouseThreat = true;
    private void Start()
    {
        spawnField_2 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 2")
            ?.GetComponent<Collider2D>();

        spawnField_1 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 1")
            ?.GetComponent<Collider2D>();
    }

    private void Update() 
    {
        if (DeerSpawner.DeerCount > 1 && threats.Count < 1 && CanArouseThreat)
        {
            StartCoroutine(ArouseRandomThreat());                       
        }
    }

    private IEnumerator ArouseRandomThreat()
    {
        var num = Random.Range(1, 3);

        switch(num)
        {
            case 1:
                StartCoroutine(CreateThreat(PoolObjectType.ArcticFox));
                print("1");
                break;
            case 2:
                StartCoroutine(CreateThreat(PoolObjectType.KillerPoacher));
                print("2");
                break;
        } 

        CanArouseThreat = false;
        yield return new WaitForSecondsRealtime(Random.Range(30, 40));
        CanArouseThreat = true;
    }

    private IEnumerator CreateThreat(PoolObjectType type)
    {
        var threat = PoolManager.Instance.GetPoolObject(type);
        threat.gameObject.SetActive(true);
        threats.Add(threat);
        InitializeThreat(type, threat);

        while (threat.GetComponent<BaseThreat>().Status != ThreatStatus.Defeated) 
            yield return new WaitForSecondsRealtime(0);

        PoolManager.Instance.CoolObject(threat, type);
        threats.Remove(threat);
        print(threats.Count);
    }

    public static Vector2 GenerateNewPosition()
    {
        var point = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));
        while (Physics2D.OverlapCircle(point, 0f) != spawnField_1
            && Physics2D.OverlapCircle(point, 0f) != spawnField_2)
            point = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));
        return point;
    }

    private void InitializeThreat(PoolObjectType type, GameObject threat)
    {
        switch (type)
        {
            case PoolObjectType.KillerPoacher:
                threat.GetComponent<KillerPoacher>().Start();
                break;
            case PoolObjectType.ArcticFox:
                threat.GetComponent<ArcticFox>().Start();
                break;
        }
    }
}
