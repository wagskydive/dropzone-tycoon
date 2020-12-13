using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetailsPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public ItemNode node;

    public Editable Name;

    public Editable Description;

    public Editable Recipe;

    public Editable Effectors;


    public void AssignItemNode(ItemNode item)
    {


        node = item;

        Name.SetDisplaytext(item.NameText.text);
        Name.OnEdited += HandleNameEdit;

        Description.SetDisplaytext(item.DescriptionText.text);
        Description.OnEdited += HandleDescriptionEdit;


        Recipe.SetDisplaytext(item.RecipeText.text);
        

        Recipe.OnEdited += HandleRecipeEdit;



        //Effectors.SetDisplaytext(skill.Effe)


    }

    void HandleNameEdit(string edit)
    {
        node.UpdateItemName(edit);
        AssignItemNode(node);
    }

    void HandleDescriptionEdit(string edit)
    {
        node.UpdateItemDescription(edit);
        AssignItemNode(node);
    }

    void HandleRecipeEdit(string edit)
    {
        if (Recipe.addMode)
        {
            //node.AddRequirement(edit);
        }
        else
        {
            //node.RemoveRequirement(edit);
        }
        AssignItemNode(node);

    }

}
