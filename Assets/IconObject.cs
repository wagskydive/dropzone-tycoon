using System;
using System.Collections.Generic;
using UnityEngine;

public class IconObject : MonoBehaviour
{
    public CinemachineTargetHandler cinemachineTargetHandler;
    public ShadowCatcher shadowCatcher;
    public ThumbnailFrom3dModel thumbnailFrom3DModel;


    private void Start()
    {
        thumbnailFrom3DModel.OnThumbnailDrag += RotateFromMouseDrag;
    }

    void RotateFromMouseDrag(Vector3 movement)
    {
        transform.Rotate(new Vector3(0,movement.x,0));
    }

    public void SetNewObject(GameObject go)
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
            shadowCatcher.SetPositionToBottomOfBounds(bounds);
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