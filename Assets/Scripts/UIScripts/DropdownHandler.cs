using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropdownHandler : MonoBehaviour
{


    Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

    public void PopulateDropDown(string[] ids, string exception)
    {
        dropdown.ClearOptions();


        List<string> names = new List<string>();

        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] != exception)
            {
                names.Add(ids[i]);
                
            }
        }

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();


        for (int i = 0; i < names.Count; i++)
        {
            
            options.Add(new Dropdown.OptionData(names[i]));
            options[i].text = names[i];
        }
        dropdown.AddOptions(options);
    }

    public string GetSelected()
    {
        return dropdown.options[dropdown.value].text;
    }
}

