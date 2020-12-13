using InventoryLogic;
using System;
using UnityEngine;

public class ItemObject : SelectableObject
{
    public event Action<ItemObject> OnItemObjectSelected;
    public event Action<ItemObject> OnItemObjectDestroy;

    public Item item;

    public override void SelectObject()
    {
        base.SelectObject();
        OnItemObjectSelected?.Invoke(this);
    }
    private void OnDestroy()
    {
        OnItemObjectDestroy?.Invoke(this);
    }
}

