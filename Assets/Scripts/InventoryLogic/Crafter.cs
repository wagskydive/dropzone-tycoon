using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLogic;

namespace InventoryLogic
{
    public class Crafter : Item
    {
        public Crafter(CrafterType type) : base(type)
        {
            crafterType = type;
            jobsManager = new JobsManager();
        }

        JobsManager jobsManager;

        CrafterType crafterType;

        Inventory input;
        Inventory output;
    }
}
