using UnityEngine;
using CharacterLogic;
using System;
using UnityEngine.EventSystems;




public class CharacterObject : SelectableObject
{
    public event Action<CharacterObject> OnCharacterObjectSelected;
    public event Action<CharacterObject> OnCharacterObjectDestroy;

    public static event Action<CharacterObject,VehicleObject> OnShowVehicleOptions; 
    public static event Action<CharacterObject,AircraftObject> OnShowAircraftOptions; 
    public static event Action<CharacterObject,StructureObject> OnShowStructureOptions; 
    public static event Action OnHideOptions; 

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
        SelectableObject.OnMouseEnterDetected -= ShowUseObjectOptions;
        SelectableObject.OnMouseExitDetected -= HideUseObjectOptions;
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
        if (selectableObject.GetType() == typeof(StructureObject))
        {
            StructureObject structureObject = (StructureObject)selectableObject;
            OnShowStructureOptions?.Invoke(this, structureObject);
        }
    }
        
    void HideUseObjectOptions(SelectableObject selectableObject)
    {
        if(currentHover ==  selectableObject)
        {

            OnHideOptions?.Invoke();
        }
    }


    private void OnDestroy()
    {
        OnCharacterObjectDestroy?.Invoke(this);
    }
}

