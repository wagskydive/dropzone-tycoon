using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using FinanceLogic;



namespace CharacterLogic
{
    public class Character
    {
        public string CharacterName { get; private set; }
        public string FinancialAccountID { get; private set; }

        public Character(string name)
        {
            CharacterName = name;
            inventory = new Inventory();            
        }



        internal void SetFinancialAccountID(string id)
        {
            FinancialAccountID = id;
        }

        internal Inventory inventory;

    }
}
