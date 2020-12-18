using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StatsLogic;

public class StatBar : MonoBehaviour
{
    public Image BarFiller;

    public Text NameText;

    Stat DisplayedStat;

    bool isChanging;

    public RectTransform TreshholdBar;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void HookUp(Stat statToDisplay)
    {
        if (DisplayedStat != null)
        {
            DisplayedStat.OnValueChanged -= SetFiller;
        }
        DisplayedStat = statToDisplay;
        SetFiller(StatsHandler.Tick(DisplayedStat, Time.deltaTime));
        NameText.text = statToDisplay.Name;
        if(statToDisplay.ValueChangePerSecond != 0)
        {
            isChanging = true;
        }

        SetTreshholdPosition(DisplayedStat.Threshhold);


        statToDisplay.OnThreshholdSet += SetTreshholdPosition;
        statToDisplay.OnValueChanged += SetFiller;
    }

    void SetFiller(float value)
    {
        BarFiller.fillAmount = value;
    }

    public void SetTreshholdPosition(float factor)
    {
        TreshholdBar.localPosition = new Vector3((rectTransform.sizeDelta.x * factor)- rectTransform.sizeDelta.x*.5f, 0, 0);
    }

}
