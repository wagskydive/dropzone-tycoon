using InventoryLogic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class ItemObject : SelectableObject
{
    public event Action<ItemObject> OnItemObjectSelected;
    public event Action<ItemObject> OnItemObjectDestroy;
    
    public Item item;



    public void SetupItemInstance(Item it)
    {
        item = it;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        Quaternion originalRotation = transform.rotation;
        Vector3 originalPosition = transform.position;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        //transform.GetChild(0).localRotation;
        //transform.GetChild(0).localRotation = Quaternion.identity;

        Bounds bounds =  ColliderAdder.AddMeshCollidersInChildren(gameObject);
        boxCollider.size = bounds.size;

        //transform.GetChild(0).Translate((transform.position-bounds.min)-new Vector3(bounds.size.x,0,bounds.size.z));
        boxCollider.center = bounds.center; // (bounds.center - bounds.min) - new Vector3(bounds.size.x, 0, bounds.size.z);
        boxCollider.isTrigger = true;

        if (item.itemType.IsObstacle)
        {
            NavMeshObstacle navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
            navMeshObstacle.carving = true;
            navMeshObstacle.center = boxCollider.center;
            navMeshObstacle.size = boxCollider.size;
        }


        transform.rotation = originalRotation;
        transform.position = originalPosition;
    }

    public void SetRigidBodyActive(bool active)
    {
        
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = !active;

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

    public override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
    }
}

