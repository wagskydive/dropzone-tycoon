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
        FinanceLogic.Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;
        textDisplayTemplate.GetComponent<IDisplayText>().Display(FinanceLogic.FinancialDataSupplier.AccountsIDs(bank));
    }


}
