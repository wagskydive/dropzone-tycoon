using System.Collections;
using System.Collections.Generic;
using InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGameplayUi : ItemDetailsDisplayer
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text catagoryText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private SpawnAndGrabButton spawnAndGrabButton;

    public override void SetItem(ItemType itemType)
    {
        nameText.text = itemType.TypeName;

        if (itemType.recipe != null)
        {
            string[] ingredients = itemType.recipe.RecipeStrings();
        }

        catagoryText.text = itemType.Catagory;
        descriptionText.text = itemType.Description;


        spawnAndGrabButton.SetSpawnable(new Item(itemType));
        base.SetItem(itemType);
    }
}
