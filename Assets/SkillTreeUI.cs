using System.Collections;
using System.Collections.Generic;
using SkillsLogic;
using UnityEngine;
using UnityEngine.UI;
using ManagementScripts;

public class SkillTreeUI : MonoBehaviour
{
    public GameObject columnPrefab;
    List<GameObject> allColumns = new List<GameObject>();

    public GameObject SkillNodePrefab;
    public List<GameObject> allNodes = new List<GameObject>();

    GameManager gameManager;

    SkillNode selectedNode = null;

    public Color NormalColor;

    public Color UpstreamColor;

    public Color DownstreamColor;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //gameManager.OnOldSkillWillBeDestroyed += UnSubscribeToNewTree;
        gameManager.OnNewSkillCreated += SubscribeToNewTree;

        SkillNode.OnNodeHoverEnter += SelectNode;
        SkillNode.OnNodeHoverExit += DeselectNode;

        SkillNode.OnNodeClicked += NodeClicked;

    }

    void UnSubscribeToNewTree(SkillTree skillTree)
    {
        skillTree.OnSkillTreeModified -= RefreshTree;
    }


    void SubscribeToNewTree(SkillTree skillTree)
    {
        skillTree.OnSkillTreeModified += RefreshTree;
        RefreshTree(skillTree);
    }

    void DestroyAllNodes()
    {
        if (allNodes.Count > 0)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                Destroy(allNodes[i]);
            }
        }
        allNodes = new List<GameObject>();
    }

    public void NodeClicked(SkillNode node)
    {
        //int col = node.transform.parent.GetSiblingIndex();
        //float per = col/ node.transform.childCount;
        //
        //RectTransform rectTransform = GetComponent<RectTransform>();
        //
        //rectTransform.position = new Vector3(rectTransform.rect.width * per, rectTransform.position.y, rectTransform.position.z) ;
    }

    Vector3 previousMousePosition;


    [Range(0.1f, 1.5f)]
    float scalingDelta;

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 newPosition = Input.mousePosition;
            if (Input.GetMouseButtonDown(2))
            {
                previousMousePosition = newPosition;
            }
            transform.Translate((previousMousePosition - newPosition)*-1);
            previousMousePosition = newPosition;
        }      
        scalingDelta = Input.mouseScrollDelta.y / 5;


        Vector3 newScale = transform.localScale + new Vector3(scalingDelta, scalingDelta, 1);
        if(newScale.x > .1f && newScale.x < 2f)
        {
            transform.localScale = newScale;
        }

    }




    public void RefreshTree(SkillTree skillTree)
    {
        Skill[] tree = skillTree.tree;

        int maxLevel = skillTree.GetMaxHiarchyLevelOfTree();

        for (int j = 0; j < maxLevel+1; j++)
        {
            if (j + 1 > allColumns.Count)
            {
                GameObject col = Instantiate(columnPrefab, transform);
                col.SetActive(true);
                allColumns.Add(col);
                
            }
        }



        if (tree.Length > allNodes.Count)
        {
            allNodes.AddRange(new GameObject[tree.Length- allNodes.Count]);
        }

        for (int i = 0; i < tree.Length; i++)
        {
            if(allNodes[i] == null)
            {
                allNodes[i] = InstantiateNodeObject(tree[i].Name);
            }
            else if(allNodes[i].GetComponent<SkillNode>().GetSkillName() != tree[i].Name)
            {
                allNodes[i].GetComponent<SkillNode>().SetSkillName(tree[i].Name);
                
            }
            allNodes[i].SetActive(true);
            SkillNode skillNode = allNodes[i].GetComponent<SkillNode>();
            skillNode.UpdateNode();
            
            allNodes[i].transform.SetParent(allColumns[skillTree.GetHiarchyLevelOfSkill(tree[i].Name)].transform);

        }

    }

    public void SelectNode(SkillNode node)
    {
        if(selectedNode != null)
        {
            DeselectNode(selectedNode);
        }

        SkillTree skillTree = gameManager.skillTree;

        selectedNode = node;
        int skillIndex = skillTree.FindIndexOfSkillByNameInSkillArray(node.GetSkillName());

        int[] upStream = skillTree.GetAllUpstreamSkills(skillIndex);
        if(upStream != null)
        {
            for (int i = 0; i < upStream.Length; i++)
            {
                allNodes[upStream[i]].GetComponent<SkillNode>().SetBaseColor(UpstreamColor);
            }
        }


        int[] downStream = skillTree.GetAllDownstreamSkills(skillIndex);
        if (downStream != null)
        {
            for (int i = 0; i < downStream.Length; i++)
            {
                allNodes[downStream[i]].GetComponent<SkillNode>().SetBaseColor(DownstreamColor);
            }
        }

    }

    public void ResetAllColors()
    {
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].GetComponent<SkillNode>().SetBaseColor(NormalColor);
        }
    }

    public void DeselectNode(SkillNode skillNode)
    {
        
        ResetAllColors();
        selectedNode = null;
    }


    private GameObject InstantiateNodeObject(string inputText)
    {
        GameObject node = Instantiate(SkillNodePrefab);

        node.SetActive(true);

        node.GetComponent<SkillNode>().SetSkillName(inputText);


        return node;
    }
}
