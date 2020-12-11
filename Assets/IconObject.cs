using System;
using System.Collections.Generic;
using UnityEngine;

public class IconObject : MonoBehaviour
{
    public CinemachineTargetHandler cinemachineTargetHandler;
    public ShadowCatcher ShadowCatcher;


    public void SetNewObject(GameObject go, string itemName)
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        go.transform.SetParent(transform);
        go.transform.localRotation = Quaternion.identity;
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = cinemachineTargetHandler.CreateBoundsFromTransform(go.transform);
            cinemachineTargetHandler.SetTargetGroupTargetsAndRadius(bounds);
            if (renderer.bounds.center != Vector3.zero)
            {
                go.transform.Translate(-renderer.bounds.center);
            }
        }
   }


    public void UnLoadObject()
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
    }
}