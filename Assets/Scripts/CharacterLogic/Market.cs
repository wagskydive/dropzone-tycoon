using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryLogic;
using FinanceLogic;

namespace CharacterLogic
{
    public class Market
    {
        public static bool ProcessOrder(Bank bank, Character buyer, Character seller, ItemAmount items, int pricePerItem)
        {
            ItemAmount sellerStock = InventoryHandler.ItemsInInventory(seller.inventory, items.item);
            if(sellerStock.amount >= items.amount)
            {
                if(FinancialDataCreator.MakeTransactionFromIdString(bank, items.amount * pricePerItem, buyer.FinancialAccountID, seller.FinancialAccountID, $"{items.amount} {items.item.itemType}"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
