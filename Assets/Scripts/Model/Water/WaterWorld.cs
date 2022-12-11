using System;
using Model.Boosters;
using Model.Inventory;
using UnityEngine;

public class WaterWorld : MonoBehaviour
{
    public static event Action<WaterWorld> WaterCollected;

    private void OnMouseDown()
    {
        Inventory.AddItem(new WaterBooster());
        WaterCollected?.Invoke(this);
    }
}