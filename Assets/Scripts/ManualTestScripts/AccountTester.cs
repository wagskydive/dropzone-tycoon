using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;
using InventoryLogic;

public class AccountTester : MonoBehaviour
{
    [SerializeField]
    private int accountAmount;

    [SerializeField]
    private int balanceRangeMin;

    [SerializeField]
    private int balanceRangeMax;

    [SerializeField]
    private int characterAmount;


    


    // Start is called before the first frame update
    void Start()
    {
        CreateTestCharacters();
        CreateTestAccounts();
    }

    private void CreateTestAccounts()
    {
        FinanceLogic.Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;
        for (int i = 0; i < accountAmount; i++)
        {
            //FinanceLogic.FinancialDataCreator.CreateNewAccount(bank, UnityEngine.Random.Range(0, 1000000000).ToString(),UnityEngine.Random.Range(balanceRangeMin, balanceRangeMax));
            FinanceLogic.FinancialDataCreator.CreateNewAccount(bank,"Builder",100);
        }
    }

    private void CreateTestCharacters()
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;

        for (int i = 0; i < characterAmount; i++)
        {
            CharacterDataCreator.CreateCharacterAccount(bank, CharacterDataCreator.CreateRandomCharacter(Random.Range(0,9999999), Random.Range(0, 9999999)));             
        }
            
    }

}
