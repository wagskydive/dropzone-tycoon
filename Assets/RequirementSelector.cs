using ManagementScripts;
using SkillsLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementSelector : MonoBehaviour
{
    public DropdownHandler dropdownHandler;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dropdownHandler = GetComponentInChildren<DropdownHandler>();
    }

    public SkillNode currentSkillNode;
    private string currentSkill;

    public void AssignNode(string skillName, SkillNode skillNode)
    {
        currentSkill = skillName;
        currentSkillNode = skillNode;
        
        SetDropDownOptions(gameManager.allSkills);
    }


    public void ClickConfirmButton()
    {
        currentSkillNode.AddRequirement(dropdownHandler.GetSelected());
    }


    private void SetDropDownOptions(List<Skill> allSkills)
    {
        string[] skills = new string[allSkills.Count];

        for (int i = 0; i < allSkills.Count; i++)
        {
            skills[i] = allSkills[i].Name;
        }

        dropdownHandler.PopulateDropDown(skills, currentSkill);
    }

}
