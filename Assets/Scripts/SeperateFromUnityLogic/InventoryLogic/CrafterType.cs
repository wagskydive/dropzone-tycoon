using System;
using System.Collections.Generic;

namespace InventoryLogic
{
    public class CrafterType : ItemType
    {
        public float CraftingSpeed;

        public CrafterType[] CanAlsoCraftLikeThis;

        internal CrafterType(string name) : base(name)
        {

        }

        public List<CrafterType> GetCraftingTypesRecursive()
        {
            List<CrafterType> output = new List<CrafterType>();
            output.Add(this);
            if (CanAlsoCraftLikeThis != null)
            {
               for (int i = 0; i < CanAlsoCraftLikeThis.Length; i++)
                {
                    output.AddRange(CanAlsoCraftLikeThis[i].GetCraftingTypesRecursive());
                }
            }
            return output;
        }
    }
}