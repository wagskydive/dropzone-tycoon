﻿using System;
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
        GameObject go = Instantiate(Resources.Load(resourcePath)) as GameObject;
        if(parent != null)
        {
            go.transform.SetParent(parent);
        }
        
        go.transform.position = position;
        LastSpawn = go;
        OnSpawned?.Invoke(go.GetComponent<ISpawnable>());
    }
}
