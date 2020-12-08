using System.Collections.Generic;

namespace InventoryLogic
{
    public static class CraftingDataSupplier

    { 

        public static List<ItemType> CraftableItemsForCraftingType(List<ItemType> allItems, CrafterType crafterType)
        {
            List<ItemType> items = new List<ItemType>();

            for (int i = 0; i < allItems.Count; i++)
            {
                if(crafterType.GetCraftingTypesRecursive().Contains(allItems[i].CrafterType))
                {
                    items.Add(allItems[i]);
                }

            }
            if(items.Count == 0)
            {
                return null;
            }

            return items;

        }




        
    }
}
