using InventoryLogic;
using System.Collections.Generic;
using UnityEngine;
using SpawnLogic;

public class ItemSpawner : Spawner
{
    Item lastSpawnedItem;

    public void SpawnItem(Item item, Vector3 position)
    {
        LastSpawn.AddComponent<ItemObject>().item = new Item(item.itemType);
        LastSpawn.AddComponent<ItemCollider>();
        lastSpawnedItem = item;
    }

    public override void Spawn(ISpawnable spawnable, Vector3 position, Transform parent = null)
    {
        base.Spawn(spawnable, position, parent);
        SpawnItem((Item)spawnable, position);
    }
}
