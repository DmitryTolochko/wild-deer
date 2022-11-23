﻿using System.Linq;
using Model.Boosters;
using ServiceInstances;
using UnityEngine;
using UnityEngine.WSA;

namespace Model.Inventory
{
    public class Item
    {
        public BoosterType Type { get; }
        public int Amount { get; set; }

        public Item(BoosterType type)
        {
            Type = type;
            Amount = 1;
        }

        public Item(BoosterType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}