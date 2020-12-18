using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using System;

public class ItemDetailsDisplayer : MonoBehaviour
{
    public event Action<Item> OnNewItemDetailsDisplayed;
    public event Action<ItemType> OnNewItemTypeDetailsDisplayed;
    Item currentItem;



    public virtual void SetItem(Item item)
    {
        SetItem(item.itemType);
        OnNewItemDetailsDisplayed?.Invoke(item);

    }
    public virtual void SetItem(ItemType itemType)
    {       
        OnNewItemTypeDetailsDisplayed?.Invoke(itemType);
    }

}
