using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LoadStructureDropdown : MonoBehaviour
{
    UnityAction<int> unityAction;
    Dropdown dropdown;

    [SerializeField]
    private StructureBuilder structureBuilder;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        PopulateStructureDropdown();

        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });

    }

    void PopulateStructureDropdown()
    {
        dropdown.ClearOptions();

        List<string> fileNames = new List<string>();
        fileNames.Add("");
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Resources/Structures/");
        FileInfo[] allFiles = info.GetFiles();
        for (int i = 0; i < allFiles.Length; i++)
        {
            if (allFiles[i].Name.EndsWith(".json"))
            {
                fileNames.Add(allFiles[i].Name);
            }
        }
        dropdown.AddOptions(fileNames);
    }

    void DropdownValueChanged(Dropdown change)
    {
        structureBuilder.LoadStructure(dropdown.options[change.value].text);
        gameObject.SetActive(false);
    }
}
