using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> HiddenObjects;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveOnObjects(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetActiveOnObjects(false);
    }

    void SetActiveOnObjects(bool enab)
    {
        for (int i = 0; i < HiddenObjects.Count; i++)
        {
            HiddenObjects[i].SetActive(enab);
        }
    }
}
