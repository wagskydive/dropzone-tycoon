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

        public static void CreateNewAccount(FinancialDataHolder bank,string id, int startMoney = 0)
        {
            bank.AllAccounts.Add(new FinancialAccount(id, startMoney));
            OnNewAccountCreated?.Invoke(bank.AllAccounts[bank.AllAccounts.Count - 1].accountID);
        }

        internal static void AddTransactionToLedger(FinancialDataHolder bank,Transaction transaction)
        {
            bank.Ledger.Add(transaction);
            int from = bank.AllAccounts.FindIndex(x => x.accountID == transaction.FromAccount.accountID);
            int to = bank.AllAccounts.FindIndex(x => x.accountID == transaction.ToAccount.accountID);

            bank.AllAccounts[from].ChangeMoney(transaction);
            bank.AllAccounts[to].ChangeMoney(transaction);
            OnTransactionProcessed?.Invoke(transaction);
        }

        public static bool MakeTransactionFromIdString(FinancialDataHolder bank, int amount, string from, string to)
        {
            int frm = FinancialDataSupplier.FindAccountIndex(bank,from);
            int t = FinancialDataSupplier.FindAccountIndex(bank,to);

            return MakeTransaction(bank, amount, bank.AllAccounts[frm], bank.AllAccounts[t]);
        }

        internal static bool MakeTransaction(FinancialDataHolder bank, int amount, FinancialAccount from, FinancialAccount to)
        {
            if (from.currentMoney < amount)
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
