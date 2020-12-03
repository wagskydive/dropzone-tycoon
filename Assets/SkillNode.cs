using ManagementScripts;
using SkillsLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public GameObject SelectRequirementPanel;

    public GameObject EditDescriptionPanel;

    public Text NameText;

    public Text RequirementsText;

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
        gameObject.name = skillName+" Node";
    }

    public void ShowSelectRequirementButtonClick()
    {
        bool panelActive = SelectRequirementPanel.activeSelf;
        if (!panelActive)
        {
            SelectRequirementPanel.SetActive(true);
            SelectRequirementPanel.GetComponent<RequirementSelector>().AssignNode(NameText.text, this);
        }
        else
        {
            SelectRequirementPanel.SetActive(false);
        }
               
    }

    void SetRequirementsText()
    {
        string requirementsString = "";
        int index = gameManager.allSkills.FindIndex(x => x.Name == NameText.text);

        for (int i = 0; i < gameManager.allSkills[index].RequiredSkills.Length; i++)
        {
            requirementsString += gameManager.allSkills[gameManager.allSkills[index].RequiredSkills[i]].Name + "\n";
        }
        RequirementsText.text = requirementsString;
    }

    public void EditDescriptionButtonClick()
    {
        EditDescriptionPanel.SetActive(!EditDescriptionPanel.activeSelf);
    }

    public void AddRequirement(string reqToAdd)
    {
        if(SkillTreeDataHandler.ValidateRequirement(gameManager.allSkills.ToArray(),reqToAdd, NameText.text))
        {
            Requirements.Add(reqToAdd);
            int index = gameManager.allSkills.FindIndex(x => x.Name == NameText.text);


            if(gameManager.allSkills[index].RequiredSkills != null && gameManager.allSkills[index].RequiredSkills.Length > 0)
            {
                int[] reqIndexes = new int[gameManager.allSkills[index].RequiredSkills.Length + 1];
                for (int i = 0; i < reqIndexes.Length-1; i++)
                {
                    reqIndexes[i] = gameManager.allSkills[index].RequiredSkills[i];
                }
                reqIndexes[reqIndexes.Length - 1] = gameManager.allSkills.FindIndex(x => x.Name == reqToAdd);
                gameManager.allSkills[index].SetRequieredSkills(reqIndexes);
            }
            else
            {                
                int[] reqIndexes = new int[1];
                reqIndexes[0] = gameManager.allSkills.FindIndex(x => x.Name == reqToAdd);

                gameManager.allSkills[index].SetRequieredSkills(reqIndexes);
            }

            SetRequirementsText();
        }


        //string reqToAdd = SelectRequirementPanel.GetComponent<RequirementSelector>().dropdownHandler.GetSelected();
        //if (!DataLogic.DataChecks.CheckForStringExists(Requirements.ToArray(), reqToAdd))
        //{
        //    Requirements.Add(reqToAdd);
        //    int index = gameManager.allSkills.FindIndex(x => x.Name == NameText.text);
        //    gameManager.allSkills[index].SetRequieredSkills(Requirements.ToArray());
        //    SetRequirementsText();
        //}            
    }
}
