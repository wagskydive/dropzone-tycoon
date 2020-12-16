using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using System;

public class ItemBrowserUi : MonoBehaviour, IHoverUi
{
    [SerializeField]
    private ItemDetailsDisplayer itemDetailsDisplayer;
    [SerializeField]
    private GameObject contentHolder;
    [SerializeField]
    private GameObject catagoryButton;

    public event Action<bool> OnHover;



    void Start()
    {
        ItemsLibrary lib = GameObject.Find("GameManager").GetComponent<ManagementScripts.GameManager>().Library;
        CreateCatagories(lib);
    }

    void CreateCatagories(ItemsLibrary library)
    {
        for (int i = 0; i < library.CatagoryStrings.Length; i++)
        {
            GameObject go = Instantiate(catagoryButton, contentHolder.transform);
            go.SetActive(true);
            go.GetComponent<IHoverUi>().SetAsChild(this);
            
            go.GetComponent<InventoryParent>().Setup(library.allItemsByCatagory[i]);
            go.GetComponent<InventoryParent>().OnSlotClicked += SetDetails;
            //go.SetActive(false);
        }
    }

    void SetDetails(ItemAmount itemAmount)
    {
        itemDetailsDisplayer.SetItem(itemAmount.itemType);
    }

    public void SetChildHover(bool setting)
    {

    }

    public bool SetAsChild(IHoverUi hoverUi)
    {
        return true;
    }
}
