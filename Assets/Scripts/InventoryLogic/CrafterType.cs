using System;

namespace InventoryLogic
{
   public class CrafterType : ItemType, ItemSupplier
    {
        public float CraftingSpeed;

        public CrafterType[] CanAlsoCraftLikeThis;

        internal CrafterType(string name) : base(name)
        {

        }

        public event Action<ItemType> OnItemSupplied;

        public ItemType SupplyItem()
        {
            return null;
        }
    }
}