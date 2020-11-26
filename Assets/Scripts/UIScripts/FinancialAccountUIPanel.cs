using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FinanceLogic;
using UnityEngine.UI;




public class FinancialAccountUIPanel : MonoBehaviour
{ 
    //[Tooltip("Assign Component that can display texts. with a script that implements IDisplayText interface")]
    public GameObject textDisplayTemplate;

    string currentDisplayedAccountID;

    private void Awake()
    {
        FindObjectOfType<ShowAccountsButton>().OnShowAllAccountsButtonClick += DisplayAccounts;
    }

    public void DisplayAccounts(bool isVisible)
    {
        if (!isVisible)
        {
            
            Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;
            textDisplayTemplate.GetComponent<IDisplayAccounts>().Display(FinancialDataSupplier.AccountsIDs(bank));

        }

    }


}
