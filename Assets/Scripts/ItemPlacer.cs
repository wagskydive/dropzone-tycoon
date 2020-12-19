using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DoorAndWindowPlacer : ItemPlacer
{

}


public abstract class ItemPlacer : MonoBehaviour, ISpawnRequester
{
    public virtual event Action OnItemPlaced;
    public static event Action<GameObject> OnPlaceHolderInstantiated;
    public virtual event Action<ISpawnable, Transform, Transform> OnSpawnRequest;
    public static event Action<ItemPlacer> OnPlacementComplete;


    protected Material[] BackupMaterials;

    protected Color canPlaceColor = new Color(0, 1, 0, .4f);
    protected Color canNotPlaceColor = new Color(1, 0, 0, .4f);

    protected ISpawnable currentSpawnable;

    protected ItemSpawner spawner;

    protected List<GameObject> placeholderGameObjects = new List<GameObject>();

    protected bool snapping;
    protected float gridSize;



    public virtual void Awake()
    {
        spawner = FindObjectOfType<ItemSpawner>();
        spawner.AddSpawnRequester(this);

        gameObject.GetComponent<ItemHandler>().OnItemPassed += SetFirstPlacementObject;

        //MouseDetect.OnLeftClickDetected += PlaceObject;

        //MouseDetect.OnMiddleClickDetected += RotateObject;
        //MouseDetect.OnMiddleUpClickDetected += CancelRotateObject;
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

    protected void DestroyListComplete()
    {
        DestroyPreviousListLeaveOne();
        if (placeholderGameObjects.Count > 0 && placeholderGameObjects[0] != null)
        {
            Destroy(placeholderGameObjects[0]);
        }
        placeholderGameObjects = new List<GameObject>();
    }

    protected void DestroyPreviousListLeaveOne()
    {
        for (int i = 0; i < placeholderGameObjects.Count; i++)
        {
            if (i > 0)
            {
                Destroy(placeholderGameObjects[i]);
            }
            else
            {
                placeholderGameObjects[i].SetActive(false);
            }

        }
        placeholderGameObjects.TrimExcess();
    }


    public virtual void  SetFirstPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        snapping = snap;
        gridSize = grSize;
        currentSpawnable = objectToAdd;
    }

    public virtual void FirstClick(Vector3 pos)
    {
        MouseDetect.OnLeftClickDetected -= FirstClick;
        placeholderGameObjects[0].transform.position = pos;

    }

    public virtual void CancelPlacement(Vector3 pointerPos)
    {
        DestroyListComplete();
    }

    public virtual void ConfirmPlacementRequest(Vector3 pointerPos)
    {
        DestroyListComplete();
        gameObject.SetActive(false);
        OnPlacementComplete?.Invoke(this);
    }

    protected void HandlePlaceHolderRotation(bool isX)
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


    protected Vector3 MakePositive(Vector3 vector3)
    {
        return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
    }


    protected Vector3 MakeNegative(Vector3 vector3)
    {
        return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z)) * -1;

    }

    protected void HandlePlaceHolderActivations(int longestDist)
    {
        longestDist = Math.Abs(longestDist);
        for (int i = 0; i < placeholderGameObjects.Count; i++)
        {

            placeholderGameObjects[i].SetActive(i < longestDist);

        }
    }

    protected GridPosition Vector3ToGridPosition(Vector3 input)
    {
        int x = Mathf.RoundToInt(input.x);
        int y = Mathf.RoundToInt(input.z);
        return new GridPosition(x, y);
    }

    protected Vector3 GridPositionToVector3(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.X, 0, gridPosition.Y);
    }

    protected GameObject InstantiatePlaceHolder(ISpawnable objectToAdd, Transform parent)
    {
        return InstantiatePlaceHolder(objectToAdd, parent, Vector3.zero);
    }

    protected GameObject InstantiatePlaceHolder(ISpawnable objectToAdd, Transform parent, Vector3 offset)
    {
        GameObject go = Instantiate(Resources.Load(objectToAdd.ResourcePath())) as GameObject;

        go.SetActive(true);
        go.transform.SetParent(parent);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = offset;
        //DestroyPreviousList();

        Bounds bounds = ColliderAdder.AddMeshCollidersInChildren(gameObject);

        OnPlaceHolderInstantiated?.Invoke(go);


        //AddCollider(go);
        go.SetActive(true);
        return go;
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
