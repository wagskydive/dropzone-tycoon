using UnityEngine;
using CharacterLogic;
using System;
using UnityEngine.EventSystems;

public class CharacterObject : SelectableObject
{
    public event Action<CharacterObject> OnCharacterObjectSelected;
    public event Action<CharacterObject> OnCharacterObjectDestroy;

    public Character character;

    public override void SelectObject()
    {
        base.SelectObject();
        OnCharacterObjectSelected?.Invoke(this);
        
    }

    private void OnDestroy()
    {
        OnCharacterObjectDestroy?.Invoke(this);
    }
}

