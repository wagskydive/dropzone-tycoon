using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using FinanceLogic;



namespace CharacterLogic
{
    public class Character
    {
        public Character(string name)
        {
            CharacterName = name;
            inventory = new Inventory();
            
        }

        public string CreateAccount(Bank bank, int startMoney =0)
        {
            FinancialAccountID = FinancialDataCreator.CreateNewAccount(bank, CharacterName, startMoney);
            return FinancialAccountID;
        }

        public string CharacterName;
        public string FinancialAccountID;

        public Inventory inventory;

    }
}
