using SpawnLogic;

namespace InventoryLogic
{
    public class Item : ISpawnable
    {
        public Item(string typeName)
        {
            itemType=typeName;
        }
        public string itemType { get; private set; }

        public string ResourcePath()
        {
            return itemType;
        }
    }
}
