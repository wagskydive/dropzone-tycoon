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
        Skill skill = new Skill(inputText);

        gameManager.allSkills.Add(skill);

        
        GameObject node = Instantiate(SkillNodePrefab, SkillTreeParent);

        node.SetActive(true);

        node.GetComponent<SkillNode>().SetSkillName(inputText);

        allSkillNames.Add(inputText);

    }

}
