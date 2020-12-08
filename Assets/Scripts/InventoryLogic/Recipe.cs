using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryLogic
{
    internal class Recipe
    {
        internal ItemAmount[] Materials;
        internal CrafterType CraftingItem;
        internal string[] SkillsRequired;

        internal void Add(ItemAmount itemAmount)
        {
            if(Materials == null)
            {
                Materials = new ItemAmount[1];
                Materials[0] = itemAmount;
            }
            else
            {
                for (int i = 0; i < Materials.Length; i++)
                {
                    if(Materials[i].itemType.typeName == itemAmount.itemType.typeName)
                    {
                        Materials[i].AddAmount(itemAmount.amount);
                    }
                }
            }
        }

        internal bool HasItemType(ItemType type)
        {
            for (int i = 0; i < Materials.Length; i++)
            {
                if(Materials[i].itemType.typeName== type.typeName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
