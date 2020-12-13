using SpawnLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectedUi : MonoBehaviour
{

    public Transform itemParent;
    public Text itemInfo;


    public Transform characterParent;
    public Text characterInfo;

    public ItemSpawner itemSpawner;

    public CharacterSpawner characterSpawner;

    SelectableObject selected;

    private void Awake()
    {

        SelectableObject.OnObjectSelected += SelectObject;
    }

    void SelectObject(SelectableObject selectableObject)
    {
        transform.position = selectableObject.transform.position + Vector3.up;
        if (selectableObject.GetType() == typeof(ItemObject))
        {
            SelectItemObject((ItemObject)selectableObject);
        }
        else if (selectableObject.GetType() == typeof(CharacterObject))
        {
            SelectCharacterObject((CharacterObject)selectableObject);
        }
    }
    void DeselectObject()
    {

    }


    void SelectItemObject(ItemObject itemObject)
    {
        if (!itemParent.gameObject.activeSelf)
        {
            itemParent.gameObject.SetActive(true);
        }
        if (characterParent.gameObject.activeSelf)
        {
            characterParent.gameObject.SetActive(false);
        }
        itemInfo.text = itemObject.item.itemType;
    }

    void SelectCharacterObject(CharacterObject characterObject)
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

