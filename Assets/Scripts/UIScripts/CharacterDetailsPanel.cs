using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterLogic;
using FinanceLogic;
using ManagementScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDetailsPanel : MonoBehaviour
{
    public Text CharacterNameText;

    public Text MoneyText;

    CharacterBrain selectedCharacterBrain;

    public string CurrentCharacterName;

    CharacterObject selectedCharacterObject;
    private GameManager gameManager;

    [SerializeField]
    GoToTaskUi goToTaskUi;

    private void Awake()
    {
        CharacterButton.OnCharacterButtonClick += GetAndShowCharacterDetails;
        gameManager = FindObjectOfType<GameManager>();
        SelectableObject.OnObjectSelected += SelectCharacter;
        SelectableObject.OnObjectDeselected += DeselectCharacter;
    }

    private void DeselectCharacter(SelectableObject obj)
    {
        if(selectedCharacterObject == obj)
        {
            SetChildrenActive(false);
            selectedCharacterBrain = null;
            selectedCharacterObject = null;
        }
    }

    void SelectCharacter(SelectableObject selectableObject)
    {
        if(selectableObject.GetType() == typeof(CharacterObject))
        {
            selectedCharacterObject = (CharacterObject)selectableObject;
            selectedCharacterBrain = selectedCharacterObject.GetComponent<CharacterBrain>();
            GetAndShowCharacterDetails(selectedCharacterBrain.character);
            SetChildrenActive(true);
        }
        else
        {
            SetChildrenActive(false);
            selectedCharacterBrain = null;
        }
    }

    void SetChildrenActive(bool active)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(active);
        } 

    }

    void GetAndShowCharacterDetails(Character character)
    {
        

        CurrentCharacterName = character.CharacterName;
        CharacterNameText.text = "Name: " + character.CharacterName;
    }

    public void MoveCharacterButtonClick()
    {
        if(selectedCharacterBrain != null)
        {
            goToTaskUi.gameObject.SetActive(true);
            goToTaskUi.OnClick += MovementTask;
            goToTaskUi.OnRightClick += CancelMovementTask ;
        }
    }

    void MovementTask(Vector3 position)
    {
        goToTaskUi.OnClick -= MovementTask;
        goToTaskUi.OnRightClick -= CancelMovementTask;

        goToTaskUi.gameObject.SetActive(false);


        STATE_GoToTarget goToTarget = new STATE_GoToTarget(selectedCharacterBrain, position, 1);
        selectedCharacterBrain.EnqueueState(goToTarget);
    }

    
    void CancelMovementTask()
    {
        goToTaskUi.OnClick -= MovementTask;
        goToTaskUi.OnRightClick -= CancelMovementTask;
        goToTaskUi.gameObject.SetActive(false);
    }



    void GetAndShowCharacterDetails(string characterName)
    {
        gameManager.ActivateCharacterReturnWasActive(characterName);
        CharacterDataHolder holder = gameManager.Characters;
        CurrentCharacterName = characterName;
        CharacterNameText.text = "Name: " + characterName;
        SetMoneyText(CharacterDataSupplier.GetCharacterFromName(holder, characterName).FinancialAccountID);
    }

    void SetMoneyText(string financialAccountID)
    {
        Bank bank = FindObjectOfType<ManagementScripts.GameManager>().bank;


        MoneyText.text = "Balance: "+FinancialDataSupplier.GetBalance(bank, financialAccountID).ToString();
    }


}

