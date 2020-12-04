using ManagementScripts;
using SkillsLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementSelector : Editable
{
    public event Action<SkillNode> OnConfirmButtonClick;
    public override event Action<string> OnEdited;

    public DropdownHandler dropdownHandler;
    public Text requirementSelectorName;

    GameManager gameManager;


    public SkillNode currentSkillNode;
    private string currentSkill;



    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void AssignNode(string skillName, SkillNode skillNode)
    {
        currentSkill = skillName;
        currentSkillNode = skillNode;
        requirementSelectorName.text = skillName;
        SetDropDownOptions(gameManager.skillTree);
    }


    public void ClickConfirmButton()
    {
        gameManager.skillTree.AddRequirementToSkill(dropdownHandler.GetSelected(), currentSkill);
        //currentSkillNode.AddRequirement(dropdownHandler.GetSelected());
        AssignNode(currentSkill, currentSkillNode);
        OnConfirmButtonClick?.Invoke(currentSkillNode);
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

    private void SetDropDownOptions(SkillTree skillTree)
    {
        List<string> skills = new List<string>();

        int[] validOptions = skillTree.ValidRequirememts(currentSkill);

        for (int i = 0; i < validOptions.Length; i++)
        {
            skills.Add(skillTree.tree[validOptions[i]].Name);

        }

        dropdownHandler.PopulateDropDown(skills.ToArray(), currentSkill);
    }

}
