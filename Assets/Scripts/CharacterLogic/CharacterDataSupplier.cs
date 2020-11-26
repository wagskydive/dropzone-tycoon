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

    }
}
