using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryLogic
{
    public class Recipe
    {
        public ItemAmount[] Ingredients { get; private set; }
        public CrafterType CraftingItem { get; private set; }
        public string[] SkillsRequired { get; private set; }

        internal void Add(ItemAmount itemAmount)
        {
            if (Ingredients == null)
            {
                Ingredients = new ItemAmount[1];
                Ingredients[0] = itemAmount;
            }
            else
            {
                for (int i = 0; i < Ingredients.Length; i++)
                {
                    if (Ingredients[i].itemType.TypeName == itemAmount.itemType.TypeName)
                    {
                        Ingredients[i].AddAmount(itemAmount.amount);
                        return;
                    }
                }
                ItemAmount[] newIngredients = new ItemAmount[Ingredients.Length + 1];
                for (int i = 0; i < newIngredients.Length; i++)
                {
                    if(i == newIngredients.Length - 1)
                    {
                        newIngredients[i] = itemAmount;
                    }
                    else
                    {
                        newIngredients[i] = Ingredients[i];
                    }
                    
                }
                Ingredients = newIngredients;
            }
        }

        internal bool HasItemType(ItemType type)
        {
            for (int i = 0; i < Ingredients.Length; i++)
            {
                if (Ingredients[i].itemType.TypeName == type.TypeName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
