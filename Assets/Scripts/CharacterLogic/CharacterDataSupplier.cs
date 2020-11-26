using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceLogic;

namespace CharacterLogic
{
    public static class CharacterDataSupplier
    {
        public static Character GetCharacterFromAccountId(CharacterHolder holder, string id)
        {
            return holder.AllCharacters.Find(x => x.FinancialAccountID == id);
        }

        public static string[] AllCharacterNames(CharacterHolder characterHolder)
        {
            List<Character> allCharacters = characterHolder.AllCharacters;
            string[] idStrings = new string[allCharacters.Count];
            for (int i = 0; i < allCharacters.Count; i++)
            {
                idStrings[i] = allCharacters[i].CharacterName;
            }
            return idStrings;
        }

    }
}
