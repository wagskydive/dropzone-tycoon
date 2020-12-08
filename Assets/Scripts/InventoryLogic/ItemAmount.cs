using System;

namespace InventoryLogic
{
    public class ItemAmount
    {
        public event Action OnAmountZero;

        public ItemAmount(ItemType i, int a)
        {
            itemType = i;
            amount = a;
        }
        public int amount;
        public ItemType itemType;

        internal void AddAmount(int am)
        {
            amount += am;
        }
        internal int RemoveAmount(int am)
        {
            if (amount > am)
            {
                amount -= am;
                return 0;
            }
            else
            {
                int rest = am - amount;
                amount = 0;
                OnAmountZero?.Invoke();
                return rest;
            }
        }
        
    }
}
