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


    public void SetTargetGroupTargetsAndRadius(Bounds bounds)
    {
        Vector3 extends = bounds.extents;

        float radius = Mathf.Max(extends.x, extends.y);
        radius = Mathf.Max(radius, extends.z);
        

        CinemachineTargetGroup.Target[] targets = targetGroup.m_Targets;
        targets[0].radius = radius;
    }
}
