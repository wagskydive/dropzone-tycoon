using InventoryLogic;
using UnityEngine;

public class StructureObject : SelectableObject
{
    [SerializeField]
    public Structure structure { get; private set; }

    ItemSpawner currentItemSpawner;

    BoxCollider boxCollider;
    private void Awake()
    {


    }

    void SetColliderToChildrenBounds()
    {
        Bounds bounds = BoundsMagic.CreateBoundsFromGameObject(gameObject, transform);
        boxCollider.size = bounds.size;
        boxCollider.center = bounds.center;
    }

    public void SetNewStructure(Structure str)
    {
        structure = str;
        

    }

    public void AddItemToStructure(ItemObject itemObject)
    {
        itemObject.transform.SetParent(transform);
        //structure.AddPart(itemObject.item);
        
        if (boxCollider == null)
        {
            boxCollider = gameObject.GetComponent<BoxCollider>();
            if(boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            boxCollider.isTrigger = true;
        }


        SetColliderToChildrenBounds();
        
    }

    public void AssignSpawner(ItemSpawner spawner)
    {
        currentItemSpawner = spawner;
        spawner.OnItemSpawned += AddItemToStructure;
    }

    //public override void OnMouseEnter()
    //{
    //    //boxCollider.enabled = false;
    //    //OnMouseEnterDetected?.Invoke(this);
    //}

    public override void SelectObject()
    {
        base.SelectObject();
    }
    public override void DeselectObject()
    {
        base.DeselectObject();
        currentItemSpawner.OnItemSpawned -= AddItemToStructure;
    }

}
