using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnLogic;
using System;

public class MouseGrabber : MonoBehaviour
{
    public event Action<Type> SpawnOrder;
    public TerrainMouseDetect terrainMouseDetect;


    private void Awake()
    {
        TerrainMouseDetect.OnTerrainOverDetected += SetPositionOverTerrainFromMouse;
    }

    public void AddObjectToGrabber(ISpawnable objectToAdd )
    {
        GameObject go = Instantiate(Resources.Load(objectToAdd.ResourcePath())) as GameObject;

        go.transform.SetParent(gameObject.transform);
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<ItemPlacer>().SetCurrentSpawnable(objectToAdd);
    }

    void SetPositionOverTerrainFromMouse(Vector3 position)
    {
        transform.position = position;
    }

}
