using InventoryLogic;
using UnityEngine;

public class StructureObject : MonoBehaviour
{
    public Structure structure { get; private set; }



    private void Awake()
    {
        if (structure == null)
        {
            structure = new Structure(1,2.4f);
        }
    }


    public void SetNewStructure(Structure str)
    {
        structure = str;
    }

    public void AddItemToStructure(ItemObject itemObject)
    {
        itemObject.transform.SetParent(transform);
        structure.AddPart(itemObject.item);
    }

    public void AssignSpawner(ItemSpawner spawner)
    {
        spawner.OnItemSpawned += AddItemToStructure;
    }

}
