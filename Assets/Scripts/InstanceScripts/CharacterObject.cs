using UnityEngine;
using CharacterLogic;
using System;
using UnityEngine.EventSystems;


public enum CharacterVehicleOptions
{
    Drive = 0,
    Passenger = 1,
    Mechanics = 2
}


public class CharacterObject : SelectableObject
{
    public event Action<CharacterObject> OnCharacterObjectSelected;
    public event Action<CharacterObject> OnCharacterObjectDestroy;

    public static event Action<CharacterObject,VehicleObject> OnShowVehicleOptions; 
    public static event Action<CharacterObject,AircraftObject> OnShowAircraftOptions; 
    public static event Action OnHideVehicleOptions; 

    public Character character;
    public CharacterBrain characterBrain;

    SelectableObject currentHover;

    public override void SelectObject()
    {
        base.SelectObject();
        OnCharacterObjectSelected?.Invoke(this);
        SelectableObject.OnMouseEnterDetected += ShowUseObjectOptions;
        SelectableObject.OnMouseExitDetected +=HideUseObjectOptions;
        
    }
    public override void DeselectObject()
    {
        base.DeselectObject();
        
    }

    void ShowUseObjectOptions(SelectableObject selectableObject)
    {
        currentHover = selectableObject;
        if(selectableObject.GetType() == typeof(VehicleObject))
        {
            VehicleObject vehicleObject = (VehicleObject)selectableObject;
            OnShowVehicleOptions?.Invoke(this,vehicleObject);
        }
        if (selectableObject.GetType() == typeof(AircraftObject))
        {
            AircraftObject aircraftObject = (AircraftObject)selectableObject;
            OnShowAircraftOptions?.Invoke(this, aircraftObject);
        }
    }
        
    void HideUseObjectOptions(SelectableObject selectableObject)
    {
        if(currentHover ==  selectableObject)
        {

            OnHideVehicleOptions?.Invoke();
        }
    }


    private void OnDestroy()
    {
        OnCharacterObjectDestroy?.Invoke(this);
    }
}

