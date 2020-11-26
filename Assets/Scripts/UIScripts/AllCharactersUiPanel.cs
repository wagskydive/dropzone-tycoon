using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterLogic;

public class AllAcharactersUiPanel : MonoBehaviour
{
    public GameObject textDisplayTemplate;

    string currentDisplayedAccountID;

    private void Awake()
    {
        FindObjectOfType<ShowCharactersButton>().OnShowAllCharactersButtonClick += DisplayCharacters;
    }

    public void DisplayCharacters(bool isVisible)
    {
        if (!isVisible)
        {


            CharacterDataHolder characterHolder = FindObjectOfType<ManagementScripts.GameManager>().Characters;
            textDisplayTemplate.GetComponent<IDisplayCharacters>().Display(CharacterDataSupplier.AllCharacterNames(characterHolder));

        }

    }
}
