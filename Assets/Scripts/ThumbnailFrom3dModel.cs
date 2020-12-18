using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryLogic;


public class ThumbnailFrom3dModel : MonoBehaviour
{




    [SerializeField]
    private Icon3dHandler icon3dHandler;

    [SerializeField]
    public ItemDetailsDisplayer itemDetailsDisplayer;

    [SerializeField]
    public IconObject iconObject;

    private void Start()
    {

        itemDetailsDisplayer.OnNewItemTypeDetailsDisplayed += SetItem;

    }

    void SetItem(ItemType itemType)
    {
        iconObject.SetItem(itemType);
    }


}
