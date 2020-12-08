using System;
using System.Collections;
using System.Collections.Generic;
using InventoryLogic;
using DataLogic;

using UnityEngine;
using UnityEngine.UI;
using ManagementScripts;

public class ItemCreatorPanel : MonoBehaviour
{
    public event Action<string> OnNewItemTypeCreated;

    public InputField NameInput;

    ItemsLibrary library;

    GameManager gameManager;



    private void Start()

    {
        gameManager = FindObjectOfType<GameManager>();
        library = gameManager.Library;
    }

    public void CreateItemButtonClick()
    {

        library.AddNewItemType(NameInput.text);



        if (NameInput.text == null || NameInput.text == "")
        {
            return;
        }
        string inputText = DataChecks.EnsureUnique(gameManager.Library.allItemTypeNames(), NameInput.text);

        Dictionary<string, float> effectors = new Dictionary<string, float>();
        effectors.Add("Test effector 1", .1f);
        effectors.Add("Test effector 2", .2f);

        string description = "No Description";

        gameManager.Library.AddNewItemType(inputText);
        OnNewItemTypeCreated?.Invoke(inputText);
    }
}
