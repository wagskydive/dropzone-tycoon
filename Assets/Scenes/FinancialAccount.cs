using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinancialAccount
{
    public int currentMoney { get; private set; }

    public List<Transaction> transactionHistory { get; private set; }

    public void ChangeMoney(int amount)
    {
        currentMoney += amount;
    }

}
