using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterLogic;
using FinanceLogic;
using ManagementScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDetailsPanel : MonoBehaviour
{
    public Text CharacterNameText;

    public Text MoneyText;



    public string CurrentCharacterName;

    private GameManager gameManager;

    private void Awake()
    {
        CharacterButton.OnCharacterButtonClick += GetAndShowCharacterDetails;
        gameManager = FindObjectOfType<ManagementScripts.GameManager>();
    }



    private void GetAndShowCharacterDetails(string characterName)
    {
        gameManager.ActivateCharacterReturnWasActive(characterName);
        CharacterDataHolder holder = gameManager.Characters;
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

