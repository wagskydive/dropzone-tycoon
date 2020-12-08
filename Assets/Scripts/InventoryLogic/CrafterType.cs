﻿using System;

namespace InventoryLogic
{
   public class CrafterType : ItemType, ItemSuppier
    {
        public float CraftingSpeed;

        public CrafterType[] CanAlsoCraftLikeThis;

        internal CrafterType(string name) : base(name)
        {

        }

        public event Action<ItemType> OnItemSupplied;

        public ItemType SupplyItem()
        {
            OnItemSupplied?.Invoke()
        }
    }
}