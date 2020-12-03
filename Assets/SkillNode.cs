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
        SelectRequirementPanel.GetComponent<RequirementSelector>().OnConfirmButtonClick += ClickDetect;


    }

    public void ClickDetect(SkillNode node)
    {
        if(node == this)
        {
            SetRequirementsText();
        }
    }

    public int CurrentIndex()
    {
        return gameManager.skillTree.FindIndexOfSkillByNameInSkillArray(NameText.text);
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
            ShowSelectRequirement();

        }
        else
        {
            if(SelectRequirementPanel.GetComponent<RequirementSelector>().currentSkillNode == this)
            {
                SelectRequirementPanel.SetActive(false);
            }
            else
            {
                ShowSelectRequirement();
            }
        }               
    }



    public void ShowSelectRequirement()
    {
        SkillTree skillTree = gameManager.skillTree;


        int[] reqs = skillTree.tree[skillTree.FindIndexOfSkillByNameInSkillArray(NameText.text)].RequiredSkills;
        if(reqs != null)
        {
            Requirements = new List<string>();
            for (int i = 0; i < reqs.Length; i++)
            {
                Requirements.Add(skillTree.tree[reqs[i]].Name);
            }
        }

            
           
        SelectRequirementPanel.GetComponent<RequirementSelector>().AssignNode(NameText.text, this);
        
    }

    void SetRequirementsText()
    {
        string requirementsString = "";
        int index = CurrentIndex();
        int[] requirements = gameManager.skillTree.tree[index].RequiredSkills;
        if(requirements != null)
        {
            for (int i = 0; i < requirements.Length; i++)
            {
                requirementsString += gameManager.skillTree.tree[requirements[i]].Name + "\n";
            }
            RequirementsText.text = requirementsString;
        }

    }

    public void EditDescriptionButtonClick()
    {
        EditDescriptionPanel.SetActive(!EditDescriptionPanel.activeSelf);
    }

    public void AddRequirement(string reqToAdd)
    {
        SkillTree skillTree = gameManager.skillTree;
        if(skillTree.ValidateRequirement(reqToAdd, NameText.text))
        {
            Requirements.Add(reqToAdd);
            SetRequirementsText();
        }           
    }
}
