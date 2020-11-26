using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDetailsPanel : MonoBehaviour
{
    public Text CharacterNameText;

    //public Text StateText;


    public string CurrentCharacterName;

    private void Awake()
    {
        CharacterButton.OnCharacterButtonClick += GetAndShowCharacterDetails;
    }



    private void GetAndShowCharacterDetails(string characterName)
    {

        CurrentCharacterName = characterName;
        CharacterNameText.text = "Name: " + characterName;
    }


}

