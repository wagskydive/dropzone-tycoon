using SpawnLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SelectedUi : MonoBehaviour
{

    public Transform itemParent;
    public Text itemInfo;


    public Transform characterParent;
    public Text characterInfo;

    public ItemSpawner itemSpawner;

    public CharacterSpawner characterSpawner;

    SelectableObject hover;

    private void Awake()
    {

        SelectableObject.OnMouseEnterDetected += HoverEnter;
        SelectableObject.OnMouseExitDetected += HoverExit;
        //SelectableObject.OnObjectSelected += SelectObject;
    }

    void HoverEnter(SelectableObject selectableObject)
    {
        transform.position = selectableObject.transform.position + Vector3.up+Vector3.back*.1f;
        if (selectableObject.GetType() == typeof(ItemObject))
        {
            HoverItemObject((ItemObject)selectableObject);
        }
        else if (selectableObject.GetType() == typeof(CharacterObject))
        {
            HoverCharacterObject((CharacterObject)selectableObject);
        }
        hover = selectableObject;
    }
    void HoverExit(SelectableObject selectableObject)
    {
        //hover = null
        //itemParent.gameObject.SetActive(false);
        characterParent.gameObject.SetActive(false);
        hover = null;
    }

    void SelectObject(SelectableObject selectableObject)
    {
        transform.position = selectableObject.transform.position + Vector3.up + Vector3.back * .1f;
        SelectableObject.OnMouseEnterDetected -= HoverEnter;
        SelectableObject.OnMouseExitDetected -= HoverExit;
        SelectableObject.OnObjectSelected += DeselectObject;
    }

    private void DeselectObject(SelectableObject obj)
    {
        SelectableObject.OnMouseEnterDetected += HoverEnter;
        SelectableObject.OnMouseExitDetected += HoverExit;
    }

    void HoverItemObject(ItemObject itemObject)
    {
        if (!itemParent.gameObject.activeSelf)
        {
            itemParent.gameObject.SetActive(true);
        }
        if (characterParent.gameObject.activeSelf)
        {
            characterParent.gameObject.SetActive(false);
        }
        itemInfo.text = itemObject.item.itemType.TypeName;
    }

    void HoverCharacterObject(CharacterObject characterObject)
    {
        if (!characterParent.gameObject.activeSelf)
        {
            characterParent.gameObject.SetActive(true);
        }
        if (itemParent.gameObject.activeSelf)
        {
            itemParent.gameObject.SetActive(false);
        }
        characterInfo.text = characterObject.name;
    }


}

