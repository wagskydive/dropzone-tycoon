using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnLogic;
using InventoryLogic;

public class MouseGrabber : ItemHandler
{
    public MouseDetect mouseDetect;


    private void Awake()
    {
        MouseDetect.OnOverDetected += SetPositionOverTerrainFromMouse;
    }

    private void Update()
    {
        if (snapping)
        {
            transform.position = VectorHelper.RoundToInt(transform.position, gridSize);
        }
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
