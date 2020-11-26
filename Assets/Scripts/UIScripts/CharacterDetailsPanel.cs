using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterLogic;
using FinanceLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDetailsPanel : MonoBehaviour
{
    public Text CharacterNameText;

    public Text MoneyText;



    public string CurrentCharacterName;

    private void Awake()
    {
        CharacterButton.OnCharacterButtonClick += GetAndShowCharacterDetails;
    }



    private void GetAndShowCharacterDetails(string characterName)
    {
        CharacterDataHolder holder = FindObjectOfType<ManagementScripts.GameManager>().Characters;
        CurrentCharacterName = characterName;
        CharacterNameText.text = "Name: " + characterName;
        SetMoneyText(CharacterDataSupplier.GetCharacterFromName(holder, characterName).FinancialAccountID);
    }

    void SetMoneyText(string financialAccountID)
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;


        MoneyText.text = "Balance: "+FinancialDataSupplier.GetBalance(bank, financialAccountID).ToString();
    }


}

