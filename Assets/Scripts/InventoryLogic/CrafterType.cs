namespace InventoryLogic
{
   public class CrafterType : ItemType
    {
        public float CraftingSpeed;

        public CrafterType[] CanAlsoCraftLikeThis;

        internal CrafterType(string name) : base(name)
        {
        }
    }
}