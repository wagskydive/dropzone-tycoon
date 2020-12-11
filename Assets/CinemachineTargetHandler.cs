using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTargetHandler : MonoBehaviour
{
    //public IconObject target;
    CinemachineTargetGroup targetGroup;
    private void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public Bounds CreateBoundsFromTransform(Transform childTransform)
    {
        Bounds bounds = new Bounds();
        bounds.Encapsulate(childTransform.GetComponent<Renderer>().bounds);

        Renderer[] childRenderers = childTransform.GetComponentsInChildren<Renderer>();
        if (childRenderers != null)
        {

            for (int i = 0; i < childRenderers.Length; i++)
            {
                bounds.Encapsulate(childRenderers[i].bounds);
            }

        }
        return bounds;
        
    }

    public void SetTargetGroupTargetsAndRadius(Bounds bounds)
    {
        Vector3 extends = bounds.extents;

        float radius = Mathf.Max(extends.x, extends.y);
        radius = Mathf.Max(radius, extends.z);
        

        CinemachineTargetGroup.Target[] targets = targetGroup.m_Targets;
        targets[0].radius = radius;
    }
}
