using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillsLogic;
using DataLogic;
using ManagementScripts;

public class SkillCreatorUI : MonoBehaviour
{
    public event Action<string> OnNewSkillCreated;

    public InputField NameInput;

    List<string> allSkillNames = new List<string>();

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void CreateNewSkillNodeFromInputField()
    {
        if(NameInput.text == null || NameInput.text == "")
        {
            return;
        }
        string inputText = DataChecks.EnsureUnique(allSkillNames.ToArray(), NameInput.text);

        Dictionary<string, float> effectors = new Dictionary<string, float>();
        effectors.Add("Test effector 1", .1f);
        effectors.Add("Test effector 2", .2f);

        string description = "No Description";

        Skill skill = new Skill(inputText, description, effectors);

        gameManager.skillTree.AddSkill(skill);
        OnNewSkillCreated?.Invoke(inputText);
    }


}
