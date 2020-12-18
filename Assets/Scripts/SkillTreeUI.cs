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

    public GameObject branchPrefab;

    public GameObject SkillNodePrefab;
    public List<GameObject> allNodes = new List<GameObject>();

    GameManager gameManager;

    SkillNodeRuntimeOld selectedNode = null;

    public Color NormalColor;

    public Color UpstreamColor;

    public Color DownstreamColor;

    public GameObject dummy;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //gameManager.OnOldSkillWillBeDestroyed += UnSubscribeToNewTree;
        gameManager.OnNewSkillTreeCreated += SubscribeToNewTree;


        SkillNodeRuntimeOld.OnNodeHoverEnter += SelectNode;
        SkillNodeRuntimeOld.OnNodeHoverExit += DeselectNode;

        SkillNodeRuntimeOld.OnNodeClicked += NodeClicked;

        dummy = new GameObject("dummy");
        dummy.AddComponent<RectTransform>().sizeDelta = SkillNodePrefab.GetComponent<RectTransform>().sizeDelta;
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

    public void NodeClicked(SkillNodeRuntimeOld node)
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
            transform.Translate((previousMousePosition - newPosition) * -1);
            previousMousePosition = newPosition;
        }
        scalingDelta = Input.mouseScrollDelta.y / 5;


        Vector3 newScale = transform.localScale + new Vector3(scalingDelta, scalingDelta, 1);
        if (newScale.x > .1f && newScale.x < 2f)
        {
            transform.localScale = newScale;
        }

    }




    public void RefreshTree(SkillTree skillTree)
    {
        Skill[] tree = skillTree.tree;

        //int maxLevel = skillTree.GetMaxHiarchyLevelOfTree();
        //
        //for (int j = 0; j < maxLevel + 1; j++)
        //{
        //    if (j + 1 > allColumns.Count)
        //    {
        //        GameObject col = Instantiate(columnPrefab, transform);
        //        col.SetActive(true);
        //        allColumns.Add(col);
        //
        //    }
        //}



        if (tree.Length > allNodes.Count)
        {
            allNodes.AddRange(new GameObject[tree.Length - allNodes.Count]);
        }





        MakeBranches(skillTree, 0).transform.SetParent(transform);

        for (int i = 0; i < tree.Length; i++)
        {
            if(allNodes[i] == null)
            {
                MakeBranches(skillTree, i).transform.SetParent(transform);
                //MakeBranches(skillTree, i);
            }

            //allNodes[i].transform.SetParent(transform);
            //allNodes[i].transform.SetParent(allColumns[skillTree.GetHiarchyLevelOfSkill(tree[i].Name)].transform);
        
        }
        //AddDummies(skillTree);

    }

    private void CrateOrAdjustNode(SkillTree skillTree, int i)
    {
        Skill[] tree = skillTree.tree;
        if (allNodes[i] == null)
        {
            allNodes[i] = InstantiateNodeObject(tree[i].Name);
        }

        allNodes[i].SetActive(true);
        SkillNodeRuntimeOld skillNode = allNodes[i].GetComponent<SkillNodeRuntimeOld>();
        skillNode.SetSkillTreeAndIndex(skillTree, i);
        skillNode.UpdateNode(i);
    }

    private GameObject MakeBranches(SkillTree skillTree, int rootSkill)
    {

        Skill[] tree = skillTree.tree;
        if (allNodes[rootSkill] == null)
        {
            allNodes[rootSkill] = InstantiateNodeObject(tree[rootSkill].Name);
        }



        int[] mReqs = skillTree.GetAllSkillsThatHaveRequirement(rootSkill);

        GameObject branch = Instantiate(branchPrefab);
        GameObject[] subBranches = null;
        if (mReqs != null && mReqs.Length > 0)
        {

            subBranches = new GameObject[mReqs.Length];
            for (int i = 0; i < mReqs.Length; i++)
            {

                CrateOrAdjustNode(skillTree, mReqs[i]);


                allNodes[mReqs[i]].transform.SetParent(branch.transform);

                branch.transform.SetParent(allNodes[rootSkill].transform.parent);


                subBranches[i] = MakeBranches(skillTree, mReqs[i]);
            }


        }
        branch.GetComponent<BranchUI>().SetupBranch(allNodes[rootSkill], subBranches);
        return branch;
    }

    void AddDummies(SkillTree skillTree)
    {
        for (int i = 0; i < allColumns.Count; i++)
        {

            SkillNodeRuntimeOld[] skillNodes = GetComponentsInChildren<SkillNodeRuntimeOld>();

            for (int j = 0; j < skillNodes.Length; j++)
            {


                for (int l = 0; l < skillNodes.Length; l++)
                {
                    if (j > l)
                    {
                        int[] lReqs = skillTree.tree[skillNodes[l].index].RequiredSkills;
                        int[] jReqs = skillTree.tree[skillNodes[j].index].RequiredSkills;
                        if (lReqs != null && jReqs != null)
                        {
                            for (int m = 0; m < lReqs.Length; m++)
                            {
                                for (int n = 0; n < jReqs.Length; n++)
                                {
                                    if (lReqs[m] == jReqs[n])
                                    {
                                        return;
                                    }
                                }

                            }
                        }

                    }
                }

                int skillsAsReq = skillTree.GetMaxRelatedHiarchyLevelOfSkill(skillNodes[j].index);

                if (skillsAsReq - 1 > skillNodes[j].activeDummies.Count)
                {
                    //float spacing = columnPrefab.GetComponent<VerticalLayoutGroup>().spacing;
                    //Vector2 newSizeDelta = (rectTransform.sizeDelta * new Vector2(1, skillsAsReq.Length));
                    //
                    //float totalSpacing = spacing * skillsAsReq.Length - 2;
                    //if(totalSpacing > 0)
                    //{
                    //    newSizeDelta += new Vector2(0, totalSpacing);
                    //}
                    //
                    //
                    //rectTransform.sizeDelta = newSizeDelta;

                    for (int k = 0; k < skillsAsReq - 1; k++)
                    {
                        SkillNodeRuntimeOld NodeWithRef = skillNodes[j];
                        Transform par = skillNodes[j].transform.parent;
                        int siblingIndex = skillNodes[j].transform.GetSiblingIndex() + 1;

                        AddDummy(NodeWithRef, par, siblingIndex);
                    }

                }


            }

        }
    }

    private void AddDummy(SkillNodeRuntimeOld NodeWithRef, Transform par, int siblingIndex)
    {
        GameObject SkillDummy = Instantiate(dummy, par);
        SkillDummy.transform.SetSiblingIndex(siblingIndex);
        NodeWithRef.activeDummies.Add(SkillDummy);
    }

    public void SelectNode(SkillNodeRuntimeOld node)
    {
        if (selectedNode != null)
        {
            DeselectNode(selectedNode);
        }

        SkillTree skillTree = gameManager.skillTree;

        selectedNode = node;
        int skillIndex = node.index;

        int[] upStream = skillTree.GetAllUpstreamSkills(skillIndex);
        if (upStream != null)
        {
            for (int i = 0; i < upStream.Length; i++)
            {
                allNodes[upStream[i]].GetComponent<SkillNodeRuntimeOld>().SetBaseColor(UpstreamColor);
            }
        }


        int[] downStream = skillTree.GetAllDownstreamSkills(skillIndex);
        if (downStream != null)
        {
            for (int i = 0; i < downStream.Length; i++)
            {
                allNodes[downStream[i]].GetComponent<SkillNodeRuntimeOld>().SetBaseColor(DownstreamColor);
            }
        }

    }

    public void ResetAllColors()
    {
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].GetComponent<SkillNodeRuntimeOld>().SetBaseColor(NormalColor);
        }
    }

    public void DeselectNode(SkillNodeRuntimeOld skillNode)
    {

        ResetAllColors();
        selectedNode = null;
    }


    private GameObject InstantiateNodeObject(string inputText)
    {
        GameObject node = Instantiate(SkillNodePrefab);

        node.SetActive(true);

        node.GetComponent<SkillNodeRuntimeOld>().SetSkillNameText(inputText);


        return node;
    }
}
