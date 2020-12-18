using System;
using System.Collections;
using System.Collections.Generic;
using InventoryLogic;
using SpawnLogic;
using UnityEngine;


public class SpawnRequester : MonoBehaviour, ISpawnRequester
{
    public event Action<ISpawnable, Transform, Transform> OnSpawnRequest;

    public void AssignSpawner(Spawner spawner)
    {
        spawner.AddSpawnRequester(this);
    }

    public void SpawnRequest(ISpawnable item, Transform position, Transform parent = null)
    {
        OnSpawnRequest?.Invoke(item, position, parent);
    }
}
