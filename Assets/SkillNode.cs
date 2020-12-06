using ManagementScripts;
using SkillsLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public static event Action<SkillNode> OnNodeHoverEnter;

    public static event Action<SkillNode> OnNodeHoverExit;

    public static event Action<SkillNode> OnNodeClicked;

    public GameObject EditSkillPanel;

    public Text NameText;

    public Text RequirementsText;

    public Text DescriptionText;

    public NodeConnectionPathCreator pathCreator;

    List<string> Requirements = new List<string>();

    string description = "";

    GameManager gameManager;

    public RectTransform nodePathLeft;
    public RectTransform nodePathRight;

    public SkillTreeUI treeUI;


    Image background;

    HoverButton hoverDetect;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        EditSkillPanel.GetComponent<SkillDetailsPanel>();
        hoverDetect = GetComponent<HoverButton>();
        hoverDetect.OnPointerEnterDetected += HoverNodeEnter;
        hoverDetect.OnPointerExitDetected += HoverNodeExit;



        background = GetComponent<Image>();

    }

    public void HoverNodeEnter()
    {
        OnNodeHoverEnter?.Invoke(this);
    }

    public void HoverNodeExit()
    {
        OnNodeHoverExit?.Invoke(this);
    }

    public void NodeClick()
    {
        OnNodeClicked?.Invoke(this);
    }

    public void ClickDetect(SkillNode node)
    {
        if(node == this)
        {
            UpdateNode();
        }
    }


    public void SkillNodeClick(SkillNode node)
    {

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

    public string GetSkillName()
    {
        return NameText.text;
    }

    public void ShowEditSkillButtonClick()
    {
        bool panelActive = EditSkillPanel.activeSelf;
        if (!panelActive)
        {
            EditSkillPanel.SetActive(true);
            ShowSkillDetails();

        }
        else
        {
            if(EditSkillPanel.GetComponent<RequirementSelector>().currentSkillNode == this)
            {
                EditSkillPanel.SetActive(false);
            }
            else
            {
                ShowSkillDetails();
            }
        }               
    }



    public void ShowSkillDetails()
    {
        EditSkillPanel.GetComponent<SkillDetailsPanel>().AssignSkillNode(this);
        
    }

    Vector3 GetPositionTroughRayCast(Vector3 inp)
    {
        Ray ray = Camera.main.ScreenPointToRay(inp);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //draw invisible ray cast/vector
            Debug.DrawLine(ray.origin, hit.point);
            //log hit area to the console
            return hit.point;

        }
        else
        {
            return Vector3.zero;
        }
    }

    public void UpdateNode()
    {
        int index = CurrentIndex();
        int[] requirements = gameManager.skillTree.tree[index].RequiredSkills;
        if (requirements != null)
        {
            //Camera cam = Camera.main;
            //for (int i = 0; i < requirements.Length; i++)
            //{
            //    pathCreator.CreateRequirementPath(GetPositionTroughRayCast(nodePathLeft.anchoredPosition), GetPositionTroughRayCast(treeUI.allNodes[requirements[i]].GetComponent<SkillNode>().nodePathRight.anchoredPosition));
            //}
            UpdateRequirementsTexts();
        }
        DescriptionText.text = gameManager.skillTree.tree[index].Description;
        background.color = background.color + Color.white*1/(gameManager.skillTree.GetHiarchyLevelOfSkill(index)+1)+new Color(0,0,0,1);
    }

    public void SetBaseColor(Color color)
    {
        background.color = color;
    }

    private void UpdateRequirementsTexts()
    {
        string requirementsString = "";
        int index = CurrentIndex();
        int[] requirements = gameManager.skillTree.tree[index].RequiredSkills;
        if (requirements != null)
        {
            for (int i = 0; i < requirements.Length; i++)
            {
                requirementsString += gameManager.skillTree.tree[requirements[i]].Name + "\n";
            }
            
        }
        else
        {
            requirementsString = "Root Skill";
        }
        RequirementsText.text = requirementsString;
    }



    public void AddRequirement(string reqToAdd)
    {
        SkillTree skillTree = gameManager.skillTree;
        if(skillTree.ValidateRequirement(reqToAdd, NameText.text))
        {
            skillTree.AddRequirementToSkill(reqToAdd, NameText.text);
            UpdateNode();
        }           
    }


    public void RemoveRequirement(string reqToRemove)
    {
        SkillTree skillTree = gameManager.skillTree;
        skillTree.RemoveRequirementFromSkill(reqToRemove, NameText.text);
        UpdateNode();
    }

}
