using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TerrainCollider))]
public class TerrainClickDetect : MonoBehaviour
{
    public static event Action<Vector3> OnTerrainClickDetected;
    Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider == GetComponent<Collider>())
            {
                OnTerrainClickDetected?.Invoke(hit.point);
            }
         }
    }
}
