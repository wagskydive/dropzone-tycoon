using System.Collections.Generic;
using StatsLogic;
using UnityEngine;


[SerializeField]
public class StatSave
{
    public string Name;
    public float Value;

    public StatSave(string statName, float val)
    { 
        Name = statName;
        Value = val;
    }
}
