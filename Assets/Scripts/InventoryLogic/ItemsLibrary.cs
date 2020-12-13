using System;
using System.Collections.Generic;
using DataLogic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryLogic
{

    public class ItemsLibrary
    {
        public List<ItemType> allItems = new List<ItemType>();


        public void AddNewItemType(string typeName)
        {

            if(allItems.Count > 0)
            {
                typeName =DataChecks.EnsureUnique(allItemTypeNames(), typeName);
            }
            
            ItemType itemType = new ItemType(typeName);
            allItems.Add(itemType);
        }

        public void AddItemsToRecipe(ItemAmount itemAmount, int itemIndex)
        {
            ItemType itemType = allItems[itemIndex];
            itemType.recipe.Add(itemAmount);
        }


        int IndexFromTypeName(string typeName)
        {
            return allItems.FindIndex(x => x.typeName == typeName);
        }

        public string[] allItemTypeNames()
        {
            string[] names = new string[allItems.Count];
            for (int i = 0; i < allItems.Count; i++)
            {
                names[i] = allItems[i].typeName;
            }
            return names;
        }


    }
}
