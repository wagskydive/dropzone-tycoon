using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlacementMode { Single, Line, Grid, SingleBorder, LineBorder };

public class ItemPlacer : MonoBehaviour, ISpawnRequester
{
    public event Action OnItemPlaced;
    public event Action<ISpawnable, Transform, Transform> OnSpawnRequest;

    PlacementMode mode;

    Material[] BackupMaterials;

    Color canPlaceColor = new Color(0, 1, 0, .4f);
    Color canNotPlaceColor = new Color(1, 0, 0, .4f);

    ISpawnable currentSpawnable;

    ItemSpawner spawner;

    List<GameObject> placeholderGameObjects = new List<GameObject>();

    bool snapping;
    float gridSize;

    [SerializeField]
    public StructureObject workingStructure;

    GridPosition startPoint;

    private void Awake()
    {
        spawner = FindObjectOfType<ItemSpawner>();
        spawner.AddSpawnRequester(this);

        gameObject.GetComponent<ItemHandler>().OnItemPassed += SetPlacementObject;
        mode = PlacementMode.Line;

        //MouseDetect.OnLeftClickDetected += PlaceObject;

        //MouseDetect.OnMiddleClickDetected += RotateObject;
        //MouseDetect.OnMiddleUpClickDetected += CancelRotateObject;

        if (workingStructure != null)
        {
            workingStructure.AssignSpawner(spawner);
        }
    }

    private void CancelRotateObject(Vector3 position, Transform parent)
    {
        GameObject go = placeholderGameObjects[placeholderGameObjects.Count - 1];
        go.transform.SetParent(gameObject.transform);
        MouseDetect.OnOverDetected -= go.transform.Rotate;
    }

    private void RotateObject(Vector3 position, Transform parent)
    {
        GameObject go = placeholderGameObjects[placeholderGameObjects.Count - 1];
        go.transform.SetParent(parent);
        MouseDetect.OnOverDetected += go.transform.Rotate;
    }

    void DestroyListComplete()
    {
        DestroyPreviousListLeaveOne();
        if (placeholderGameObjects.Count > 0 && placeholderGameObjects[0] != null)
        {
            Destroy(placeholderGameObjects[0]);
        }
        placeholderGameObjects = new List<GameObject>();
    }

    void DestroyPreviousListLeaveOne()
    {

        for (int i = 0; i < placeholderGameObjects.Count; i++)
        {
            if (i > 0)
            {
                Destroy(placeholderGameObjects[i]);
            }

        }
        placeholderGameObjects.TrimExcess();
    }



    public void SetPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        snapping = snap;
        gridSize = grSize;
        currentSpawnable = objectToAdd;

        if (mode == PlacementMode.Single)
        {
            DestroyPreviousListLeaveOne();
            GameObject go = InstantiatePlaceHolder(objectToAdd, gameObject.transform);
            DestroyListComplete();


            placeholderGameObjects.Add(go);
        }
        else if (mode == PlacementMode.Line)
        {
            startPoint = Vector3ToGridPosition(transform.position);
            GameObject go = InstantiatePlaceHolder(objectToAdd, gameObject.transform);
            DestroyListComplete();


            placeholderGameObjects.Add(go);


            MouseDetect.OnLeftClickDetected += FirstClick;

        }


    }

    void FirstClick(Vector3 pos, Transform parent)
    {
        MouseDetect.OnLeftClickDetected -= FirstClick;
        startPoint = Vector3ToGridPosition(transform.position);

        placeholderGameObjects[0].transform.position = GridPositionToVector3(startPoint);
        placeholderGameObjects[0].transform.SetParent(parent);
        MouseDetect.OnLeftClickDetected += ConfirmLinePlacement;
        MouseDetect.OnOverDetected += HandleLinePlacement;
    }

    void SetPlaceholdersActive()
    {

    }

    int lastLongestDist;
    bool lastX;

    void HandleLinePlacement(Vector3 pointerPos)
    {
        int[] distance = startPoint.GridDistance(Vector3ToGridPosition(pointerPos));

        Debug.Log("Distance X: " + distance[0] + "\n" + "Distance Y: " + distance[1]);
        //X Line

        int longestDist = distance[0];
        bool isX = true;


        if (Math.Abs( distance[1] )> Math.Abs(distance[0]))
        {
            longestDist = distance[1];
            isX = false;
        }


        if(longestDist != lastLongestDist || lastX != isX)
        {
            if (longestDist != 0)
            {
                AdjustListLenght(longestDist);
                HandlePlaceHolderActivations(longestDist);
            }


            HandlePlaceHolderPositioning(longestDist, isX);
            HandlePlaceHolderRotation(isX);
        }
        lastLongestDist = longestDist;
        lastX = isX;


    }

    private void HandlePlaceHolderRotation(bool isX)
    {
        for (int i = 0; i < placeholderGameObjects.Count; i++)
        {
            placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
            if (!isX)
            {
                placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
                placeholderGameObjects[i].transform.Rotate(Vector3.up, -90);
            }
        }
    }


Vector3 MakePositive(Vector3 vector3)
{
    return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
}


private Vector3 MakeNegative(Vector3 vector3)
{
    return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z)) * -1;

}


private void HandlePlaceHolderPositioning(int longestDist, bool isX)
{
    for (int i = 0; i < placeholderGameObjects.Count; i++)
    {
        if (isX)
        {
            if (longestDist > 0)
            {
                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(i, 0, 0);//MakePositive(placeholderGameObjects[i].transform.localPosition);
            }
            else
            {
                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(-i - 1, 0, 0);//MakePositive(placeholderGameObjects[i].transform.localPosition);
            }
        }
        else
        {
            if (longestDist > 0)
            {
                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(0, 0, i);//MakePositive(placeholderGameObjects[i].transform.localPosition);
            }
            else
            {
                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(0, 0, -i - 1);//MakePositive(placeholderGameObjects[i].transform.localPosition);
            }


        }
    }
}


private void HandlePlaceHolderActivations(int longestDist)
{
    longestDist = Math.Abs(longestDist);
    for (int i = 0; i < placeholderGameObjects.Count; i++)
    {

        placeholderGameObjects[i].SetActive(i < longestDist);

    }
}

private void AdjustListLenght(int longestDist)
{
    int visablePlaceholders = Math.Abs(longestDist);
    int placeCount = placeholderGameObjects.Count;
    if (placeCount < visablePlaceholders)
    {
        for (int i = 0; i < visablePlaceholders - placeCount; i++)
        {
            GridPosition gP = new GridPosition(startPoint.X + longestDist - 1, startPoint.Y);

            GameObject go = InstantiatePlaceHolder(currentSpawnable, workingStructure.transform, GridPositionToVector3(gP));

            placeholderGameObjects.Add(go);
        }
    }
}

void ConfirmLinePlacement(Vector3 pointerPos, Transform parent)
{
    MouseDetect.OnLeftClickDetected -= ConfirmLinePlacement;
    MouseDetect.OnOverDetected -= HandleLinePlacement;

    DestroyPreviousListLeaveOne();
}

GridPosition Vector3ToGridPosition(Vector3 input)
{
    int x = Mathf.RoundToInt(input.x);
    int y = Mathf.RoundToInt(input.z);
    return new GridPosition(x, y);
}

Vector3 GridPositionToVector3(GridPosition gridPosition)
{
    return new Vector3(gridPosition.X, 0, gridPosition.Y);
}

private GameObject InstantiatePlaceHolder(ISpawnable objectToAdd, Transform parent)
{
    return InstantiatePlaceHolder(objectToAdd, parent, Vector3.zero);
}
private GameObject InstantiatePlaceHolder(ISpawnable objectToAdd, Transform parent, Vector3 offset)
{
    GameObject go = Instantiate(Resources.Load(objectToAdd.ResourcePath())) as GameObject;

    go.SetActive(true);
    go.transform.SetParent(parent);
    go.transform.localRotation = Quaternion.identity;
    go.transform.localPosition = offset;
    //DestroyPreviousList();

    Bounds bounds = ColliderAdder.AddMeshCollidersInChildren(gameObject);

    Material placementMaterial = Resources.Load("Materials/PlacementMaterial") as Material;
    SetMaterial(go, placementMaterial);

    AddCollider(go);
    go.SetActive(true);
    return go;
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


    placeholderGameObjects[0].SetActive(false);
    placeholderGameObjects[0].transform.position = position;

    DestroyPreviousListLeaveOne();

    //spawner.Spawn(currentSpawnable, position);

    OnSpawnRequest?.Invoke(currentSpawnable, placeholderGameObjects[0].transform, parent);


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
        SetColors(placeholderGameObjects, canNotPlaceColor);
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.GetType() != typeof(TerrainCollider))
    {
        SetColors(placeholderGameObjects, canPlaceColor);
    }
}

public void AssignSpawner(Spawner spawner)
{
    throw new NotImplementedException();
}

public void SpawnRequest(ISpawnable item, Transform position, Transform parent = null)
{

}
}
