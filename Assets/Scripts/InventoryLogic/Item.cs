using SpawnLogic;

namespace InventoryLogic
{
    public class Item : ISpawnable
    {
        public ItemType itemType;
        public Item(ItemType type)
        {
            itemType=type;
        }

        public string itemType { get; private set; }

        public string ResourcePath()
        {
            return itemType;
        }

    }
}
