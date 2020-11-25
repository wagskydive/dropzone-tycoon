using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ConfirmButton : MonoBehaviour
{
    Button button;

    public InputField inputField;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        inputField.onValueChanged.AddListener(CheckInputField);
    }

    private void CheckInputField(string inputFromField)
    {
        
        if(int.Parse(inputFromField) > 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
