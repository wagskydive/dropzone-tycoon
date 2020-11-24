using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceLogic
{
    public static class FinancialDataSupplier
    {
        internal static int FindAccountIndex(FinancialDataHolder bank, string ID)
        {
            return bank.AllAccounts.FindIndex(x => x.accountID == ID);
        }

        public static string AccountsInfo(FinancialDataHolder bank)
        {
            string textString = "Account Display\n\n";
            List<FinancialAccount> allAccounts = bank.AllAccounts;
            for (int i = 0; i < allAccounts.Count; i++)
            {
                textString += $"index: { i } \n";
                textString += $"ID: { allAccounts[i].accountID } \n";
                textString += $"Current Amount: { allAccounts[i].currentMoney } \n\n";
            }
            return textString;
        }
    }       
}
