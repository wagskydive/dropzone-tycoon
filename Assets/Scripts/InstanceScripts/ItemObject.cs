using InventoryLogic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class ItemObject : SelectableObject
{
    public event Action<ItemObject> OnItemObjectSelected;
    public event Action<ItemObject> OnItemObjectDestroy;
    
    public Item item;

    private void Awake()
    {
        SetupItemInstance();
    }

    private void SetupItemInstance()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();


       
        Bounds bounds =  ColliderAdder.AddMeshCollidersInChildren(gameObject);
        boxCollider.size = bounds.size;
        transform.GetChild(0).localRotation = Quaternion.identity;
        //transform.GetChild(0).Translate((transform.position-bounds.min)-new Vector3(bounds.size.x,0,bounds.size.z));
        boxCollider.center = (bounds.center - bounds.min) - new Vector3(bounds.size.x, 0, bounds.size.z);
        boxCollider.isTrigger = true;

        NavMeshObstacle navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        navMeshObstacle.carving = true;
        navMeshObstacle.center = boxCollider.center;
        navMeshObstacle.size = boxCollider.size;
        
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

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

