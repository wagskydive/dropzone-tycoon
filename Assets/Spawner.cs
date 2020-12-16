using System;
using UnityEngine;
using SpawnLogic;

public abstract class Spawner : MonoBehaviour
{
    public GameObject LastSpawn;
    public static event Action<ISpawnable> OnSpawned;

    public void AddSpawnRequester(ISpawnRequester spawnRequester)
    {
        spawnRequester.OnSpawnRequest += Spawn;
    }

    public void RemoveSpawnRequester(ISpawnRequester spawnRequester)
    {
        spawnRequester.OnSpawnRequest -= Spawn;
    }

    public virtual void Spawn(ISpawnable spawnable, Vector3 position, Transform parent = null)
    {
        string resourcePath = spawnable.ResourcePath();
        GameObject SpawnParent = new GameObject();
        

        GameObject go = Instantiate(Resources.Load(resourcePath), SpawnParent.transform) as GameObject;
        if (parent != null)
        {
            SpawnParent.transform.SetParent(parent);
        }

        SpawnParent.transform.position = position;
        LastSpawn = SpawnParent;
        OnSpawned?.Invoke(go.GetComponent<ISpawnable>());
    }
}
