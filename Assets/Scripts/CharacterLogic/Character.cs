using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterLogic
{
    public class Character
    {
        public Character(string name, string financialAccountId)
        {
            CharacterName = name;
            FinancialAccountID = financialAccountId;
        }

        

        public string CharacterName;
        public string FinancialAccountID;
        public string[] OwnedItems;

        public string[] WishList;
    }
}
