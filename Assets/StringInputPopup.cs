using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringInputPopup : MonoBehaviour
{
    public event Action<string> OnInputConfirmed;

    public InputField inputField;
    public Text headerText;

    string inputValue;

    public void Assign(string header = "Name:")
    {
        headerText.text = header;
        inputField.onEndEdit.AddListener(delegate { InputDetected(); });

    }

    void InputDetected()
    {
        inputValue = inputField.text;
    }

    private void OnEnable()
    {
        inputField.text = "";
    }

    public void OkButtonClick()
    {
        if(inputField.text != "")
        {
            OnInputConfirmed?.Invoke(inputField.text);
            gameObject.SetActive(false);
        }
        
    }
}
