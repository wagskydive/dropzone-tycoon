using ManagementScripts;
using SkillsLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public DropdownHandler dropdownHandler;

    public GameObject SelectRequirementPanel;

    public GameObject EditDescriptionPanel;

    public Text NameText;

    List<string> Requirements = new List<string>();

    string description = "";

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetSkillName(string skillName)
    {
        NameText.text = skillName;
    }

    public void ShowSelectRequirementButtonClick()
    {
        SelectRequirementPanel.SetActive(!SelectRequirementPanel.activeSelf);
        string[] skills = new string[gameManager.allSkills.Count];

        for (int i = 0; i < gameManager.allSkills.Count; i++)
        {
            skills[i] = gameManager.allSkills[i].Name;
        }

        dropdownHandler.PopulateDropDown(skills, NameText.text);
    }

    public void EditDescriptionButtonClick()
    {
        EditDescriptionPanel.SetActive(!EditDescriptionPanel.activeSelf);
    }

    public void AddRequirement()
    {
        string reqToAdd = dropdownHandler.GetSelected();
        if (!DataLogic.DataChecks.CheckForIdExists(Requirements.ToArray(), reqToAdd))
        {
            Requirements.Add(reqToAdd);
        }            
    }
}
