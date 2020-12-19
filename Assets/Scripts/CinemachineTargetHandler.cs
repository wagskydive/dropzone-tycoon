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
        SelectableObject.OnObjectSelected += HandleSelectedObject;
    }

    void HandleSelectedObject(SelectableObject selectableObject)
    {
        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        target.target = selectableObject.transform;
        CinemachineTargetGroup.Target[] targets = new CinemachineTargetGroup.Target[1];
        targets[0] = target;
        targetGroup.m_Targets = targets ;
        SetTargetGroupTargetsAndRadius(selectableObject.OuterBounds());
    }

    public void SetTargetGroupTargetsAndRadius(Bounds bounds)
    {
        Vector3 extends = bounds.extents;

        float radius = Mathf.Max(extends.x, extends.y);
        radius = Mathf.Max(radius, extends.z);
        

        //CinemachineTargetGroup.Target[] targets = targetGroup.m_Targets;
        targetGroup.m_Targets[0].radius = radius;
        targetGroup.m_Targets[0].weight = 1;
    }


}
