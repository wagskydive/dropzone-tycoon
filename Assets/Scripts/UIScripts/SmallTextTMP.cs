using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class SmallTextTMP : MonoBehaviour, IDisplayText
{
    TMP_Text tMP_Text;

    private void Awake()
    {
        tMP_Text = GetComponent<TMP_Text>();
    }


    public void Display(string[] text)
    {
        //tMP_Text.text = text;
    }

}
