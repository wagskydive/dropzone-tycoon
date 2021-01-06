using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunwayObject : SelectableObject
{
    public float Direction { get => GetDirection(); }

    public Transform StartPoint;
    public Transform EndPoint;

    private float GetDirection()
    {
        return Vector3.SignedAngle(Vector3.forward, transform.forward.normalized, Vector3.up);
    }

}
