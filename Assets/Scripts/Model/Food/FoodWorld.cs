using System;
using System.Collections;
using Model.Boosters;
using Unity.VisualScripting;
using UnityEngine;

namespace Model.Food
{
    public class FoodWorld : MonoBehaviour
    {
        public bool isCollected = false;
        public bool isCollectable = false;

        private Rigidbody2D rb;
        public static event Action FoodCollected;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // private void Update()
        // {
        //     StartCoroutine(Grow());
        // }

        private void OnMouseDown()
        {
            isCollected = true;
            Inventory.Inventory.AddItem(new FoodBooster());
            FoodCollected?.Invoke();
        }

        // private IEnumerator Grow()
        // {
        //     yield return new WaitForSecondsRealtime(3);
        //     isCollectable = true;
        // }

        public void ResetItem()
        {
            isCollected = false;
            isCollectable = false;
        }
    }
}