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
        public static Character GetCharacterFromAccountId(CharacterDataHolder holder, string id)
        {
            return holder.AllCharacters.Find(x => x.FinancialAccountID == id);
        }

        public static Character GetCharacterFromName(CharacterDataHolder holder, string name)
        {
            return holder.AllCharacters.Find(x => x.CharacterName == name);
        }



        public static Character GetCharacterFromIndex(CharacterDataHolder holder, int index)
        {
            return holder.AllCharacters[index];
        }

        public static int GetIndexFromName(CharacterDataHolder holder, string name)
        {
            return holder.AllCharacters.FindIndex(x => x.CharacterName == name);
        }

        public static string[] AllCharacterNames(CharacterDataHolder characterHolder)
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
