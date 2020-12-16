using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InventoryLogic;

public static class UiTopLayerSetter
{
    public static void SetUiObjectUpTop(GameObject gameObject)
    {
        gameObject.transform.SetParent(gameObject.transform.root);
        gameObject.transform.SetSiblingIndex(gameObject.transform.root.childCount-1);
    }
}


[RequireComponent(typeof(Image))]
public class InventorySlotUi : MonoBehaviour, IHoverUi,IPointerEnterHandler, IPointerExitHandler
{
    public event Action<InventorySlotUi> OnSlotUiButtonClicked;
    public event Action<ItemAmount> OnSlotClick;
    public event Action<bool> OnHover;

    [SerializeField]
    private Text amountText;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private ItemDisplayer nameDisplayer;
    [SerializeField]
    private string placeHolderImage;


    ItemAmount currentItemAmount;
    public bool childHover;
    public IHoverUi parentHoverUi;
    bool isChild = false;

    public void SetItemAmount(ItemAmount itemAmount)
    {
        currentItemAmount = itemAmount;
        UpdateImage();
        UpdateAmount(itemAmount.Amount);
        bool[] inp = { true, false, false, false };
        nameDisplayer.SetDetails(itemAmount.itemType, inp);
        nameDisplayer.gameObject.SetActive(false);
    }

    void SetItemDisplayerOnTop()
    {
        UiTopLayerSetter.SetUiObjectUpTop(nameDisplayer.gameObject);
    }

    public void UpdateImage()
    {
        Sprite sprite = Resources.Load<Sprite>(currentItemAmount.itemType.ResourcePath+"_Icon");
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>(placeHolderImage);
        }
        icon.sprite = sprite;
    }

    public void UpdateAmount(int newAmount)
    {
        amountText.text = newAmount.ToString();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isChild)
        {
            parentHoverUi.SetChildHover(true);            
        }
        nameDisplayer.gameObject.SetActive(true);
        OnHover?.Invoke(true);
        //SetItemDisplayerOnTop();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isChild)
        {
            parentHoverUi.SetChildHover(false);
        }
        nameDisplayer.gameObject.SetActive(false);
        OnHover?.Invoke(false);
        //itemDisplayer.transform.SetParent(transform);
    }

    public void PointerClick()
    {
        OnSlotUiButtonClicked?.Invoke(this);
        OnSlotClick?.Invoke(currentItemAmount);
        Debug.Log("Slot Ui Button clicked: " + currentItemAmount.itemType.TypeName);
    }

    public void SetChildHover(bool setting)
    {
        childHover = setting;
    }

    public bool SetAsChild(IHoverUi hoverUi)
    {
        parentHoverUi = hoverUi;
        isChild = true;
        return isChild;
    }




}
