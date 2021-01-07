using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviourOptionsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject optionButton;
    private GameObject[] vehicleOptionButtons;

    [SerializeField]
    Transform testTarget;

    CharacterVehicleOptions vehicleOptions = new CharacterVehicleOptions();


    SelectableObject hoverObj;
    bool willHide;

    CharacterObject selectedCharacter;

    private void Awake()
    {
        CharacterObject.OnShowVehicleOptions += ShowVehicleOptions;
        CharacterObject.OnShowAircraftOptions += ShowAircraftOptions;
        CharacterObject.OnHideVehicleOptions += ExitDetect;
        CreateVehicleOptions();
    }

    void ExitDetect()
    {
        Invoke("HideVehicleOptions", .7f);
        willHide = true;
    }

    private void HideVehicleOptions()
    {
        for (int i = 0; i < vehicleOptionButtons.Length; i++)
        {
            vehicleOptionButtons[i].SetActive(false);
        }
        willHide = false;
    }

    private void ShowAircraftOptions(CharacterObject _characterObject, AircraftObject obj)
    {
        selectedCharacter = _characterObject;
        if (willHide)
        {
            CancelInvoke("HideVehicleOptions");
        }
        transform.position = obj.transform.position;
        for (int i = 0; i < vehicleOptionButtons.Length; i++)
        {
            vehicleOptionButtons[i].SetActive(true);
        }
        hoverObj = obj;
        obj.SetAllNonSelectable();
    }
    
    private void ShowVehicleOptions(CharacterObject _characterObject, VehicleObject obj)
    {
        selectedCharacter = _characterObject;
        if (willHide)
        {
            CancelInvoke("HideVehicleOptions");
        }
        transform.position = obj.transform.position;
        for (int i = 0; i < vehicleOptionButtons.Length; i++)
        {
            vehicleOptionButtons[i].SetActive(true);
        }
        hoverObj = obj;
        obj.SetAllNonSelectable();
    }

    void UseObjectTask(SelectableObject selectableObject)
    {

        CharacterBrain characterBrain = selectedCharacter.characterBrain;
        STATE_TakeSeat takeSeat = new STATE_TakeSeat(characterBrain, selectableObject, 0);
        characterBrain.EnqueueState(takeSeat);

    }



    private void VehicleOptionClicked(CharacterVehicleOptions option)
    {
        if (option == CharacterVehicleOptions.Drive)
        {
            UseObjectTask(hoverObj);
        }
        hoverObj.SetAllSelectable();

    }

    void CreateVehicleOptions()
    {

        var values = Enum.GetValues(typeof(CharacterVehicleOptions));
        vehicleOptionButtons = new GameObject[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            CharacterVehicleOptions option = (CharacterVehicleOptions)i;
            vehicleOptionButtons[i] = Instantiate(optionButton, transform);
            vehicleOptionButtons[i].GetComponent<OptionButton>().OnOptionClick += OptionClicked;

            vehicleOptionButtons[i].GetComponent<OptionButton>().SetupButton(vehicleOptions, i);
        }
    }

    private void OptionClicked(Enum arg1, int arg2)
    {
        if(arg1.GetType() == typeof(CharacterVehicleOptions))
        {
            CharacterVehicleOptions op = (CharacterVehicleOptions)arg2;
            VehicleOptionClicked(op);
        }
    }
}
