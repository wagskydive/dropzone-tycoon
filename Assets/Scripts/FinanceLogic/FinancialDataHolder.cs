using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceLogic
{
    public class FinancialDataHolder
    {


        internal List<FinancialAccount> AllAccounts = new List<FinancialAccount>();

        internal List<Transaction> Ledger = new List<Transaction>();
    }


}
