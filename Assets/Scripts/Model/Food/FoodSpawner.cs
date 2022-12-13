using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour
{
    private Collider2D spawnField_1;
    private Collider2D spawnField_2;
    private const int MaxFoodItemsCount = 5;
    public static bool IsWaiting = false;

    private void Start()
    {
        spawnField_1 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 1")
            ?.GetComponent<Collider2D>();

        spawnField_2 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 2")
            ?.GetComponent<Collider2D>();

        FoodWorld.FoodCollected += foodWorld =>
        {
            var foodWorldGameObject = foodWorld.gameObject;
            GameModel.FoodSpawned.Remove(foodWorldGameObject);
            PoolManager.Instance.CoolObject(foodWorldGameObject, PoolObjectType.Food);
        };
    }

    private void Update()
    {
        if (!IsWaiting && GameModel.FoodSpawned.Count < MaxFoodItemsCount)
        {
            StartCoroutine(Wait(Random.Range(60, 120)));
            CreateNewFoodItem(PoolObjectType.Food);
        }
    }

    private IEnumerator Wait(int time)
    {
        IsWaiting = true;
        yield return new WaitForSecondsRealtime(time);
        IsWaiting = false;
    }


    private void GenerateNewPosition(GameObject otherObject)
    {
        var point = new Vector3(Random.Range(-10.0f, 15.0f), Random.Range(-5.0f, 5.0f), 0);
        while (Physics2D.OverlapCircle(point, 0f) != spawnField_1
               && Physics2D.OverlapCircle(point, 0f) != spawnField_2)
        {
            point = new Vector3(Random.Range(-10.0f, 15.0f), Random.Range(-5.0f, 5.0f), 0);
        }

        otherObject.transform.position = point;
    }

    private void CreateNewFoodItem(PoolObjectType type)
    {
        var foodItem = PoolManager.Instance.GetPoolObject(type);
        foodItem.gameObject.SetActive(true);
        GenerateNewPosition(foodItem);
        GameModel.FoodSpawned.Add(foodItem);
    }
}