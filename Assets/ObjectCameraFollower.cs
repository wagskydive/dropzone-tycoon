using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObjectCameraFollower : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    Transform originalTarget;

    private void Awake()
    {
        SelectableObject.OnObjectSelected += SetTarget;
    }
    void SetTarget(SelectableObject selectableObject)
    {
        originalTarget = vcam.m_Follow;
        vcam.m_Follow = selectableObject.transform;



        // ATTEMPT ON SELECTING PARENT STRUCTURE

        //if(selectableObject.transform.parent.GetComponent<StructureObject>() != null)
        //{
        //    vcam.m_Follow = selectableObject.transform.parent;
        //}
        //else
        //{
        //    vcam.m_Follow = selectableObject.transform;
        //}

    }
}
