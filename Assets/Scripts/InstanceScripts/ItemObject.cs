using InventoryLogic;
using System;
using UnityEngine;

public class ItemObject : SelectableObject
{
    public event Action<ItemObject> OnItemObjectSelected;
    public event Action<ItemObject> OnItemObjectDestroy;

    public Item item;

    private void Awake()
    {
        AddSnapCollider();
    }

    private void AddSnapCollider()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

       
        Bounds bounds =  ColliderAdder.AddMeshCollidersInChildren(gameObject);
        boxCollider.size = bounds.size;
        
        transform.GetChild(0).Translate(-bounds.min);
        boxCollider.center = bounds.center - bounds.min;
    }

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

