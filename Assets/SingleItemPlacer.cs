using InventoryLogic;
using SpawnLogic;
using UnityEngine;
using System.Collections.Generic;

public class SingleItemPlacer : ItemPlacer
{






    public override void SetFirstPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        base.SetFirstPlacementObject(objectToAdd, snap, grSize);
        MouseDetect.OnLeftClickDetected += FirstClick;
    }



    public override void FirstClick(Vector3 pos)
    {
        base.FirstClick(pos);

        placeholderGameObjects[0].transform.SetParent(null);
        ConfirmPlacementRequest(pos);
    }

    public override void ConfirmPlacementRequest(Vector3 pointerPos)
    {
        Item item = (Item)currentSpawnable;
        spawner.Spawn(currentSpawnable, placeholderGameObjects[0].transform);
        base.ConfirmPlacementRequest(pointerPos);
    }

}