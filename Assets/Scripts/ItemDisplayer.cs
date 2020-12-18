using System.Collections;
using System.Collections.Generic;
using InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField]
    public GameObject textTemplate;

    bool DisplayName;
    bool DisplayCatagory;
    bool DisplayDescription;
    bool DisplayResourcsePath;


    public virtual void CreateNewTextObject(string text)
    {

        GameObject go = Instantiate(textTemplate, transform);
        go.GetComponent<ITextDisplayer>().SetText(text);
        go.SetActive(true);
    } 



    public void SetDetails(Item item, bool[] which = null)
    {
        SetDetails(item.itemType, which);
    }

    public void SetDetails(ItemType itemType, bool[] which = null  )
    {
        if(which != null && which.Length == 4)
        {
            DisplayName = which[0];
            DisplayCatagory = which[1];
            DisplayDescription = which[2];
            DisplayResourcsePath = which[3];
        }
        SetDetailsTexts(itemType);
        textTemplate.SetActive(false);
    }

    public void SetDetailsTexts(ItemType itemType)
    {
        if (DisplayName)
        {
            CreateNewTextObject(itemType.TypeName);
        }
        if (DisplayCatagory)
        {
            CreateNewTextObject(itemType.Catagory);
        }       
        if (DisplayDescription)
        {
            CreateNewTextObject(itemType.Description);
        }
        if (DisplayResourcsePath)
        {
            CreateNewTextObject(itemType.ResourcePath);
        }               
    }
}


