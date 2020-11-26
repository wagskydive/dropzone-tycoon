using System;
using DataLogic;

namespace FinanceLogic
{
    public static class FinancialDataCreator
    {
        public static event Action<Transaction> OnTransactionProcessed;

        public static event Action<string> OnNewAccountCreated;

        public static string CreateNewAccount(Bank bank,string id, int startMoney = 0)
        {
            string[] allIds = FinancialDataSupplier.AccountsIDs(bank);
            bank.AllAccounts.Add(new FinancialAccount(DataChecks.EnsureUnique(allIds,id), startMoney));
            OnNewAccountCreated?.Invoke(bank.AllAccounts[bank.AllAccounts.Count - 1].accountID);
            return bank.AllAccounts[bank.AllAccounts.Count - 1].accountID;
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

        public static bool MakeTransactionFromIdString(Bank bank, int amount, string from, string to, string description = "")
        {
            int frm = FinancialDataSupplier.FindAccountIndex(bank,from);
            int t = FinancialDataSupplier.FindAccountIndex(bank,to);

            return MakeTransaction(bank, amount, bank.AllAccounts[frm], bank.AllAccounts[t], description);
        }

        internal static bool MakeTransaction(Bank bank, int amount, FinancialAccount from, FinancialAccount to, string description = "")
        {
            if (from.currentMoney < amount || from == to)
            {
                return false;
            }
            else
            {
                Transaction transaction = new Transaction(amount, from, to, description);
                AddTransactionToLedger(bank, transaction);
                return true;
            }
        }


    }
}
