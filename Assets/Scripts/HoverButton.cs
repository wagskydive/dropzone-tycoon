using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnPointerEnterDetected;

    public event Action OnPointerExitDetected;

    public List<GameObject> HiddenObjects;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveOnObjects(true);
        OnPointerEnterDetected?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetActiveOnObjects(false);
        OnPointerExitDetected?.Invoke();
    }

    void SetActiveOnObjects(bool enab)
    {
        for (int i = 0; i < HiddenObjects.Count; i++)
        {
            HiddenObjects[i].SetActive(enab);
        }
    }
}
