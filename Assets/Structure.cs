using InventoryLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part
{
    public Item item { get; private set; }

    public Part(Vector3 position)
    {

    }
    public void Place(Vector3 position)
    {

    }
}

public class Structure : MonoBehaviour
{
    List<Part> parts = new List<Part>();

    public void InstallItem(Item item)
    {
        //parts.Add(new Part())
    }
}
