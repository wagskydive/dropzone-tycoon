using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryLogic
{
    public static class InventoryHandler
    {
        public static void AddToInventory(Inventory inventory, ItemAmount itemAmount)
        {
            for (int i = 0; i < inventory.inventoryList.Count; i++)
            {
                if (inventory.inventoryList[i].item.itemType == itemAmount.item.itemType)
                {
                    inventory.inventoryList[i].amount += itemAmount.amount;
                    return;
                }
            }
            inventory.inventoryList.Add(itemAmount);            
        }

        public static bool RemoveFromInventory(Inventory inventory, ItemAmount itemAmount)
        {
            int index = IndexOfItemInInventory(inventory, itemAmount.item);
            if(index != 0 && inventory.inventoryList[index].amount >= itemAmount.amount)
            {
                inventory.inventoryList[index].amount -= itemAmount.amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TransferItemsBetweenInventories(Inventory from, Inventory to, ItemAmount items)
        {
            if(RemoveFromInventory(from, items))
            {
                AddToInventory(to, items);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ItemAmount ItemsInInventory(Inventory inventory, Item item)
        {
            for (int i = 0; i < inventory.inventoryList.Count; i++)
            {
                if(inventory.inventoryList[i].item.itemType == item.itemType)
                {
                    return inventory.inventoryList[i];
                }
            }
            return null;
        }

        public static int IndexOfItemInInventory(Inventory inventory, Item item)
        {
            for (int i = 0; i < inventory.inventoryList.Count; i++)
            {
                if (inventory.inventoryList[i].item.itemType == item.itemType)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
