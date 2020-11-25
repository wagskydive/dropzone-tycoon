using System.Collections;
using System.Collections.Generic;


namespace FinanceLogic
{
    public class Transaction
    {
        internal Transaction(int amount, FinancialAccount from, FinancialAccount to)
        {
            FromAccount = from;
            FromAccountID = from.accountID;

            ToAccount = to;
            ToAccountID = to.accountID;

            Amount = amount;

            transactionID = RandomGenerator.RandomString(16);
        }

        public string transactionID { get; private set; }

        public string FromAccountID { get; private set; }

        public string ToAccountID { get; private set; }

        internal FinancialAccount FromAccount { get; private set; }

        internal FinancialAccount ToAccount { get; private set; }

        public int Amount { get; private set; }
    }
}
