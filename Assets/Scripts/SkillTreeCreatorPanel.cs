using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillsLogic;
using ManagementScripts;
using System.IO;

public class SkillTreeCreatorPanel : MonoBehaviour
{
    public InputField NameInput;

    public GameObject SkillCreator;

    public DropdownHandler FilesDropdown;

    GameManager gameManager;

    public Editable Loader;

    public string fileName;

    public string pathRoot;

    private void Start()
    {
        pathRoot = Application.persistentDataPath + "/";
        gameManager = FindObjectOfType<GameManager>();
        Loader.OnEdited += LoadTree;
    }

    void LoadNodes(Skill[] skills, string nameP)
    {
        gameManager.LoadSkillTree(skills, nameP);
        gameObject.SetActive(false);
        SkillCreator.SetActive(true);
    }

    public void SaveTreeButtonClick()
    {
        string path = pathRoot + gameManager.skillTree.TreeName + ".skilltree";
        Debug.Log(path);
        FileSaver.SkillTreeToJson(path, gameManager.skillTree.tree);
    }

    public void LoadTreeButtonPress()
    {

        var info = new DirectoryInfo(pathRoot);
        FileInfo[] fileInfos = info.GetFiles();
        List<string> files = new List<string>();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string extension = Path.GetExtension(fileInfos[i].Name);
            if(extension == ".skilltree")
            {
                string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extension.Length);
                files.Add(result);
            }

            
        }

        FilesDropdown.PopulateDropDown(files.ToArray());

        Loader.EnableEditMode();
    }
    void LoadTree(string namePath)
    {

    
        string path = pathRoot + namePath + ".skilltree";
        LoadNodes(FileSaver.JsonToSkillTree(path),namePath);
    }

    

    public void CreatNewTreeButtonPress()
    {
        if(NameInput.text == null || NameInput.text == "")
        {
            return;
        }
        else
        {
            gameManager.NewTree(NameInput.text);
            gameObject.SetActive(false);
            SkillCreator.SetActive(true);
        }
    }
}
