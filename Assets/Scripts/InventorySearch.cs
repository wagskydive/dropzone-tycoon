using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySearch : MonoBehaviour
{
    public event Action<string> OnSearchRequest;


    [SerializeField]
    private InventoryParent inventoryParent;

    [SerializeField]
    InputField inputField;
    
    public void TrySearchButtonClick()
    {
        if(inputField.text != null && inputField.text != "")
        {
            OnSearchRequest?.Invoke(inputField.text);
        }
    }


}



