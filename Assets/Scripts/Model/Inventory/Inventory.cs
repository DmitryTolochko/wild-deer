using System;
using System.Collections.Generic;
using System.Linq;
using Model.Boosters;
using ServiceInstances;
using UnityEngine.SceneManagement;

namespace Model.Inventory
{
    public static class Inventory
    {
        private static readonly List<InventoryItem> items = new();
        public static IEnumerable<InventoryItem> GetAllItems() => items;
        public static event Action ItemAdded;
        public static event Action ItemRemoved;
        public static event Action<BoosterType> BoosterUsed;

        static Inventory()
        {
            AddItem(new FoodBooster());
            AddItem(new FoodBooster());
            AddItem(new FoodBooster());
            AddItem(new FoodBooster());
            AddItem(new ProtectiveCapBooster());
            AddItem(new ProtectiveCapBooster());
            AddItem(new ProtectiveCapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
        }

        public static void AddItem(BoosterType boosterType)
        {
            var toIncrementAmountItem = items.FirstOrDefault(x => x.Type == boosterType);
            if (toIncrementAmountItem != null)
            {
                toIncrementAmountItem.Amount++;
            }
            else
            {
                var item = new InventoryItem(boosterType);
                items.Add(item);
            }

            if (SceneManager.GetActiveScene().name != "ShopScene")
            {
                ItemAdded?.Invoke();
            }
        }

        public static void AddItem(IBooster booster)
        {
            var toIncrementAmountItem = items.FirstOrDefault(x => x.Type == booster.Type);
            if (toIncrementAmountItem != null)
            {
                toIncrementAmountItem.Amount++;
            }
            else
            {
                var item = new InventoryItem(booster.Type);
                items.Add(item);
            }

            if (SceneManager.GetActiveScene().name != "ShopScene")
            {
                ItemAdded?.Invoke();
            }
        }

        public static void RemoveItem(BoosterType type)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Type == type)
                {
                    items.RemoveAt(i);
                    break;
                }
            }

            ItemRemoved?.Invoke();
        }

        public static bool TryGetItem(BoosterType type, out InventoryItem item)
        {
            item = items.FirstOrDefault(item => item.Type == type);
            return item != null;
        }

        public static void UseBooster(BoosterType type)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Type != type)
                    continue;
                if (items[i].Amount > 0)
                {
                    items[i].Amount--;
                }

                if (items[i].Amount == 0)
                {
                    items.RemoveAt(i);
                }

                break;
            }

            BoosterUsed?.Invoke(type);
        }
    }
}