using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


public class DirectoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject contentParent;
    public Text DirectoryName;

    public bool childHover;

    [SerializeField]
    public string resourcePath;

    bool isChild = false;
    public DirectoryButton parentDirectoryButton;


    public DirectoryInfo parentDirectoryInfo;

    public DirectoryInfo rootDirectoryInfo;

    bool isLoaded = false;
    private void Awake()
    {
        if (parentDirectoryButton != null)
        {
            isChild = true;
        }
    }





    public void CreateDirectoryTree(string baseLevelPath, string extentionFilter = "fbx")
    {

        Setup(baseLevelPath);
        CreateSubChildren(baseLevelPath, extentionFilter);
    }

    public void SetupAsChild(string thisLevel, string extentionFilter = "fbx")
    {


        Setup(thisLevel, extentionFilter);
        resourcePath = null;
        isChild = true;
        //CreateSubChildren(thisLevel, extentionFilter);
    }


    public void Setup(string thisLevel, string extentionFilter = "fbx")
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(thisLevel);
        DirectoryName.text = directoryInfo.Name;
        

    }


    public void CreateSubChildren(string thisLevel, string extentionFilter = "fbx")
    {
        DirectoryInfo info = new DirectoryInfo(thisLevel);
        DirectoryInfo[] directoryInfos = info.GetDirectories();
        if (directoryInfos.Any())
        {
            for (int i = 0; i < directoryInfos.Length; i++)
            {
                if (directoryInfos[i].Name != info.Name)
                {
                    CreateChild(thisLevel + "/" + directoryInfos[i].Name + "/", extentionFilter);

                }
            }
        }

    }

    public void CreateChild(string childLevelPath, string extentionFilter = "fbx")
    {

        GameObject child = Instantiate(gameObject, contentParent.transform);
        child.GetComponent<DirectoryButton>().SetupAsChild(childLevelPath, extentionFilter);
    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        contentParent.SetActive(true);
        if (isChild)
        {
            parentDirectoryButton.childHover = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isChild)
        {
            parentDirectoryButton.childHover = false;
        }
        if (!childHover)
        {
            contentParent.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isLoaded)
        {
            CreateDirectoryTree(resourcePath);
        }
    }
}
