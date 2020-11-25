using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceLogic
{
    public static class FinancialDataSupplier
    {
        internal static int FindAccountIndex(Bank bank, string ID)
        {
            return bank.AllAccounts.FindIndex(x => x.accountID == ID);
        }



        public static string[] AccountsIDs(Bank bank)
        {
            
            List<FinancialAccount> allAccounts = bank.AllAccounts;
            string[] idStrings = new string[allAccounts.Count];
            for (int i = 0; i < allAccounts.Count; i++)
            {
                idStrings[i] = allAccounts[i].accountID;
            }
            return idStrings;
        }

        public static int GetBalance(Bank bank, string id)
        {
            return bank.AllAccounts[FindAccountIndex(bank, id)].currentMoney;
        }

        public static List<Transaction> GetTransactionHistory(Bank bank, string id)
        {
            return bank.AllAccounts[FindAccountIndex(bank, id)].transactionHistory;
        }

    }       
}
