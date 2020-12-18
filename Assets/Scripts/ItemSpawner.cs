using InventoryLogic;
using System;
using UnityEngine;
using SpawnLogic;

public class ItemSpawner : Spawner
{
    public event Action<ItemObject> OnItemSpawned;
    public static event Action<ItemSpawner> OnItemSpawnerStart;

    Item lastSpawnedItem;

    private void Start()
    {
        OnItemSpawnerStart?.Invoke(this);
    }

    public void SpawnItem(Item item, Transform position)
    {
        ItemObject itemObject = LastSpawn.AddComponent<ItemObject>();
        itemObject.item = new Item(item.itemType);

        lastSpawnedItem = item;
        OnItemSpawned?.Invoke(itemObject);
    }

    public override void Spawn(ISpawnable spawnable, Transform position, Transform parent = null)
    {
        base.Spawn(spawnable, position, parent);
        SpawnItem((Item)spawnable, position);
    }
}
