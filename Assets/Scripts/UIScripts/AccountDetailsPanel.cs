using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinanceLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccountDetailsPanel : MonoBehaviour
{
    public Text TransactionsText;

    public Text AccountIDText;

    public Text BalanceText;


    public string CurrentAccountId;

    private void Awake()
    {
        AccountButton.OnAccountButtonClick += GetAndShowAccountDetails;
        FinanceLogic.FinancialDataCreator.OnTransactionProcessed += Refresh;

    }

    void Refresh(Transaction transaction = null)
    {
        GetAndShowAccountDetails(CurrentAccountId);
    }

    private void GetAndShowAccountDetails(string id)
    {
        CurrentAccountId = id;
        FinanceLogic.Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;

        AccountIDText.text = "Account ID: " + id;

        int balance = FinanceLogic.FinancialDataSupplier.GetBalance(bank, id);
        ShowBalanceText(balance.ToString());

        List<FinanceLogic.Transaction> transactions = FinanceLogic.FinancialDataSupplier.GetTransactionHistory(bank, id);

        TransactionsText.text = ShowTransactionHistory(transactions);
    }



    private void ShowBalanceText(string v)
    {
        BalanceText.text = "Balance: " + v;
    }



    string ShowTransactionHistory(List<FinanceLogic.Transaction> transactions)
    {

        if (transactions != null && transactions.Count > 0)
        {
            string historyString = "";
            for (int i = 0; i < transactions.Count; i++)
            {
                if (transactions[i].FromAccountID == CurrentAccountId)
                {
                    historyString += "-" + transactions[i].Amount.ToString() + " sent to: " + transactions[i].ToAccountID;
                }
                else if (transactions[i].ToAccountID == CurrentAccountId)
                {
                    historyString += transactions[i].Amount.ToString() + " received from: " + transactions[i].FromAccountID;

                }
                else
                {
                    historyString += "Invalid transaction found: " + transactions[i].Amount.ToString() + "from: " + transactions[i].FromAccountID + " sent to: " + transactions[i].ToAccountID + "\n";
                }

                historyString += " for: " + transactions[i].Description + "\n";
            }
            return "Transaction History: \n" + historyString;
        }
        else
        {
            return "No transactions yet";
        }
    }

}

