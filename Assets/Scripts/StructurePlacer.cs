using InventoryLogic;
using SpawnLogic;
using UnityEngine;
using System.Collections.Generic;

public class StructurePlacer : ItemPlacer
{


    public void SetPlacementStructureObject(StructureObject structureObject)
    {
        SetFirstPlacementObject(structureObject.structure, true, 1);
        structureObject.transform.SetParent(transform);
        structureObject.transform.localPosition = Vector3.zero;
        placeholderGameObjects.Add(structureObject.gameObject);
        gameObject.SetActive(true);
    }




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
        GameObject go = placeholderGameObjects[0];
        go.transform.SetParent(null);
        go.name = "Placed Structure";
        go.GetComponent<StructureObject>().SetNewStructure((Structure)currentSpawnable);
        go.GetComponent<StructureObject>().SetSelectable(true);
        placeholderGameObjects = new List<GameObject>();
        base.ConfirmPlacementRequest(pointerPos);
    }

}
