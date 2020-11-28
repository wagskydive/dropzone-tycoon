﻿using System;
using System.Collections.Generic;
using FinanceLogic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AccountDetailsPanel))]
public class TransactionCreatorUI : MonoBehaviour
{
    //FIXME: Make this into dropdownhandler instead
    public Dropdown GiveMoneyTo;

    public InputField AmountToGive;

    string toGiveString;

    string currentAccountId;

    public GameObject[] AmountPart;

    public GameObject[] ReceiverPart;

    public GameObject ConfirmButton;

    public GameObject NewTransactionButton;

    private void Awake()
    {
        AccountButton.OnAccountButtonClick += ResetUI;
    }

    public void ResetUI(string id)
    {
        currentAccountId = id;

        SetActiveBoolOnGoArray(AmountPart, false);

        SetActiveBoolOnGoArray(ReceiverPart, false);

        ConfirmButton.SetActive(false);

        NewTransactionButton.SetActive(true);
    }

    public void InitializeUI()
    {
        GiveMoneyTo.onValueChanged.RemoveAllListeners();
        AmountToGive.onValueChanged.RemoveAllListeners();

        AmountToGive.text = "0";
        AmountToGive.onValueChanged.AddListener(ValidateInputField);

        SetActiveBoolOnGoArray(ReceiverPart, true);

        FinanceLogic.Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;


        //FIXME: use dropdownhandler instead
        GiveMoneyTo.gameObject.GetComponent<DropdownHandler>().PopulateDropDown(FinancialDataSupplier.AccountsIDs(bank),currentAccountId);

        GiveMoneyTo.onValueChanged.AddListener(DropdownListener);

        

        SetActiveBoolOnGoArray(AmountPart, true);

        SetActiveBoolOnGoArray(ReceiverPart, false);

        ConfirmButton.SetActive(false);

        NewTransactionButton.SetActive(false);

    }

    private void ValidateInputField(string mountString)
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;
        int amount =  int.Parse(mountString);
        if(amount > 0)
        {
            int balance = FinancialDataSupplier.GetBalance(bank, currentAccountId);
            if (amount > balance)
            {
                amount = balance;
                AmountToGive.text = balance.ToString();
            }
            SetActiveBoolOnGoArray(ReceiverPart, true);
        }
        else
        {
            SetActiveBoolOnGoArray(ReceiverPart, false);
            ConfirmButton.SetActive(false);
        }
    }

    void SetActiveBoolOnGoArray(GameObject[] objects, bool val)
    {
        foreach (var go in objects)
        {
            go.SetActive(val);
        }
    }

    public void ConfirmTransaction()
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;

        int amount = int.Parse(AmountToGive.text);
        SubmitTransaction(toGiveString);
        ResetUI(currentAccountId);
    }

    private void DropdownListener(int index)
    {
        toGiveString = GiveMoneyTo.options[index].text;
        ConfirmButton.SetActive(true);
    }

    private void SubmitTransaction(string sendMoneyTo)
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;
        FinanceLogic.FinancialDataCreator.MakeTransactionFromIdString(bank, int.Parse(AmountToGive.text), currentAccountId, sendMoneyTo, " Transfer trough Transaction UI");
    }

}
