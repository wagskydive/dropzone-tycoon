﻿using System;

namespace InventoryLogic
{
    public class ItemType
    {
        internal ItemType(string name, string description = null, CrafterType crafterType = null)
        {
            TypeName = name;
            Description = description;
            SetCrafterType(crafterType);
        }
        public string TypeName { get; internal set; }
        public string Description { get; internal set; }
        public Recipe recipe { get; internal set; }

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

        public void SetDescription(string edit)
        {
            Description = edit;
        }

        internal bool IsRoot()
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
