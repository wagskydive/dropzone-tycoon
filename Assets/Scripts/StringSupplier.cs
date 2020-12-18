using System;
using System.Collections.Generic;
using UnityEngine;

public class StringSupplier : MonoBehaviour
{
    public event Action<string> OnStringConfirmed;

    public void SupplyString(string supply)
    {
        OnStringConfirmed?.Invoke(supply);
    }
}
