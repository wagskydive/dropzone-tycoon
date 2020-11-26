using System;
using UnityEngine;

public static class UiManager
{
    public static event Action<GameObject> OnRefresh;

    public static void RefreshAll(GameObject refresher)
    {
        OnRefresh?.Invoke(refresher);
    }

}
