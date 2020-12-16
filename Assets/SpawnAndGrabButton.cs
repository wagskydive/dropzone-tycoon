using InventoryLogic;
using SpawnLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnAndGrabButton : MonoBehaviour
{

    [SerializeField]
    private MouseGrabber mouseGrabber;

    ISpawnable currentSpawnable;

    public void SetSpawnable(ISpawnable spawnable)
    {
        currentSpawnable = spawnable;

    }

    public void OnButtonPress()
    {
        mouseGrabber.AddObjectItemPlacer(currentSpawnable);

    }
}
