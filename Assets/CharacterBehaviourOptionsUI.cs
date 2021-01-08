using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharacterVehicleOptions
{
    Drive = 0,
    Passenger = 1,
    Mechanics = 2
}


public enum CharacterStructureOptions
{
    GoInside = 0,
    ReDesign = 1,
    Paint = 2
}


public class CharacterBehaviourOptionsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject optionButton;
    private GameObject[] vehicleOptionButtons;
    private GameObject[] structureOptionButtons;

    [SerializeField]
    Transform testTarget;

    CharacterVehicleOptions vehicleOptions = new CharacterVehicleOptions();
    CharacterStructureOptions structureOptions = new CharacterStructureOptions();

    SelectableObject hoverObj;
    bool willHide;

    CharacterObject selectedCharacter;

    private void Awake()
    {
        CharacterObject.OnShowVehicleOptions += ShowVehicleOptions;
        CharacterObject.OnShowAircraftOptions += ShowAircraftOptions;
        CharacterObject.OnShowStructureOptions += ShowStructureOptions;
        CharacterObject.OnHideVehicleOptions += ExitDetect;
        CreateVehicleOptions();
        CreateStructureOptions();
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

    private void ShowStructureOptions(CharacterObject _characterObject, StructureObject obj)
    {
        selectedCharacter = _characterObject;
        transform.position = obj.transform.position;
        for (int i = 0; i < structureOptionButtons.Length; i++)
        {
            structureOptionButtons[i].SetActive(true);
        }
        hoverObj = obj;
        obj.SetAllNonSelectable();
    }



    void UseObjectTask(SelectableObject selectableObject)
    {

        CharacterBrain characterBrain = selectedCharacter.characterBrain;
        STATE_TakeSeat takeSeat = new STATE_TakeSeat(characterBrain, selectableObject, 0, true);
        characterBrain.EnqueueState(takeSeat);

    }

    void SitAsPassenger(SelectableObject selectableObject)
    {

        CharacterBrain characterBrain = selectedCharacter.characterBrain;
        STATE_TakeSeat takeSeat = new STATE_TakeSeat(characterBrain, selectableObject, 1);
        characterBrain.EnqueueState(takeSeat);

    }
    
    void GoToStructureTask(SelectableObject selectableObject)
    {

        CharacterBrain characterBrain = selectedCharacter.characterBrain;
        Vector3 centerGroundFloor = selectableObject.transform.position+selectableObject.GetComponent<BoxCollider>().center;
        centerGroundFloor = new Vector3(centerGroundFloor.x, 0, centerGroundFloor.z);

        characterBrain.GoTo(centerGroundFloor);
        characterBrain.TriggerEnterFinishedOverride(selectableObject.GetComponent<BoxCollider>(), characterBrain.GoTo(centerGroundFloor));
        STATE_WaitSeconds waitState = new STATE_WaitSeconds(characterBrain.character, 4);
        characterBrain.EnqueueState(waitState);
        
        STATE_TakeSeat takeSeat = new STATE_TakeSeat(characterBrain, selectableObject, 0);
        characterBrain.EnqueueState(takeSeat);
    }



    private void VehicleOptionClicked(CharacterVehicleOptions option)
    {
        if (option == CharacterVehicleOptions.Drive)
        {
            UseObjectTask(hoverObj);
        }

        if (option == CharacterVehicleOptions.Passenger)
        {
            UseObjectTask(hoverObj);
        }
        hoverObj.SetAllSelectable();

    }

    private void StructureOptionClicked(CharacterStructureOptions option)
    {
        if (option == CharacterStructureOptions.GoInside)
        {
            GoToStructureTask(hoverObj);
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

    void CreateStructureOptions()
    {

        var values = Enum.GetValues(typeof(CharacterStructureOptions));
        structureOptionButtons = new GameObject[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            CharacterStructureOptions option = (CharacterStructureOptions)i;
            structureOptionButtons[i] = Instantiate(optionButton, transform);
            structureOptionButtons[i].GetComponent<OptionButton>().OnOptionClick += OptionClicked;

            structureOptionButtons[i].GetComponent<OptionButton>().SetupButton(structureOptions, i);
        }
    }

    private void OptionClicked(Enum options, int index)
    {
        if (options.GetType() == typeof(CharacterVehicleOptions))
        {
            CharacterVehicleOptions op = (CharacterVehicleOptions)index;
            VehicleOptionClicked(op);
        }
        if (options.GetType() == typeof(CharacterStructureOptions))
        {
            CharacterStructureOptions op = (CharacterStructureOptions)index;
            StructureOptionClicked(op);
        }
    }


}
