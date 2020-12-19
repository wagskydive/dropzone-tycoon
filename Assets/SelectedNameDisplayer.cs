using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedNameDisplayer : MonoBehaviour
{
    public Text displayText;
    private void Awake()
    {
        SelectableObject.OnObjectSelected += SetNameText;
    }

    private void SetNameText(SelectableObject obj)
    {
        if(obj.GetType() == typeof(StructureObject))
        {
            StructureObject structureObject = (StructureObject)obj;
            displayText.text = structureObject.structure.Name;
        }
        
    }
}
