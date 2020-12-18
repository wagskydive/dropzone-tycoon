using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TreshholdUiBar : MonoBehaviour
{
    public RectTransform TreshholdBar;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTreshholdPosition(float factor)
    {
        TreshholdBar.localPosition = new Vector3(rectTransform.rect.width * factor, rectTransform.localPosition.y, rectTransform.localPosition.z);
    }
}
