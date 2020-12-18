using ManagementScripts;
using SkillsLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementSelector : Editable
{
    public event Action<SkillNodeRuntimeOld> OnConfirmButtonClick;
    public override event Action<string> OnEdited;

    public DropdownHandler dropdownHandler;
    public Text requirementSelectorText;

    GameManager gameManager;


    public SkillNodeRuntimeOld currentSkillNode;
    private string currentSkill;

    //public override bool addMode { get; private set; }

    public void AddButtonClick()
    {
        SetAddMode(true);
        SetDropDownOptions(gameManager.skillTree);
        EnableEditMode();
    }

    public void RemoveButonClick()
    {
        SetAddMode(false);
        SetDropDownOptions(gameManager.skillTree);
        EnableEditMode();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void AssignSkillNode(string skillName, SkillNodeRuntimeOld skillNode)
    {
        currentSkill = skillName;
        currentSkillNode = skillNode;
        requirementSelectorText.text = RequirementSelectorStringBuilder();
        
    }

    string RequirementSelectorStringBuilder()
    {
        if (addMode)
        {
            return "Add requirement:";
        }
        else
        {
            return "Remove requirement:";
        }
    }


    public void ClickConfirmButton()
    {
        if (addMode)
        {
            gameManager.skillTree.AddRequirementToSkill(dropdownHandler.GetSelected(), currentSkill);

        }
        else
        {
            gameManager.skillTree.RemoveRequirementFromSkill(dropdownHandler.GetSelected(), currentSkill);

        }
        //currentSkillNode.AddRequirement(dropdownHandler.GetSelected());
        AssignSkillNode(currentSkill, currentSkillNode);
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
        if (addMode)
        {
            

            int[] validOptions = skillTree.ValidRequirememts(currentSkill);
            if(validOptions != null)
            {
                List<string> skills = new List<string>();
                for (int i = 0; i < validOptions.Length; i++)
                {
                    skills.Add(skillTree.tree[validOptions[i]].Name);

                }

                dropdownHandler.PopulateDropDown(skills.ToArray(), currentSkill);
            }


        }
        else
        {
            int[] reqInts = skillTree.tree[skillTree.FindIndexOfSkillByNameInSkillArray(currentSkill)].RequiredSkills;
            if(reqInts != null)
            {
                string[] requirements = new string[reqInts.Length];
                for (int i = 0; i < reqInts.Length; i++)
                {
                    requirements[i] = skillTree.tree[reqInts[i]].Name;
                }
                dropdownHandler.PopulateDropDown(requirements, "");
            }

        }

    }

}
