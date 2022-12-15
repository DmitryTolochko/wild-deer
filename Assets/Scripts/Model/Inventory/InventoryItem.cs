using System.Linq;
using Model.Boosters;
using ServiceInstances;
using UnityEngine;

public class InventoryItem
{
    public BoosterType Type { get; }
    public int Amount { get; set; }

    public InventoryItem(BoosterType type)
    {
        Type = type;
        Amount = 1;
    }

    public InventoryItem(BoosterType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}