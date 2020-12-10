using InventoryLogic;
using ManagementScripts;
using SkillsLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeEditor : Editable
{
    public event Action<ItemNode> OnConfirmButtonClick;
    public override event Action<string> OnEdited;

    public DropdownHandler dropdownHandler;
    public Text recipeEditorText;

    public InputField amountInput;

    GameManager gameManager;


    public ItemNode currentItemNode;
    private string currentItem;

    int currentAmount = 1;
    int currentIndex;

    public void ValidateAmount(string amountString)
    {
        int output;
        Int32.TryParse(amountString, out output);
        if(output < 1)
        {
            output = 1;
        }
        currentAmount = output;
        amountInput.text = currentAmount.ToString();
    }

    //public override bool addMode { get; private set; }

    public void AddButtonClick()
    {
        SetAddMode(true);
        SetDropDownOptions(gameManager.Library);
        EnableEditMode();
    }

    public void RemoveButonClick()
    {
        SetAddMode(false);
        SetDropDownOptions(gameManager.Library);
        EnableEditMode();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        
 
    }

    public override void AssignItemNode(string itemName, ItemNode ItemNode)
    {
        currentItem = itemName;
        currentIndex = gameManager.Library.IndexFromTypeName(itemName);
        currentItemNode = ItemNode;
        recipeEditorText.text = RecipeEditorStringBuilder();
        amountInput.text = "1";
        currentAmount = 1;

    }

    string RecipeEditorStringBuilder()
    {
        if (addMode)
        {
            return "Add ingredient:";
        }
        else
        {
            return "Remove ingredient:";
        }
    }


    public void ClickConfirmButton()
    {
        ItemsLibrary library = gameManager.Library;
        if (addMode)
        {
            ItemType type = library.allItems[library.IndexFromTypeName(dropdownHandler.GetSelected())];
            ItemAmount itemAmount = new ItemAmount(type, currentAmount);
            library.AddItemsToRecipe(itemAmount, currentIndex);

        }
        else
        {
            //gameManager.skillTree.RemoveRequirementFromSkill(dropdownHandler.GetSelected(), currentItem);

        }
        //currentSkillNode.AddRequirement(dropdownHandler.GetSelected());
        AssignItemNode(currentItem, currentItemNode);
        OnConfirmButtonClick?.Invoke(currentItemNode);
        EditConfirmed();
    }

    public override void EditConfirmed()
    {
        DisableEditMode();

        OnEdited?.Invoke(dropdownHandler.GetSelected());
    }

    public override void SetDisplaytext(string text)
    {
        base.SetDisplaytext(text);
    }

    private void SetDropDownOptions(ItemsLibrary library)
    {
        if (addMode)
        {


            int[] validOptions = library.ValidIngredients(currentIndex);
            if (validOptions != null)
            {
                List<string> ingredients = new List<string>();
                for (int i = 0; i < validOptions.Length; i++)
                {
                    ingredients.Add(library.allItems[validOptions[i]].TypeName);

                }

                dropdownHandler.PopulateDropDown(ingredients.ToArray(), currentItem);
            }


        }
        else
        {
            
            int[] ingredientIndeces = library.ItemTypesInRecipe(currentItem);
            if (ingredientIndeces != null)
            {
                string[] ingredientTypes = new string[ingredientIndeces.Length];
                for (int i = 0; i < ingredientIndeces.Length; i++)
                {
                    ingredientTypes[i] = library.allItems[ingredientIndeces[i]].TypeName;
                }
                dropdownHandler.PopulateDropDown(ingredientTypes, "");
            }

        }

    }

}
