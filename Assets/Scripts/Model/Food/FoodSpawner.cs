using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Model.Food;

public class FoodSpawner : MonoBehaviour
{
    private Collider2D spawnField_1; 
    private Collider2D spawnField_2; 
    private const int MaxFoodItemsCount = 5;
    public static bool IsWaiting = true;

    private void Start()
    {
        spawnField_1 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 1")
            ?.GetComponent<Collider2D>();

        spawnField_2 = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "SpawnField 2")
            ?.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!IsWaiting && GameModel.FoodSpawned.Count < MaxFoodItemsCount)
        {
            // StartCoroutine(Wait(Random.Range(3, 10)));
            StartCoroutine(CreateNewFoodItem(PoolObjectType.Food));
        }
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

    // private IEnumerator Wait(int time)
    // {
    //     IsWaiting = true;
    //     yield return new WaitForSecondsRealtime(time);
    //     IsWaiting = false;
    // }

    private IEnumerator CreateNewFoodItem(PoolObjectType type)
    {
        GameObject foodItem = PoolManager.Instance.GetPoolObject(type);
        foodItem.gameObject.SetActive(true);
        GenerateNewPosition(foodItem);
        GameModel.FoodSpawned.Add(foodItem);

        while (!foodItem.GetComponent<FoodWorld>().isCollected) 
            yield return new WaitForSecondsRealtime(0);

        GameModel.FoodSpawned.Remove(foodItem);
        foodItem.gameObject.GetComponent<FoodWorld>().ResetItem();
        PoolManager.Instance.CoolObject(foodItem, type);
    }
}
