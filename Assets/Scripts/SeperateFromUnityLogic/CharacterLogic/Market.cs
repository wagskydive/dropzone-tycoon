﻿using System;
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
            ItemAmount sellerStock = InventoryHandler.ItemsInInventory(seller.inventory, items.itemType);
            if(sellerStock.Amount >= items.Amount)
            {
                if(FinancialDataCreator.MakeTransactionFromIdString(bank, items.Amount * pricePerItem, buyer.FinancialAccountID, seller.FinancialAccountID, $"{items.Amount} {items.itemType.TypeName}"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
