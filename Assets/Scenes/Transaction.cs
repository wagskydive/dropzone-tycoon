using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transaction
{
    public Transaction(int amount, FinancialAccount from, FinancialAccount to)
    {
        if(from.currentMoney < amount)
        {
            Debug.Log("transaction failed. not enough in -from- account.");
        }
    }
}
