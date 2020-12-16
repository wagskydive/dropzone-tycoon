using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnLogic;
using InventoryLogic;

public class MouseGrabber : ItemHandler
{
    public TerrainMouseDetect terrainMouseDetect;



    private void Awake()
    {
        TerrainMouseDetect.OnTerrainOverDetected += SetPositionOverTerrainFromMouse;
    }

    public void AddObjectItemPlacer(ISpawnable objectToAdd)
    {


        //go.AddComponent<ItemPlacer>().SetCurrentSpawnable(objectToAdd);
        //go.GetComponent<ItemPlacer>().OnItemPlaced += DisableGrabbedItem;
        PassItem(objectToAdd);

    }

    void DisableGrabbedItem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }

    void SetPositionOverTerrainFromMouse(Vector3 position)
    {
        transform.position = position;
    }


}
