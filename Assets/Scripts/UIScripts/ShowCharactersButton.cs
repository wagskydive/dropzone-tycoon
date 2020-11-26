using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCharactersButton : MonoBehaviour
{
    public event Action<bool> OnShowAllCharactersButtonClick;

    public GameObject AllCharactersPanel;

    public GameObject DetailCharacterPanel;


    bool isVisible = false;

    public void ClickButton()
    {
        if (isVisible)
        {
            AllCharactersPanel.SetActive(false);
            DetailCharacterPanel.SetActive(false);
            OnShowAllCharactersButtonClick?.Invoke(isVisible);
            GetComponentInChildren<Text>().text = "Show Characters";
        }
        else
        {
            AllCharactersPanel.SetActive(true);
            DetailCharacterPanel.SetActive(true);
            OnShowAllCharactersButtonClick?.Invoke(isVisible);
            GetComponentInChildren<Text>().text = "Hide Characters";
        }

        isVisible = !isVisible;
    }
}
