using InventoryLogic;
using SpawnLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnAndGrabButton : MonoBehaviour, IPointerUpHandler
{

    [SerializeField]
    private MouseGrabber mouseGrabber;

    ISpawnable currentSpawnable;

    public void SetSpawnable(ISpawnable spawnable)
    {
        currentSpawnable = spawnable;

    }



    public void OnPointerUp(PointerEventData eventData)
    {
        mouseGrabber.AddObjectItemPlacer(currentSpawnable);
    }
}
