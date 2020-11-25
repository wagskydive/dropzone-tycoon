using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryLogic
{
    public class Inventory
    {
        public Inventory()
        {
            inventoryList = new List<ItemAmount>();
            ItemAmount nullItem = new ItemAmount(new Item("nullItem"), 1);
            inventoryList.Add(nullItem);
        }

        internal List<ItemAmount> inventoryList;


    }
}
