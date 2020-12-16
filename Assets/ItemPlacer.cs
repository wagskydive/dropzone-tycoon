using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPlacer : MonoBehaviour
{

    Renderer renderer;



    Material[] BackupMaterials;

    Color canPlaceColor = new Color(0, 1, 0, .4f);
    Color canNotPlaceColor = new Color(1, 0, 0, .4f);

    ISpawnable currentSpawnable;

    ItemSpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<ItemSpawner>();
        //gameObject.R
        renderer = gameObject.GetComponent<Renderer>();
        Material placementMaterial = Resources.Load("Materials/PlacementMaterial") as Material;
        SetMaterial(placementMaterial);

        AddCollider();

        //SetColors(Color.green);

        TerrainMouseDetect.OnTerrainClickDetected += PlaceObject;
    }

    public void SetCurrentSpawnable(ISpawnable spawnable)
    {

        currentSpawnable = spawnable;
    }

    private void SetMaterial(Material placementMaterial)
    {
        //gameObject.GetComponent<Renderer>().material = placementMaterial;
        SetAllMaterialsInRenderer(renderer, placementMaterial);
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        if (childRenderers != null)
        {
            for (int i = 0; i < childRenderers.Length; i++)
            {
                SetAllMaterialsInRenderer(childRenderers[i], placementMaterial);
                childRenderers[i].material = placementMaterial;
            }
        }
    }

    void SetAllMaterialsInRenderer(Renderer childRenderer, Material material)
    {
        for (int i = 0; i < childRenderer.materials.Length; i++)
        {
            childRenderer.materials[i] = material;
        }
    }

    void PlaceObject(Vector3 position, PointerEventData.InputButton eventbutton)
    {
        if(eventbutton == PointerEventData.InputButton.Left)
        {
            spawner.Spawn(currentSpawnable, position);
            Destroy(gameObject);
        }
    }

    private void SetColors(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;

        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        if (childRenderers != null)
        {
            for (int i = 0; i < childRenderers.Length; i++)
            {
                childRenderers[i].material.color = color;
            }
        }
    }

    void AddCollider()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = renderer.bounds.size;
        boxCollider.center = renderer.bounds.center;
        boxCollider.isTrigger = true;
    }


    void MakeMaterialsBackup()
    {
        BackupMaterials = new Material[renderer.materials.Length];
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            BackupMaterials[i] = new Material(renderer.materials[i]);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetType() != typeof(TerrainCollider))
        {
            SetColors(canNotPlaceColor);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetType() != typeof(TerrainCollider))
        {
            SetColors(canPlaceColor);
        }
    }
}
