using System;
using System.Collections.Generic;
using UnityEngine;
using SkydiveLogic;

public class AircraftObject : OperatableObject
{
    public event Action<Load> OnAircraftLoaded;
    public Aircraft aircraft;
    public void SubscribeToAircraft()
    {
        aircraft.OnAircraftLoaded += OnLoadedEvent;
    }

    void OnLoadedEvent(Load load)
    {
        OnAircraftLoaded?.Invoke(load);
    }

}
