using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceManager
{
    List<FinancialAccount> allAccounts;


    List<Transaction> ledger;


    public bool MakeTransaction(int amount, FinancialAccount from, FinancialAccount to)
    {
        if (from.currentMoney < amount)
        {
            return false;
        }
        else
        {
            Transaction transaction = new Transaction(amount, from, to);
            from.ChangeMoney(-amount);
            to.ChangeMoney(amount);
            ledger.Add(transaction);
            return true;
        }
    }
}
