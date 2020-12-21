using System;

namespace InventoryLogic
{
    public class ItemAmount
    {
        public event Action OnAmountZero;

        public int Amount;
        public ItemType itemType;


        public ItemAmount(ItemType itemType, int amount)
        {
            this.itemType = itemType;
            Amount = amount;
        }

        internal void AddAmount(int amountToAdd)
        {
            Amount += amountToAdd;
        }

        //Tries to remove set amount, returns remaining if requested amount is to much;
        internal int RemoveAmount(int amountToRemove)
        {
            if (Amount > amountToRemove)
            {
                Amount -= amountToRemove;
                return 0;
            }
            else
            {
                int rest = amountToRemove - Amount;
                Amount = 0;
                OnAmountZero?.Invoke();
                return rest;
            }
        }
        
    }
}
