using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBuilderUi : MonoBehaviour
{




    [SerializeField]
    GameObject[] SelectedUI;
    [SerializeField]
    GameObject[] NonSelectedUI;


    private void Awake()
    {
        SelectableObject.OnObjectSelected += HandleSelected;
        SelectableObject.OnObjectDeselected += HandleDeselected;


    }

    private void HandleDeselected(SelectableObject obj)
    {
        SomethingSelectedForBuilding(false);
    }

    void HandleSelected(SelectableObject selectableObject)
    {
        if(selectableObject.GetType() == typeof(StructureObject))
        {
            SomethingSelectedForBuilding(true);
        }
        else
        {
            SomethingSelectedForBuilding(false);
        }
    }

    void SomethingSelectedForBuilding(bool somethingIsSelected)
    {
        for (int i = 0; i < SelectedUI.Length; i++)
        {
            SelectedUI[i].SetActive(somethingIsSelected);
        }
        for (int i = 0; i < NonSelectedUI.Length; i++)
        {
            NonSelectedUI[i].SetActive(!somethingIsSelected);
        }

    }
}
