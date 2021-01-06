using SpawnLogic;

namespace InventoryLogic
{
    [System.Serializable]
    public class Item : ISpawnable
    {
        public ItemType itemType;

        

        public Item(ItemType type)
        {
            itemType=type;
        }



        public string ResourcePath()
        {
            return itemType.ResourcePath;
        }

    }
}
