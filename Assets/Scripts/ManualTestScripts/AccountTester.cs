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

    public FinanceLogic.FinancialDataHolder testBank;


    // Start is called before the first frame update
    void Start()
    {
        testBank = new FinanceLogic.FinancialDataHolder();
        CreateTestAccounts();
    }

    private void CreateTestAccounts()
    {
        for (int i = 0; i < accountAmount; i++)
        {
            FinanceLogic.FinancialDataCreator.CreateNewAccount(testBank, UnityEngine.Random.Range(0, 1000000000).ToString(),UnityEngine.Random.Range(balanceRangeMin, balanceRangeMax));
        }
    }

    

}
