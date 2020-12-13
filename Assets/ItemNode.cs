using ManagementScripts;
using InventoryLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNode : MonoBehaviour
{
    public static event Action<ItemNode> OnNodeHoverEnter;
                               
    public static event Action<ItemNode> OnNodeHoverExit;
                               
    public static event Action<ItemNode> OnNodeClicked;

    public GameObject EditItemPanel;

    public Text NameText;

    public Text RecipeText;

    public Text DescriptionText;


    ItemsLibrary library;

    public int index { get; private set; }

    public RectTransform nodePathLeft;
    public RectTransform nodePathRight;

    Image background;

    public Image icon;

    HoverButton hoverDetect;

    private void Awake()
    {

        EditItemPanel.GetComponent<ItemDetailsPanel>();
        hoverDetect = GetComponent<HoverButton>();
        hoverDetect.OnPointerEnterDetected += HoverNodeEnter;
        hoverDetect.OnPointerExitDetected += HoverNodeExit;



        background = GetComponent<Image>();

    }

    public void SetLibraryAndIndex(ItemsLibrary lib, int ind)
    {
        library = lib;
        index = ind;

    }





    internal void UpdateItemDescription(string edit)
    {

        library.ModifyDescription(index, edit);
        DescriptionText.text = edit;
    }

    public void HoverNodeEnter()
    {
        OnNodeHoverEnter?.Invoke(this);
    }

    public void HoverNodeExit()
    {
        OnNodeHoverExit?.Invoke(this);
    }

    public void NodeClick()
    {
        OnNodeClicked?.Invoke(this);
    }




    public void ItemNodeClick(ItemNode node)
    {

    }



    public void SetItemNameText(string itemName)
    {


        NameText.text = itemName;
        gameObject.name = itemName + " Node";
    }

    public void UpdateItemName(string name)
    {
        
        library.RenameItemType(index, name);
    }

    public string GetItemName()
    {
        return library.allItems[index].TypeName;
    }

    public void ShowEditSkillButtonClick()
    {
        bool panelActive = EditItemPanel.activeSelf;
        if (!panelActive)
        {
            EditItemPanel.SetActive(true);
            ShowItemDetails();

        }
        else
        {
            EditItemPanel.SetActive(false);
        }
    }



    public void ShowItemDetails()
    {
        EditItemPanel.GetComponent<ItemDetailsPanel>().AssignItemNode(this);
        EditItemPanel.SetActive(true);
    }


    public void UpdateNode(int index)
    {
        SetItemNameText(library.allItems[index].TypeName);
        UpdateRecipeTexts();

        DescriptionText.text = library.allItems[index].Description;

        //background.color = background.color + Color.white * 1 / (skillTree.GetHiarchyLevelOfSkill(index) + 1) + new Color(0, 0, 0, 1);
    }

    public void SetBaseColor(Color color)
    {
        background.color = color;
    }

    private void UpdateRecipeTexts()
    {
        string recipeString = "";

        if (library.allItems[index].recipe != null)
        {


            ItemAmount[] ingedients = library.allItems[index].recipe.Ingredients;
            if (ingedients != null)
            {
                for (int i = 0; i < ingedients.Length; i++)
                {
                    recipeString += ingedients[i].amount + ": " + ingedients[i].itemType.TypeName + "\n";
                }

            }
        }
        else
        {
            recipeString = "Root Item";
        }
        RecipeText.text = recipeString;
    }



    //public void AddRequirement(string reqToAdd)
    //{
    //
    //    if (skillTree.ValidateRequirement(reqToAdd, skillTree.tree[index].Name))
    //    {
    //        skillTree.AddRequirementToSkill(reqToAdd, skillTree.tree[index].Name);
    //        UpdateNode(index);
    //    }
    //}
    //
    //
    //public void RemoveRequirement(string reqToRemove)
    //{
    //
    //    skillTree.RemoveRequirementFromSkill(reqToRemove, skillTree.tree[index].Name);
    //    UpdateNode(index);
    //}

}
