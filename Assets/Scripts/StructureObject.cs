using InventoryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureObject : SelectableObject
{
    public static event Action<StructureObject> OnStructureSelected;
    public event Action<GridPosition > OnStructurePartSelected;

    [SerializeField]
    public Structure structure { get; private set; }

    ItemSpawner currentItemSpawner;

    BoxCollider boxCollider;

    public override void Awake()
    {
        base.Awake();
        FindSeats();
    }

    void FindSeats()
    {
        SelectableObject[] childObjects = GetComponentsInChildren<SelectableObject>();
        List<Transform> seatList = new List<Transform>();
        for (int i = 0; i < childObjects.Length; i++)
        {
            if (childObjects[i].seats.Any())
            {
                foreach (var seat in childObjects[i].seats)
                {
                    seatList.Add(seat);
                }
            }
        }
        seats = seatList.ToArray();
    }

    void SetColliderToChildrenBounds()
    {
        Bounds bounds = BoundsMagic.CreateBoundsFromGameObject(gameObject, transform);
        boxCollider.size = bounds.size;
        boxCollider.center = bounds.center;
    }

    public void SetNewStructure(Structure str)
    {
        structure = str;

        SetChildrenSelectable(false);
        if (boxCollider == null)
        {
            boxCollider = gameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }
            boxCollider.isTrigger = true;
        }
    }

    bool partsSubscribed;

    void SubscriberToPartSelection()
    {
        SelectableObject[] selectableObjects = GetComponentsInChildren<SelectableObject>();

        if (selectableObjects != null)
        {
            for (int i = 0; i < selectableObjects.Length; i++)
            {
                if (selectableObjects[i] != this)
                {
                    selectableObjects[i].OnClicked += StructurePartSelected;

                }
            }
        }

        partsSubscribed = true;

    }

    public ItemObject ItemObjectFromWallPlacement(WallPlacement wallPlacement)
    {
        ItemObject[] itemObjects = GetComponentsInChildren<ItemObject>();
        Item item = structure.ItemFromWallPlacement(wallPlacement);
        if (item != null)
        {
            for (int i = 0; i < itemObjects.Length; i++)
            {
                if(itemObjects[i].item ==item)
                {
                    return itemObjects[i];
                }
            }
        }
        return null;
    }

    public void AddItemToStructure(ItemObject itemObject)
    {
        itemObject.transform.SetParent(transform);
        //structure.AddPart(itemObject.item);        
        
        SetColliderToChildrenBounds();
    }

    Item selectedPart;

    public Vector3 PositionFromGridPosition(GridPosition gridPosition, int floor)
    {
        return transform.position + new Vector3(gridPosition.X, (float)floor, gridPosition.Y);
    }

    void StructurePartSelected(SelectableObject selectableObject)
    {
        if (selectableObject.GetType() == typeof(ItemObject))
        {
            ItemObject itemObject = (ItemObject)selectableObject;
            selectedPart = itemObject.item;
            GridPosition gridPosition = structure.FindPartPosition(itemObject.item);
            if (gridPosition != null)
            {
                OnStructurePartSelected?.Invoke(gridPosition);
            }
        }

    }

    public void AssignSpawner(ItemSpawner spawner)
    {
        currentItemSpawner = spawner;
        spawner.OnItemSpawned += AddItemToStructure;
    }

    public void SetChildrenSelectable(bool selectable)
    {
        SelectableObject[] selectableObjects = GetComponentsInChildren<SelectableObject>();

        for (int i = 0; i < selectableObjects.Length; i++)
        {
            if (selectableObjects[i] != this)
            {
                selectableObjects[i].SetSelectable(selectable);

            }
        }
        if (boxCollider != null)
        {

            boxCollider.enabled = !selectable;
        }
    }




    public override void SelectObject()
    {
        if (!partsSubscribed)
        {
            SubscriberToPartSelection();
        }
        base.SelectObject();
        isCurrentlySelectable = false;
        SetChildrenSelectable(true);


        OnStructureSelected?.Invoke(this);
    }
    public override void DeselectObject()
    {
        base.DeselectObject();
        if (currentItemSpawner != null)
        {
            currentItemSpawner.OnItemSpawned -= AddItemToStructure;
        }
        SetSelectable(true);
        SetChildrenSelectable(false);

    }

}
