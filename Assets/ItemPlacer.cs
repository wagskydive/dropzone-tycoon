using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPlacer : MonoBehaviour, ISpawnRequester
{
    public event Action OnItemPlaced;
    public event Action<ISpawnable, Vector3, Transform> OnSpawnRequest;

    Material[] BackupMaterials;

    Color canPlaceColor = new Color(0, 1, 0, .4f);
    Color canNotPlaceColor = new Color(1, 0, 0, .4f);

    ISpawnable currentSpawnable;

    ItemSpawner spawner;

    List<GameObject> previousGameObjects = new List<GameObject>();

    bool snapping;
    float gridSize;

    private void Awake()
    {
        spawner = FindObjectOfType<ItemSpawner>();
        spawner.AddSpawnRequester(this);

        gameObject.GetComponent<ItemHandler>().OnItemPassed += SetPlacementObject;


        MouseDetect.OnLeftClickDetected += PlaceObject;
    }

    void DestroyPreviousList()
    {
        for (int i = 0; i < previousGameObjects.Count; i++)
        {
            Destroy(previousGameObjects[i]);
        }
        previousGameObjects = new List<GameObject>();
    }

    public void SetPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        snapping = snap;
        gridSize = grSize;
        currentSpawnable = objectToAdd;
        GameObject go = Instantiate(Resources.Load(objectToAdd.ResourcePath())) as GameObject;

        go.SetActive(true);
        go.transform.SetParent(gameObject.transform);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;
        //DestroyPreviousList();




        Bounds bounds = ColliderAdder.AddMeshCollidersInChildren(gameObject);
        transform.GetChild(0).Translate((transform.position - bounds.min) - new Vector3(bounds.size.x, 0, bounds.size.z));

        //go.transform.position = bounds.min;
        //transform.GetChild(0).localPosition = - new Vector3(bounds.size.x, 0, bounds.size.z);

        //transform.GetChild(0).Translate(- new Vector3(bounds.max.x, 0, bounds.max.z));

        //go.transform.localPosition = go.transform.localPosition - bounds.center;

        Material placementMaterial = Resources.Load("Materials/PlacementMaterial") as Material;
        SetMaterial(go, placementMaterial);

        AddCollider(go);
        go.SetActive(true);

        previousGameObjects.Add(go);
    }




    private void SetMaterial(GameObject go, Material placementMaterial)
    {
        Renderer renderer = go.GetComponent<Renderer>();
        //gameObject.GetComponent<Renderer>().material = placementMaterial;
        SetAllMaterialsInRenderer(renderer, placementMaterial);
        Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();
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

    void PlaceObject(Vector3 position, Transform parent)
    {
        if (snapping)
        {
            position = VectorHelper.RoundToInt(transform.position, gridSize);
        }
        DestroyPreviousList();
        //spawner.Spawn(currentSpawnable, position);

        OnSpawnRequest?.Invoke(currentSpawnable, position, parent);
        OnItemPlaced?.Invoke();

    }

    private void SetColors(List<GameObject> gameObjects, Color color)
    {

        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject go = gameObjects[i];
            go.GetComponent<Renderer>().material.color = color;

            Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();
            if (childRenderers != null)
            {
                for (int j = 0; j < childRenderers.Length; j++)
                {
                    childRenderers[j].material.color = color;
                }
            }
        }


    }

    void AddCollider(GameObject go)
    {
        BoxCollider boxCollider = go.AddComponent<BoxCollider>();
        Renderer renderer = go.GetComponent<Renderer>();
        boxCollider.size = renderer.bounds.size;
        boxCollider.center = renderer.bounds.center;
        boxCollider.isTrigger = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(TerrainCollider))
        {
            SetColors(previousGameObjects, canNotPlaceColor);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetType() != typeof(TerrainCollider))
        {
            SetColors(previousGameObjects, canPlaceColor);
        }
    }

    public void AssignSpawner(Spawner spawner)
    {
        throw new NotImplementedException();
    }

    public void SpawnRequest(ISpawnable item, Vector3 position, Transform parent = null)
    {

    }
}
