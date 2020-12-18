using System;
using UnityEngine;
using SpawnLogic;

public interface ISpawnRequester
{
    event Action<ISpawnable, Transform, Transform> OnSpawnRequest;

    void AssignSpawner(Spawner spawner);

    void SpawnRequest(ISpawnable item, Transform position, Transform parent = null);
}
