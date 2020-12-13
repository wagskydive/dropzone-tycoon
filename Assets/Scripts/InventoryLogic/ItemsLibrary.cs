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

        public Action<ItemsLibrary> OnLibraryModified;

        public List<ItemType> allItems = new List<ItemType>();



        public void AddNewItemType(string typeName)
        {
            if (allItems.Count > 0)
            {
                typeName = DataChecks.EnsureUnique(allItemTypeNames(), typeName);
            }

            ItemType itemType = new ItemType(typeName);
            allItems.Add(itemType);
            OnLibraryModified?.Invoke(this);
        }

        public void AddItemsFromStringArray(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {

                if (allItems.Count > 0)
                {
                    names[i] = DataChecks.EnsureUnique(allItemTypeNames(), names[i]);
                }

                ItemType itemType = new ItemType(names[i]);
                allItems.Add(itemType);
                
            }
            OnLibraryModified?.Invoke(this);
        }

        public void AddNewCraftingType(string crafterTypeName)
        {
            if (allItems.Count > 0)
            {
                crafterTypeName = DataChecks.EnsureUnique(allItemTypeNames(), crafterTypeName);
            }

            CrafterType crafterType = new CrafterType(crafterTypeName);
            allItems.Add(crafterType);
        }

        void MakeItemIntoCrafterType(int index)
        {
            CrafterType crafterType = new CrafterType(allItems[index].TypeName);
            allItems[index] = crafterType;

        }

        public void AddItemsToRecipe(ItemAmount itemAmount, int itemIndex)
        {

                        allItems[itemIndex].AddToRecipe(itemAmount);
            OnLibraryModified?.Invoke(this);
        }

        public int IndexFromTypeName(string typeName)
        {

            int index = allItems.FindIndex(x => x.TypeName == typeName);
            return index;
        }

        public void ModifyDescription(int index, string edit)
        {
            allItems[index].SetDescription(edit);

        }

        public string[] allItemTypeNames()
        {
            string[] names = new string[allItems.Count];
            for (int i = 0; i < allItems.Count; i++)
            {

                names[i] = allItems[i].TypeName;

            }
            return names;
        }



    }
}
