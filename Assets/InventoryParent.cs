using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using InventoryLogic;
using System;

public class InventoryParent : MonoBehaviour, IHoverUi, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<bool> OnHover;
    public event Action<ItemAmount> OnSlotClicked;



    [SerializeField]
    private GameObject contentParent;
    [SerializeField]
    private GameObject ItemSlotPrefab;

    [SerializeField]
    public string resourcePath;

    public Text DirectoryName;

    public bool childHover;
    public IHoverUi parentHoverUi;
    public bool MouseIsOver;

    bool isChild = false;

    bool isLoaded = false;

    ItemType[] itemTypes;



    private Vector3 contentParentOriginalPosition;

    Vector3 lastMousePosition;

    


    private void Awake()
    {
        contentParentOriginalPosition = contentParent.transform.localPosition;
    }

    public void Setup(ItemType[] types)
    {
        itemTypes = types;
        CreateChildren();
        DirectoryName.text = types[0].Catagory;
    }

    public void CreateChildren()
    {
        for (int i = 0; i < itemTypes.Length; i++)
        {
            CreateChild(itemTypes[i]);

        }
    }

    public void CreateChild(ItemType itemType)
    {
        GameObject child = Instantiate(ItemSlotPrefab, contentParent.transform);
        child.GetComponent<IHoverUi>().SetAsChild(this);
        child.GetComponent<InventorySlotUi>().SetItemAmount(new ItemAmount(itemType,0));
        child.GetComponent<InventorySlotUi>().OnSlotClick += SlotClicked;
        //child.GetComponent<IHoverUi>().SetupAsChild(childLevelPath, extentionFilter);
    }


    void SlotClicked(ItemAmount itemAmount)
    {
        OnSlotClicked?.Invoke(itemAmount);
    }

    private void Update()
    {
        if (contentParent.activeSelf)
        {
            if (Input.GetMouseButtonDown(2))
            {
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                contentParent.transform.position += (Vector3.up * (Input.mousePosition.y - lastMousePosition.y));
                lastMousePosition = Input.mousePosition;
            }
        }

    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseIsOver = true;
        contentParent.SetActive(true);
        contentParent.transform.localPosition = contentParentOriginalPosition;
        if (isChild)
        {
            parentHoverUi.SetChildHover(true);
        }
        OnHover?.Invoke(true);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        MouseIsOver = false;
        if (isChild)
        {
            parentHoverUi.SetChildHover(false);
        }
        //if (!childHover)
        //{
        //    contentParent.SetActive(false);
        //}
        contentParent.SetActive(false);
        OnHover?.Invoke(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        
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
