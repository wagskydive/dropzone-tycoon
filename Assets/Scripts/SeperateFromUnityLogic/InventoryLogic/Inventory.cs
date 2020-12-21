using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryLogic
{
    public class Inventory
    {
        public Inventory()
        {
            items = new List<ItemAmount>();
            //ItemAmount nullItem = new ItemAmount(new Item("nullItem"), 1);
            //inventoryList.Add(nullItem);
        }

        public List<ItemAmount> items { get; internal set; }


    }
}
