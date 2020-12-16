using System;
using UnityEngine;
using SpawnLogic;

public interface ISpawnRequester
{
    event Action<ISpawnable, Vector3, Transform> OnSpawnRequest;

    void AssignSpawner(Spawner spawner);

    void SpawnRequest(ISpawnable item, Vector3 position, Transform parent = null);
}
