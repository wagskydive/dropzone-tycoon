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

    public void SpawnItem(Item item, Transform spawnTransformData)
    {
        ItemObject itemObject = LastSpawn.AddComponent<ItemObject>();
        itemObject.SetupItemInstance(item);

        lastSpawnedItem = item;
        OnItemSpawned?.Invoke(itemObject);
    }

    public override void Spawn(ISpawnable spawnable, Transform spawnTransformData, Transform parent = null)
    {
        base.Spawn(spawnable, spawnTransformData, parent);
        SpawnItem((Item)spawnable, spawnTransformData);
    }
}
