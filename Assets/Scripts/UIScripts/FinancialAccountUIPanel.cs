using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;




public class FinancialAccountUIPanel : MonoBehaviour
{ 
    //[Tooltip("Assign Component that can display texts. with a script that implements IDisplayText interface")]
    public GameObject textDisplayTemplate;

    string currentDisplayedAccountID;


    void RefreshDisplay()
    {

    }

    public void DisplayAccounts()
    {
        FinanceLogic.FinancialDataHolder bank = FindObjectOfType<AccountTester>().testBank;
        textDisplayTemplate.GetComponent<IDisplayText>().SetText(FinanceLogic.FinancialDataSupplier.AccountsInfo(bank));
    }


}
