using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillsLogic;
using DataLogic;
using ManagementScripts;

public class SkillCreatorUI : MonoBehaviour
{
    public event Action<string> OnNewSkillCreated;
    public InputField NameInput;

    public GameObject SkillNodePrefab;

    public Transform SkillTreeParent;

    List<string> allSkillNames = new List<string>();

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void CreateSkillNode()
    {
        string inputText = DataChecks.EnsureUnique(allSkillNames.ToArray(), NameInput.text);

        Dictionary<string, float> effectors = new Dictionary<string, float>();
        effectors.Add("Test effector 1", .1f);
        effectors.Add("Test effector 2", .2f);

        string description = "Test Description";

        Skill skill = new Skill(inputText, description, effectors);

        gameManager.skillTree.AddSkill(skill);

        
        GameObject node = Instantiate(SkillNodePrefab, SkillTreeParent);

        node.SetActive(true);

        node.GetComponent<SkillNode>().SetSkillName(inputText);

        allSkillNames.Add(inputText);

    }

    public void SaveTreeButtonClick()
    {
        string path = Application.persistentDataPath + "/SkillTree6.json";
        Debug.Log(path);


        FileSaver.SkillTreeToJson(path, gameManager.skillTree.tree);

        FileSaver.JsonToSkillTree(path);


        //Debug.Log(SaveAndLoad.SaveSkillTree(gameManager.allSkills.ToArray(), path));
    }


}
