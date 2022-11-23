using System.Collections;
using System.Linq;
using UnityEngine;

namespace Model.Food
{
    public class FoodSpawner : MonoBehaviour
    {
        private PolygonCollider2D gameField; 
        private Collider2D foodField; 
        private int foodItemsCount = 0;
        private const int MaxFoodItemsCount = 5;

        private bool isWaiting = false;
        private void Start()
        {
            gameField = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(x => x.name == "GameField")
                ?.GetComponent<PolygonCollider2D>();

            foodField = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(x => x.name == "FoodField")
                ?.GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (!isWaiting && foodItemsCount < MaxFoodItemsCount)
            {
                StartCoroutine(Wait(Random.Range(3, 10)));
                StartCoroutine(GenerateRoutine(PoolObjectType.Food));
            }
        }

        private void GenerateNewPosition(GameObject otherObject)
        {
            while (!(foodField.bounds.Intersects(otherObject.GetComponent<Collider2D>().bounds) && 
                     !gameField.bounds.Intersects(otherObject.GetComponent<Collider2D>().bounds)))
            {
                otherObject.transform.position = new Vector3(UnityEngine.Random.value * 20-10, UnityEngine.Random.value * 10-5, 0);
            }
        }

        private IEnumerator Wait(int time)
        {
            isWaiting = true;
            yield return new WaitForSecondsRealtime(time);
            isWaiting = false;
        }

        private IEnumerator GenerateRoutine(PoolObjectType type)
        {
            GameObject item = PoolManager.Instance.GetPoolObject(type);
            GenerateNewPosition(item);
            item.gameObject.SetActive(true);
            foodItemsCount += 1;
            var foodItem = item.GetComponent<FoodWorld>();
            while (!foodItem.isCollected) 
                yield return new WaitForSecondsRealtime(0);
            foodItemsCount -= 1;
            item.gameObject.GetComponent<FoodWorld>().ResetItem();
            PoolManager.Instance.CoolObject(item, type);
        }
    }
}
