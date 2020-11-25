using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AccountButton : MonoBehaviour
{
    public static event Action<string> OnAccountButtonClick;

    public string buttonID { get; private set; }

    Button button;

    Text buttonText;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();
    }

    public void SetButtonID(string id)
    {
        buttonID = id;
        buttonText.text = id;
    }

    public void ButtonClick()
    {
        OnAccountButtonClick?.Invoke(buttonID);
    }
}
