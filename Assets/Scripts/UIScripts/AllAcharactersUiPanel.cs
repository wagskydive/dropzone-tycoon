using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterLogic;

public class AllCharactersUiPanel : MonoBehaviour
{
    //[Tooltip("Assign Component that can display texts. with a script that implements IDisplayText interface")]
    public GameObject textDisplayTemplate;

    string currentDisplayedAccountID;

    private void Awake()
    {
        FindObjectOfType<ShowCharactersButton>().OnShowAllCharactersButtonClick += DisplayAccounts;
    }

    public void DisplayAccounts(bool isVisible)
    {
        if (!isVisible)
        {

            CharacterHolder holder = FindObjectOfType<ManagementScripts.GameManager>().Characters;
            textDisplayTemplate.GetComponent<IDisplayCharacters>().Display(CharacterDataSupplier.AllCharacterNames(holder));
        }

    }
}
