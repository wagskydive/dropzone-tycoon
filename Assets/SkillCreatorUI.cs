using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillsLogic;
using DataLogic;

public class SkillCreatorUI : MonoBehaviour
{
    public event Action<string> OnNewSkillCreated;
    public InputField NameInput;

    public GameObject SkillNodePrefab;

    List<string> allSkillNames = new List<string>();

    public void CreateSkillNode()
    {        
        string inputText = DataChecks.EnsureUnique(allSkillNames.ToArray(), NameInput.text);
        GameObject node = Instantiate(SkillNodePrefab, transform.parent);

        node.GetComponent<SkillNode>().SetSkillName(inputText);


        allSkillNames.Add(inputText);

    }

}
