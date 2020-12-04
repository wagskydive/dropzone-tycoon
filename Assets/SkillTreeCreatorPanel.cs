using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillsLogic;
using ManagementScripts;

public class SkillTreeCreatorPanel : MonoBehaviour
{
    public InputField NameInput;

    public GameObject SkillCreator;

    GameManager gameManager;

    public string fileName;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void LoadNodes(Skill[] skills)
    {
        gameManager.LoadTree(skills, NameInput.text);
    }

    public void SaveTreeButtonClick()
    {
        string path = Application.persistentDataPath + "/" + gameManager.skillTree.TreeName + ".json";
        Debug.Log(path);
        FileSaver.SkillTreeToJson(path, gameManager.skillTree.tree);
    }

    public void LoadTreeButtonPress()
    {
        string path = Application.persistentDataPath + "/" + NameInput.text + ".json";
        LoadNodes(FileSaver.JsonToSkillTree(path));
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
            SkillCreator.SetActive(true);
        }
    }
}
