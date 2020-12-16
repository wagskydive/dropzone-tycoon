using UnityEngine;
using System;
using SpawnLogic;

public abstract class ItemHandler : MonoBehaviour
{
    public event Action<ISpawnable> OnItemPassed;

    public virtual void PassItem(ISpawnable spawnable)
    {
        OnItemPassed?.Invoke(spawnable);
    }
}
