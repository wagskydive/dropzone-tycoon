using UnityEngine;
using System;
using SpawnLogic;

public abstract class ItemHandler : MonoBehaviour
{
    public event Action<ISpawnable, bool, float> OnItemPassed;

    public virtual void PassItem(ISpawnable spawnable, bool snap, float gridSize)
    {
        OnItemPassed?.Invoke(spawnable, snap, gridSize);
    }
}
