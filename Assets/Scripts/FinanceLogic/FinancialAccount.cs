using System.Collections.Generic;

namespace FinanceLogic
{
    internal class FinancialAccount
    {
        public FinancialAccount(string ID, int startMoney = 0)
        {
            System.Random _random = new System.Random();
            
            accountID = ID;
            currentMoney = startMoney;

            transactionHistory = new List<Transaction>();
        }

        public string accountID;

        public int currentMoney { get; private set; }

        public List<Transaction> transactionHistory { get; private set; }

        public void ChangeMoney(Transaction transaction)
        {

            if(accountID == transaction.FromAccount.accountID)
            {
                currentMoney -= transaction.Amount;
            }

            else if (accountID == transaction.ToAccount.accountID)
            {
                currentMoney += transaction.Amount;
            }
            else
            {

                //Notify wrong transaction
                return;
            }

            transactionHistory.Add(transaction);
        }
    }
  
}
