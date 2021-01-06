using System;
using UnityEngine;

namespace InventoryLogic
{
    [System.Serializable]
    public class ItemType
    {
        public ItemType(string name, string resourcePath = "", string catagory = null, string description = null, CrafterType crafterType = null, bool isObstacle = true, bool stretchItem = false)
        {
            if (name.EndsWith(".fbx"))
            {
                name = name.Substring(0, name.Length - 4);          
            }
            TypeName = name;
            if(resourcePath == "")
            {
                ResourcePath = "Items/" + name;
            }
            else
            {
                if (resourcePath.EndsWith(".fbx"))
                {
                    resourcePath = resourcePath.Substring(0, resourcePath.Length - 4);                    
                }
                if (!resourcePath.EndsWith(name))
                {
                    resourcePath += name;
                }
                if (!resourcePath.StartsWith("Items/"))
                {
                    resourcePath = "Items/" + resourcePath;
                }
                ResourcePath = resourcePath;
            }
            Catagory = catagory;
            Description = description;
            SetCrafterType(crafterType);
            IsObstacle = isObstacle;
            IsStretchItem = stretchItem;
        }

        public string TypeName { get; internal set; }
        public string Catagory { get; internal set; }
        public string Description { get; internal set; }
        public string ResourcePath { get; internal set; }
        public Recipe recipe { get; internal set; }
        public bool IsObstacle { get; internal set; }
        public bool IsStretchItem { get; internal set; }

        internal string[] skillsRelated;

        public CrafterType CrafterType { get; private set; }

        public void SetCrafterType(CrafterType type)
        {
            CrafterType = type;
        }

        public void SetTypeName(string name)
        {
            TypeName = name;
        }

        public void AddToRecipe(ItemAmount itemAmount)
        {
            if(recipe == null)
            {
                recipe = new Recipe();
            }
            recipe.Add(itemAmount);
        }

        public void SetOutputAmountOnRecipe(int amount)
        {
            recipe.SetOutputAmount(amount);
        }

        public void SetDescription(string edit)
        {
            Description = edit;
        }

        public bool IsRoot()
        {
            if (recipe == null || recipe.Ingredients == null || recipe.Ingredients.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
