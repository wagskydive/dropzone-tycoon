using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterLogic;

public class StatBar : MonoBehaviour
{
    public Image BarFiller;

    public Text NameText;

    Stat DisplayedStat;

    public void HookUp(Stat statToDisplay)
    {
        if (DisplayedStat != null)
        {
            DisplayedStat.OnValueChanged -= SetFiller;
        }
        DisplayedStat = statToDisplay;
        SetFiller(DisplayedStat.Value);
        NameText.text = statToDisplay.Name;
        DisplayedStat.OnValueChanged += SetFiller;
    }

    void SetFiller(float value)
    {
        BarFiller.fillAmount = value;
    }

}
