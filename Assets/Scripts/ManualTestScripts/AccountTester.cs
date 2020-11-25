using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

}
