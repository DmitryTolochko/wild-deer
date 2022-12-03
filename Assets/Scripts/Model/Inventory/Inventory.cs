using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Model.Boosters;
using ServiceInstances;

namespace Model.Inventory
{
    public static class Inventory
    {
        private static readonly List<Item> items = new();
        public static IEnumerable<Item> GetAllItems() => items;
        public static event Action ItemAdded;
        public static event Action BoosterUsed;

        static Inventory()
        {
            AddItem(new FoodBooster());
            AddItem(new ProtectiveCapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            AddItem(new PinkTrapBooster());
            
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
                var item = new Item(booster.Type);
                items.Add(item);
            }

            ItemAdded?.Invoke();
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

            BoosterUsed?.Invoke();
        }
    }
}