using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceLogic
{
    public static class FinancialDataCreator
    {
        public static event Action<Transaction> OnTransactionProcessed;

        public static event Action<string> OnNewAccountCreated;

        public static void CreateNewAccount(Bank bank,string id, int startMoney = 0)
        {            
            bank.AllAccounts.Add(new FinancialAccount(EnsureUnique(bank,id), startMoney));
            OnNewAccountCreated?.Invoke(bank.AllAccounts[bank.AllAccounts.Count - 1].accountID);
        }

        public static bool CheckForIdExists(Bank bank, string idToCheck)
        {
            return bank.AllAccounts.Exists(x => x.accountID == idToCheck);
        }

        public static string EnsureUnique(Bank bank, string id)
        {
            int append = 1;
            while (CheckForIdExists(bank, id))
            {
                if (id.EndsWith($"_{ append }"))
                {
                    id = id.Remove(id.Length - $"_{ append }".Length, $"_{ append }".Length);                    
                }
                
                append++;
                id += $"_{ append }";

            }
            return id;
 
        }

        internal static void AddTransactionToLedger(Bank bank,Transaction transaction)
        {
            bank.Ledger.Add(transaction);
            int from = bank.AllAccounts.FindIndex(x => x.accountID == transaction.FromAccount.accountID);
            int to = bank.AllAccounts.FindIndex(x => x.accountID == transaction.ToAccount.accountID);

            bank.AllAccounts[from].ChangeMoney(transaction);
            bank.AllAccounts[to].ChangeMoney(transaction);
            OnTransactionProcessed?.Invoke(transaction);
        }

        public static bool MakeTransactionFromIdString(Bank bank, int amount, string from, string to)
        {
            int frm = FinancialDataSupplier.FindAccountIndex(bank,from);
            int t = FinancialDataSupplier.FindAccountIndex(bank,to);

            return MakeTransaction(bank, amount, bank.AllAccounts[frm], bank.AllAccounts[t]);
        }

        internal static bool MakeTransaction(Bank bank, int amount, FinancialAccount from, FinancialAccount to)
        {
            if (from.currentMoney < amount || from == to)
            {
                return false;
            }
            else
            {
                Transaction transaction = new Transaction(amount, from, to);
                AddTransactionToLedger(bank, transaction);
                return true;
            }
        }


    }
}
