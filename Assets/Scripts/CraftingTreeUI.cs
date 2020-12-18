using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ManagementScripts;
using InventoryLogic;
using System;

public class CraftingTreeUI : MonoBehaviour, ITreeBrowser
{
    public GameObject columnPrefab;
    List<GameObject> allColumns = new List<GameObject>();

    public GameObject ItemNodePrefab;
    public List<GameObject> allNodes = new List<GameObject>();

    GameManager gameManager;

    ItemNodeRuntimeOld selectedNode = null;

    public Color NormalColor;

    public Color UpstreamColor;

    public Color DownstreamColor;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if(gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        //gameManager.OnOldSkillWillBeDestroyed += UnSubscribeToNewTree;
        gameManager.OnNewLibraryCreated += SubscribeToNewTree;

        
        ItemNodeRuntimeOld.OnNodeHoverEnter += SelectNode;
        ItemNodeRuntimeOld.OnNodeHoverExit += DeselectNode;

        ItemNodeRuntimeOld.OnNodeClicked += NodeClicked;

    }

    private void NodeClicked(ItemNodeRuntimeOld node)
    {
        
    }

    void UnSubscribeToNewTree(ItemsLibrary library)
    {
        library.OnLibraryModified -= RefreshTree;
    }


    void SubscribeToNewTree(ItemsLibrary library)
    {
        library.OnLibraryModified += RefreshTree;
        RefreshTree(library);
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




    public void RefreshTree(ItemsLibrary library)
    {
        List<ItemType> allItems = library.allItems;

        int maxLevel = library.GetMaxHiarchyLevelOfTree();

        for (int j = 0; j < maxLevel + 1; j++)
        {
            if (j + 1 > allColumns.Count)
            {
                GameObject col = Instantiate(columnPrefab, transform);
                col.SetActive(true);
                allColumns.Add(col);

            }
        }



        if (allItems.Count > allNodes.Count)
        {
            allNodes.AddRange(new GameObject[allItems.Count - allNodes.Count]);
        }

        for (int i = 0; i < allItems.Count; i++)
        {
            if (allNodes[i] == null)
            {
                allNodes[i] = InstantiateNodeObject(allItems[i].TypeName);
            }

            allNodes[i].SetActive(true);
            ItemNodeRuntimeOld itemNode = allNodes[i].GetComponent<ItemNodeRuntimeOld>();
            itemNode.SetLibraryAndIndex(library, i);
            itemNode.UpdateNode(i);

            allNodes[i].transform.SetParent(allColumns[library.GetHiarchyLevelOfItem(allItems[i].TypeName)].transform);

        }

    }

    public void SelectNode(ItemNodeRuntimeOld node)
    {
        if (selectedNode != null)
        {
            DeselectNode(selectedNode);
        }

        ItemsLibrary library = gameManager.Library;

        selectedNode = node;
        int skillIndex = node.index;

        int[] upStream = library.GetAllUpstreamItems(skillIndex);
        if (upStream != null)
        {
            for (int i = 0; i < upStream.Length; i++)
            {
                allNodes[upStream[i]].GetComponent<ItemNodeRuntimeOld>().SetBaseColor(UpstreamColor);
            }
        }


        int[] downStream = library.GetAllDownstreamItems(skillIndex);
        if (downStream != null)
        {
            for (int i = 0; i < downStream.Length; i++)
            {
                allNodes[downStream[i]].GetComponent<ItemNodeRuntimeOld>().SetBaseColor(DownstreamColor);
            }
        }

    }

    public void ResetAllColors()
    {
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].GetComponent<ItemNodeRuntimeOld>().SetBaseColor(NormalColor);
        }
    }

    public void DeselectNode(ItemNodeRuntimeOld skillNode)
    {

        ResetAllColors();
        selectedNode = null;
    }


    public GameObject InstantiateNodeObject(string inputText)
    {
        GameObject node = Instantiate(ItemNodePrefab);

        node.SetActive(true);

        node.GetComponent<ItemNodeRuntimeOld>().SetItemNameText(inputText);


        return node;
    }
}
