using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCompass : MonoBehaviour
{
    public Transform aircraft;

    public Transform compass;

    private void Update()
    {
        float rotationOffset = Vector3.SignedAngle(Vector3.forward, aircraft.forward, Vector3.up);
        compass.rotation = Quaternion.AngleAxis(rotationOffset, Vector3.forward);
    }
}
