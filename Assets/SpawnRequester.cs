using System;
using System.Collections;
using System.Collections.Generic;
using InventoryLogic;
using SpawnLogic;
using UnityEngine;


public class SpawnRequester : MonoBehaviour, ISpawnRequester
{
    public event Action<ISpawnable, Vector3> OnSpawnRequest;

    public void AssignSpawner(Spawner spawner)
    {
        spawner.AddSpawnRequester(this);
    }

    public void SpawnRequest(ISpawnable item, Vector3 position)
    {
        OnSpawnRequest?.Invoke(item, position);
    }
}
