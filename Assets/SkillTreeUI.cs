using System.Collections;
using System.Collections.Generic;
using SkillsLogic;
using UnityEngine;
using ManagementScripts;

public class SkillTreeUI : MonoBehaviour
{
    public GameObject columnPrefab;
    List<GameObject> allColumns = new List<GameObject>();

    public GameObject SkillNodePrefab;
    public List<GameObject> allNodes = new List<GameObject>();

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //gameManager.OnOldSkillWillBeDestroyed += UnSubscribeToNewTree;
        gameManager.OnNewSkillCreated += SubscribeToNewTree;

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
            allNodes[i].GetComponent<SkillNode>().UpdateNode();
            allNodes[i].transform.SetParent(allColumns[skillTree.GetHiarchyLevelOfSkill(tree[i].Name)].transform);

        }



    }


    private GameObject InstantiateNodeObject(string inputText)
    {
        GameObject node = Instantiate(SkillNodePrefab);

        node.SetActive(true);

        node.GetComponent<SkillNode>().SetSkillName(inputText);


        return node;
    }
}
