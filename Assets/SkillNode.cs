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

    public List<GameObject> activeDummies;

    public Text NameText;

    public Text RequirementsText;

    public Text DescriptionText;

    public NodeConnectionPathCreator pathCreator;

    List<string> Requirements = new List<string>();

    string description = "";

    SkillTree skillTree;

    public int index { get; private set; }

    public RectTransform nodePathLeft;
    public RectTransform nodePathRight;

    public SkillTreeUI treeUI;



    public Image Background;

    public HoverButton HoverDetect;

    private void Awake()
    {

        //EditSkillPanel.GetComponent<SkillDetailsPanel>();
        //HoverDetect = GetComponent<HoverButton>();
        HoverDetect.OnPointerEnterDetected += HoverNodeEnter;
        HoverDetect.OnPointerExitDetected += HoverNodeExit;

       

        //Background = GetComponent<Image>();

    }

    public void SetSkillTreeAndIndex(SkillTree tree,int ind)
    {
        skillTree = tree;
        index = ind;
        skillTree.OnSkillTreeModified += ResetTree;
        skillTree.OnSkillNameModified += SkillRenameDetected;
        //UpdateNode(index);
    }

    private void SkillRenameDetected(int renamedSkillIndex)
    {
        int[] reqs = skillTree.tree[index].RequiredSkills;
        if(reqs != null && reqs.Length > 0)
        {
            for (int i = 0; i < reqs.Length; i++)
            {
                if (reqs[i] == renamedSkillIndex)
                {
                    UpdateNode(index);
                    
                    return;
                }
            }
        }
    }

    void ResetTree(SkillTree tree)
    {
        skillTree = tree;
        UpdateNode(index);
    }

    internal void UpdateSkillDescription(string edit)
    {

        skillTree.ModifyDescription(index, edit);
        DescriptionText.text = edit;
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
            UpdateNode(index);
        }
    }


    public void SkillNodeClick(SkillNode node)
    {

    }



    public void SetSkillNameText(string skillName)
    {


        NameText.text = skillName;
        gameObject.name = skillName+" Node";
    }

    public void UpdateSkillName(string name)
    {
        skillTree.RenameSkill(skillTree.tree[index].Name, name);
    }

    public string GetSkillName()
    {
        return skillTree.tree[index].Name;
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

    public void UpdateNode(int index)
    {
        SetSkillNameText(skillTree.tree[index].Name);
        UpdateRequirementsTexts();

        DescriptionText.text = skillTree.tree[index].Description;
        Background.color = Background.color + Color.white*1/(skillTree.GetHiarchyLevelOfSkill(index)+1)+new Color(0,0,0,1);
        if (transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void SetBaseColor(Color color)
    {
        Background.color = color;
    }

    private void UpdateRequirementsTexts()
    {
        string requirementsString = "";

        int[] requirements = skillTree.tree[index].RequiredSkills;
        if (requirements != null)
        {
            for (int i = 0; i < requirements.Length; i++)
            {
                requirementsString += skillTree.tree[requirements[i]].Name + "\n";
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

        if(skillTree.ValidateRequirement(reqToAdd, skillTree.tree[index].Name))
        {
            skillTree.AddRequirementToSkill(reqToAdd, skillTree.tree[index].Name);
            UpdateNode(index);
        }           
    }


    public void RemoveRequirement(string reqToRemove)
    {

        skillTree.RemoveRequirementFromSkill(reqToRemove, skillTree.tree[index].Name);
        UpdateNode(index);
    }

}
