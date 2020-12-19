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


public class StructureBuilder : MonoBehaviour
{


    public StructureObject currentBuild;

    public ItemSpawner itemSpawner;

    public GameObject ItemPlacerObject;

    public ItemHandler itemHandler;

    BoxCollider boxCollider;

    public GameManager gameManager;
    GameObject go;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        go = new GameObject();

        SelectableObject.OnObjectSelected += HandleSelected;

        itemSpawner.OnItemSpawned += ItemSpawner_OnItemSpawned;
        //Invoke("SetNewStructure", 3);

        ItemPlacer.OnPlacementComplete += ItemPlacer_OnPlacementComplete;
    }

    private void ItemPlacer_OnPlacementComplete(ItemPlacer obj)
    {
        Destroy(obj);
    }

    private void ItemSpawner_OnItemSpawned(ItemObject obj)
    {
        currentBuild.AddItemToStructure(obj);
    }

    private void HandleSelected(SelectableObject selected)
    {
        if (selected.GetType() == typeof(StructureObject))
        {
            currentBuild = (StructureObject)selected;
        }
        else
        {
            currentBuild = null;
        }
    }


    public void StartNewStructure()
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

    private void WallPlacer_OnWallPlaced(WallPlacement wall)
    {
        currentBuild.structure.AddPartAsWall(wall);
    }

    public void AddWindowsAndDoors()
    {
        WallLinePlacer wallLinePlacer = ItemPlacerObject.AddComponent<WallLinePlacer>();
        int wallstartItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway Start");
        Item StartItem = new Item(gameManager.Library.allItems[wallstartItemTypeIndex]);

        int wallEndItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway End");
        Item EndItem = new Item(gameManager.Library.allItems[wallstartItemTypeIndex]);
        
        int wallSingleItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway");
        Item SingleItem = new Item(gameManager.Library.allItems[wallstartItemTypeIndex]);
        
        int wallMiddleItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway Middle");
        Item MiddleItem = new Item(gameManager.Library.allItems[wallstartItemTypeIndex]);

        wallLinePlacer.SetWorkingStructure(itemSpawner, currentBuild, 0);

        itemHandler.gameObject.SetActive(true);
        wallLinePlacer.SetFirstPlacementObject(MiddleItem, true, StartItem, EndItem, SingleItem);

        wallLinePlacer.OnWallPlaced += WallPlacer_OnWallPlaced;
    }


    public void SetExistingStructure(Structure structure)
    {
        currentBuild.SetNewStructure(structure);
        Rebuild(currentBuild.structure);
    }
    int saveAmounts = 0;

    public void SaveCurrentStructure()
    {
        gameManager.AddStructureToSavedList(currentBuild.structure);
        FileSaver.SaveStructureBluePrint(Application.dataPath + "/Resources/Structures/" + currentBuild.structure.Name + saveAmounts.ToString() + ".json", currentBuild.structure);
        currentBuild.DeselectObject();
        saveAmounts++;
    }


    public void LoadButtonPress()
    {
        Structure structure = FileSaver.JsonToStructureBluePrint(Application.dataPath + "/Resources/Structures/" + "My New Structure" + "0.json", gameManager.Library);
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

            itemSpawner.Spawn(structure.walls[i].item, go.transform, currentBuild.transform);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(newStructureObject.transform);
        }


        Bounds bounds = BoundsMagic.CreateBoundsFromGameObject(gameObject);
        if (boxCollider == null)
        {
            boxCollider = newStructureObject.AddComponent<BoxCollider>();

        }
        boxCollider.size = bounds.size;

        return newStructureObject;
    }
}
