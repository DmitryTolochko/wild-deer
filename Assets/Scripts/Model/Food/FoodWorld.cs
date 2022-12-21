using System;
using Model.Boosters;
using Model.Inventory;
using UnityEngine;


public class FoodWorld : MonoBehaviour
{
    public static event Action<FoodWorld> FoodCollected;

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        var mouseWorldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(mouseWorldCoords, transform.position) >= 0.4)
        {
            return;
        }

        Inventory.AddItem(new FoodBooster());
        FoodCollected?.Invoke(this);
    }
}