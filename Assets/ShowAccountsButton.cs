using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAccountsButton : MonoBehaviour
{
    public event Action<bool> OnShowAllAccountsButtonClick;

    public GameObject AllAccountsPanel;

    public GameObject DetailAccountPanel;

    bool isVisible = false;

    public void ClickButton()
    {
        if (isVisible)
        {
            AllAccountsPanel.SetActive(false);
            DetailAccountPanel.SetActive(false);
            GetComponentInChildren<Text>().text = "Show Accounts";
        }
        else
        {
            AllAccountsPanel.SetActive(true);
            DetailAccountPanel.SetActive(true);
            OnShowAllAccountsButtonClick?.Invoke(isVisible);
            GetComponentInChildren<Text>().text = "Hide Accounts";
        }

        isVisible = !isVisible;
    }
}
