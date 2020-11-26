using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowCharacterUiButton : MonoBehaviour
{
    public event Action OnShowCharacterButtonClicked;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ClickButton()
    {
        OnShowCharacterButtonClicked?.Invoke();
    }
}
