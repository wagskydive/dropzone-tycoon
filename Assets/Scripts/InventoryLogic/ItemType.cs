namespace InventoryLogic
{
    public class ItemType
    {
        internal ItemType(string name)
        {
            name = typeName;
        }
        public string typeName { get; internal set; }
        internal Recipe recipe;

        internal string[] skillsRelated;



    }
}
