namespace InventoryLogic
{
    public class Item
    {
        public Item(string typeName)
        {
            itemType=typeName;
        }
        public string itemType { get; private set; }
    }
}
