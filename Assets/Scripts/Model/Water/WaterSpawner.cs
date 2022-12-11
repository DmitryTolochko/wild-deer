using System.Collections;
using UnityEngine;
using System.Linq;


public class WaterSpawner : MonoBehaviour
{
    private Collider2D spawnField;
    private const int MaxWaterItemsCount = 1;
    public static bool IsWaiting;
    private int count;

    private void Start()
    {
        spawnField = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "WaterField")
            ?.GetComponent<Collider2D>();


        WaterWorld.WaterCollected += waterWorld =>
        {
            PoolManager.Instance.CoolObject(waterWorld.gameObject, PoolObjectType.Water);
            count--;
        };
    }

    private void Update()
    {
        if (!IsWaiting && count < MaxWaterItemsCount)
        {
            StartCoroutine(Wait(Random.Range(60, 120)));
            CreateNewWaterItem(PoolObjectType.Water);
        }
    }

    private void GenerateNewPosition(GameObject otherObject)
    {
        var point = new Vector3(Random.Range(5.5f, 10f), Random.Range(-2.4f, 0), 0);
        while (Physics2D.OverlapCircle(point, 0f) != spawnField)
        {
            point = new Vector3(Random.Range(5.5f, 10f), Random.Range(-2.4f, 0), 0);
        }

        otherObject.transform.position = point;
    }

    private IEnumerator Wait(int time)
    {
        IsWaiting = true;
        yield return new WaitForSecondsRealtime(time);
        IsWaiting = false;
    }

    private void CreateNewWaterItem(PoolObjectType type)
    {
        var waterItem = PoolManager.Instance.GetPoolObject(type);
        waterItem.gameObject.SetActive(true);
        GenerateNewPosition(waterItem);
        count++;
    }
}