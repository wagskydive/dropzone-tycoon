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
    }

    public override void Spawn(ISpawnable spawnable, Vector3 position)
    {
        base.Spawn(spawnable, position);
        SpawnItem((Item)spawnable, position);
    }
}
