using UnityEngine;
using System;
using SpawnLogic;

public abstract class ItemHandler : MonoBehaviour
{
    public event Action<ISpawnable, bool, float> OnItemPassed;

    [SerializeField]
    public  bool snapping;

    [SerializeField]
    public float gridSize = 1;



    public virtual void PassItem(ISpawnable spawnable, bool snap, float gridSize)
    {
        OnItemPassed?.Invoke(spawnable, snap, gridSize);
    }

    public virtual void AddObjectItemPlacer(ISpawnable objectToAdd)
    {

        PassItem(objectToAdd, snapping, gridSize);

    }
}
