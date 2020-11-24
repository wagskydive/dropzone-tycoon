using System.Collections;
using System.Collections.Generic;


namespace FinanceLogic
{
    public class Transaction
    {
        internal Transaction(int amount, FinancialAccount from, FinancialAccount to)
        {
            FromAccount = from;
            ToAccount = to;
            Amount = amount;

            transactionID = RandomGenerator.RandomString(16);
        }

        public string transactionID { get; private set; }
    
        internal FinancialAccount FromAccount { get; private set; }

        internal FinancialAccount ToAccount { get; private set; }

        internal int Amount { get; private set; }
    }
}
