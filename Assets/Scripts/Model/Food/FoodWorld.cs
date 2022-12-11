using System;
using Model.Boosters;
using Model.Inventory;
using UnityEngine;


public class FoodWorld : MonoBehaviour
{
    public static event Action<FoodWorld> FoodCollected;

    private void OnMouseDown()
    {
        Inventory.AddItem(new FoodBooster());
        FoodCollected?.Invoke(this);
    }
}