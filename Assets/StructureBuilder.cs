using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using ManagementScripts;
using System;

public static class DummyStructureBuilder
{
    public static Structure AllWalls(int length, int width, Item item)
    {
        Structure structure = new Structure("Dummy", 1, 2.4f);
        for (int i = 0; i < length; i++)
        {

            for (int j = 0; j < width; j++)
            {
                //x wall
                structure.AddPartAsWall(i, j, i + 1, j, item, 0);
                // y wall
                //structure.AddPartAsWall(i, j, i, j+1, item,0);


            }
        }
        return structure;

    }
}

[RequireComponent(typeof(MeshModifier))]
public class StructureBuilder : MonoBehaviour
{


    public StructureObject currentBuild;

    public ItemSpawner itemSpawner;

    public GameObject ItemPlacerObject;

    public ItemHandler itemHandler;

    public StringInputPopup SavePopup;

    BoxCollider boxCollider;

    public GameManager gameManager;
    GameObject go;


    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        go = new GameObject();

        StructureObject.OnStructureSelected += HandleSelected;

        itemSpawner.OnItemSpawned += ItemSpawner_OnItemSpawned;
        //Invoke("SetNewStructure", 3);

        ItemPlacer.OnPlacementComplete += ItemPlacer_OnPlacementComplete;

        SavePopup.OnInputConfirmed += SaveCurrentStructure;
    }



    private void ItemPlacer_OnPlacementComplete(ItemPlacer obj)
    {
        for (int i = 0; i < obj.ToBeReplaced.Count; i++)
        {
            Destroy(obj.ToBeReplaced[i].gameObject);
        }

        currentBuild.DeselectObject();
    }

    private void ItemSpawner_OnItemSpawned(ItemObject obj)
    {
        currentBuild.AddItemToStructure(obj);
    }

    private void HandleSelected(StructureObject selected)
    {

        currentBuild = selected;
    }


    public void StartNewBoxStructure()
    {

        WallBoxPlacer wallBoxPlacer = ItemPlacerObject.AddComponent<WallBoxPlacer>();

        string newName = "My New Structure";
        Structure newStructure = new Structure(newName, 1, 2.4f);

        GameObject newStructureObject = new GameObject(newName);

        StructureObject structureObject = newStructureObject.AddComponent<StructureObject>();

        structureObject.SetNewStructure(newStructure);

        currentBuild = structureObject;
        wallBoxPlacer.SetWorkingStructure(itemSpawner, structureObject, 0);
        int wallItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall");
        Item item = new Item(gameManager.Library.allItems[wallItemTypeIndex]);
        //itemHandler.AddObjectItemPlacer(item);
        itemHandler.gameObject.SetActive(true);

        wallBoxPlacer.SetFirstPlacementObject(item, true, 1);
        wallBoxPlacer.OnWallPlaced += WallPlacer_OnWallPlaced;
    }

    public void StartNewLineStructure()
    {

        WallLinePlacer wallLinePlacer = ItemPlacerObject.AddComponent<WallLinePlacer>();

        string newName = "My New Structure";
        Structure newStructure = new Structure(newName, 1, 2.4f);

        GameObject newStructureObject = new GameObject(newName);

        StructureObject structureObject = newStructureObject.AddComponent<StructureObject>();

        structureObject.SetNewStructure(newStructure);

        currentBuild = structureObject;


        wallLinePlacer.SetWorkingStructure(itemSpawner, structureObject, 0);
        int wallItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall");
        Item item = new Item(gameManager.Library.allItems[wallItemTypeIndex]);
        //itemHandler.AddObjectItemPlacer(item);
        itemHandler.gameObject.SetActive(true);

        wallLinePlacer.SetFirstPlacementObject(item, true, 1);
        wallLinePlacer.OnWallPlaced += WallPlacer_OnWallPlaced;
    }

    private void WallPlacer_OnWallPlaced(WallPlacement wall)
    {

        currentBuild.structure.AddPartAsWall(wall, wall.Stretch);
        currentBuild.SelectObject();
    }

    public void AddWindowsButtonclick()
    {
        currentBuild.OnStructurePartSelected += StartWindowPlacement;
    }


    void StartWindowPlacement(GridPosition startPoint)
    {
        currentBuild.OnStructurePartSelected -= StartWindowPlacement;

        WallLinePlacer wallLinePlacer = ItemPlacerObject.AddComponent<WallLinePlacer>();

        wallLinePlacer.SetWorkingStructure(itemSpawner, currentBuild, 0);
        int wallItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Window Slide");
        Item item = new Item(gameManager.Library.allItems[wallItemTypeIndex]);
        //itemHandler.AddObjectItemPlacer(item);
        itemHandler.gameObject.SetActive(true);

        GameObject[] outObjects;
        gameManager.StretchObjects.TryGetValue("Wall Window Slide", out outObjects);
        wallLinePlacer.SetFirstPlacementObject(item, true, outObjects);
        wallLinePlacer.OnWallPlaced += WallPlacer_OnWallPlaced;
        wallLinePlacer.stretch = true;
        wallLinePlacer.FirstClick(currentBuild.PositionFromGridPosition(startPoint, 0));
    }


    public void AddDoorButtonclick()
    {
        currentBuild.OnStructurePartSelected += StartDoorPlacement;
    }


    void StartDoorPlacement(GridPosition startPoint)
    {
        currentBuild.OnStructurePartSelected -= StartWindowPlacement;

        WallLinePlacer wallLinePlacer = ItemPlacerObject.AddComponent<WallLinePlacer>();

        wallLinePlacer.SetWorkingStructure(itemSpawner, currentBuild, 0);
        int wallItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway");
        Item item = new Item(gameManager.Library.allItems[wallItemTypeIndex]);
        //itemHandler.AddObjectItemPlacer(item);
        itemHandler.gameObject.SetActive(true);

        GameObject[] outObjects;
        gameManager.StretchObjects.TryGetValue("Wall Doorway", out outObjects);
        wallLinePlacer.SetFirstPlacementObject(item, true, outObjects);
        wallLinePlacer.OnWallPlaced += WallPlacer_OnWallPlaced;
        wallLinePlacer.stretch = true;
        wallLinePlacer.FirstClick(currentBuild.PositionFromGridPosition(startPoint, 0));
    }

    public void SetExistingStructure(Structure structure)
    {
        currentBuild.SetNewStructure(structure);
        Rebuild(currentBuild.structure);
    }


    public void SaveCurrentStructure(string path)
    {
        gameManager.AddStructureToSavedList(currentBuild.structure);
        FileSaver.SaveStructureBluePrint(Application.dataPath + "/Resources/Structures/" + path + ".json", currentBuild.structure);
        currentBuild.DeselectObject();

    }


    public void LoadStructure(string path)
    {
        Structure structure = FileSaver.JsonToStructureBluePrint(Application.dataPath + "/Resources/Structures/" + path, gameManager.Library);
        StructurePlacer structurePlacer = ItemPlacerObject.AddComponent<StructurePlacer>();
        structurePlacer.SetPlacementStructureObject(Rebuild(structure).GetComponent<StructureObject>());
    }


    GameObject Rebuild(Structure structure)
    {
        GameObject newStructureObject = new GameObject(structure.Name);

        if (currentBuild != null)
        {
            currentBuild.DeselectObject();

        }
        currentBuild = newStructureObject.AddComponent<StructureObject>();
        currentBuild.SetNewStructure(structure);




        ItemObject[] objects = currentBuild.transform.GetComponentsInChildren<ItemObject>();
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i].gameObject);
            }
        }
        GameObject go = new GameObject();
        bool stretching = false;
        for (int i = 0; i < structure.walls.Count; i++)
        {


            float xPos = structure.walls[i].StartPoint.X * structure.GridSize;
            float zPos = structure.walls[i].StartPoint.Y * structure.GridSize;
            float floor = structure.walls[i].Floor * structure.FloorSize;
            Vector3 position = new Vector3(xPos, floor, zPos);
            if (structure.walls[i].StartPoint.X == structure.walls[i].EndPoint.X)
            {
                go.transform.Rotate(Vector3.up, -90);
            }
            go.transform.position = position;


            if (structure.walls[i].Stretch || lastStretching)
            {
                stretching = true;
                GameObject goA = HandleObjectSwap(stretching, structure.walls[i].item.itemType.TypeName);
                goA.transform.SetParent(currentBuild.transform);
                goA.transform.position = go.transform.position;
                goA.transform.rotation = go.transform.rotation;
            }

            else
            {
                stretching = false;
                itemSpawner.Spawn(structure.walls[i].item, go.transform, currentBuild.transform);
            }





            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(newStructureObject.transform);
            lastStretching = stretching;
        }


        Bounds bounds = BoundsMagic.CreateBoundsFromGameObject(gameObject);
        if (boxCollider == null)
        {
            boxCollider = newStructureObject.AddComponent<BoxCollider>();

        }
        boxCollider.size = bounds.size;

        return newStructureObject;
    }

    List<WallPlacement> stretchParts = new List<WallPlacement>();


    GameObject[] currentStretchObjects;
    bool lastStretching;
    private GameObject HandleObjectSwap(bool stretch, string baseTypeName)
    {

        if (!lastStretching)
        {
            gameManager.StretchObjects.TryGetValue(baseTypeName, out currentStretchObjects);
            GameObject go = Instantiate(currentStretchObjects[1]);
            go.SetActive(true);

            return go;
        }
        else if (stretch)
        {
            GameObject go = Instantiate(currentStretchObjects[2]);
            go.SetActive(true);

            return go;
        }
        else
        {
            GameObject go = Instantiate(currentStretchObjects[3]);
            go.SetActive(true);

            return go;
        }
    }






}
