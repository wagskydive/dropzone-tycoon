using InventoryLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconRender : MonoBehaviour
{

    [SerializeField]
    private IconObject iconObject;

    public void SetIcon(ItemType itemType)
    {
        iconObject.SetItem(itemType);
    }
}
