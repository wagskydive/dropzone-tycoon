using System;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;

public class InventoryUi : MonoBehaviour
{
    public event Action<Inventory> OnInventoryChanged;

    [SerializeField]
    private GameObject inventorySlotUiPrefab;

    [SerializeField]
    private Transform slotParent;

    public void LoadInventory(Inventory inventory)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject go = Instantiate(inventorySlotUiPrefab, slotParent);
            go.GetComponent<InventorySlotUi>().SetItemAmount(inventory.items[i]);
        }
    }
}
