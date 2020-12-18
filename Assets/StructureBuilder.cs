using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;

public static class DummyStructureBuilder
{
    public static Structure AllWalls(int length, int width, Item item)
    {
        Structure structure = new Structure(1,2.4f);
        for (int i = 0; i < length; i++)
        {

            for (int j = 0; j < width; j++)
            {
                //x wall
                structure.AddPartAsWall(i, j, i + 1, j, item,0);
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


    public ManagementScripts.GameManager gameManager;
    GameObject go;
    private void Awake()
    {
        go = new GameObject();
        //Invoke("SetNewStructure", 3);
    }

    public void AddWall(GridPosition start,GridPosition end, Item item)
    {

    }

    public void SetNewStructure(Structure structure)
    {
        int wallItemTypeIndex = gameManager.Library.IndexFromTypeName("Wall Doorway");
        Item item = new Item(gameManager.Library.allItems[wallItemTypeIndex]);
        currentBuild.SetNewStructure(structure);
        Rebuild(currentBuild.structure);
    }

    void Rebuild(Structure structure)
    {
        ItemObject[] objects = currentBuild.transform.GetComponentsInChildren<ItemObject>();
        if(objects != null)
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
            float floor = structure.walls[i].Floor * structure.StorySize;
            Vector3 position = new Vector3(xPos,floor, zPos);
            if (structure.walls[i].StartPoint.X == structure.walls[i].EndPoint.X)
            {
                go.transform.Rotate(0, -90, 0);
            }
            go.transform.position = position;

            itemSpawner.Spawn(structure.walls[i].item,go.transform, currentBuild.transform);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }
    }
}
