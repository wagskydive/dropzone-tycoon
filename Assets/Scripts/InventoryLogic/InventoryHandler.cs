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
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].itemType.TypeName == itemAmount.itemType.TypeName)
                {
                    inventory.items[i].Amount += itemAmount.Amount;
                    return;
                }
            }
            inventory.items.Add(itemAmount);            
        }

        public static bool RemoveFromInventory(Inventory inventory, ItemAmount itemAmount)
        {
            int index = IndexOfItemInInventory(inventory, itemAmount.itemType);
            if(index != 0 && inventory.items[index].Amount >= itemAmount.Amount)
            {
                inventory.items[index].Amount -= itemAmount.Amount;
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

        public static ItemAmount ItemsInInventory(Inventory inventory, ItemType item)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if(inventory.items[i].itemType.TypeName == item.TypeName)
                {
                    return inventory.items[i];
                }
            }
            return null;
        }

        public static int IndexOfItemInInventory(Inventory inventory, ItemType item)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].itemType.TypeName == item.TypeName)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
