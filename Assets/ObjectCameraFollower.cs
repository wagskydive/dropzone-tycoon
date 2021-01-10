using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class ObjectCameraFollower : MonoBehaviour
{
    public static event Action<float> OnOrbit;

    public CinemachineVirtualCamera vcam;

    Transform originalTarget;

    private void Awake()
    {
        SelectableObject.OnObjectSelected += SetTarget;
        SelectableObject.OnObjectDeselected += ReleaseTarget;
        MouseDetect.OnMiddleClickDetected += DragStart;

    }
    void SetTarget(SelectableObject selectableObject)
    {

        transform.position = selectableObject.transform.position;
        transform.rotation = selectableObject.transform.rotation;

        transform.SetParent(selectableObject.transform);

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

    void ReleaseTarget(SelectableObject selectableObject)
    {
        transform.parent = null;
    }
    
    

    Vector3 startDragPosition;
    private void Update()
    {
        Orbit();
        Zoom();
        //if (Input.GetMouseButtonDown(2))
        //{
        //    mouseMiddleClickPos = Input.mousePosition;
        //    startDragPosition = transform.position;
        //
        //}
        //if (Input.GetMouseButton(2))
        //{
        //    
        //    if (transform.parent != null)
        //    {
        //        transform.parent = null;
        //    }
        //
        //    Vector3 difference = Input.mousePosition - mouseMiddleClickPos;
        //    Vector3 conversion = new Vector3(-difference.x/10, 0, -difference.y/10);
        //
        //    transform.position = startDragPosition + conversion;
        //
        //}
    }

    float zoomLevel = 0;

    void Zoom()
    {
        float zoom = Input.mouseScrollDelta.y;
        if(zoom != 0)
        {
            CinemachineOrbitalTransposer transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            Vector3 zoomVector = new Vector3(0, -zoom*2, zoom);

            Vector3 currentOffSet = transposer.m_FollowOffset;
            zoomVector += transposer.m_FollowOffset;
            zoomVector.y = Mathf.Clamp(zoomVector.y, 2.5f, Mathf.Infinity);
            zoomVector.z = Mathf.Clamp(zoomVector.z, -Mathf.Infinity, -5);

            transposer.m_FollowOffset = zoomVector;
        }


    }

    float orbitSpeed = 3;

    void Orbit()
    {
        float screenWidth = Screen.width;
        float mousePos = Input.mousePosition.x;
        //Debug.Log(mousePos);
        if (mousePos < screenWidth * .1f && mousePos > 0)
        {
            float offset = ((mousePos / screenWidth) - .1f)* orbitSpeed;
            //Orbit((offset-1)*3);
            vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = offset;
            OnOrbit?.Invoke(offset);
        }
        else if (mousePos > screenWidth * .9f && mousePos < screenWidth)
        {
            float offset = ((mousePos/ screenWidth) - .9f)* orbitSpeed;
            //Orbit(offset * 3);
            vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = offset;
            OnOrbit?.Invoke(offset);
        }
        else
        {
            vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;

        }
    }

    void DragStart(Vector3 pos)
    {
        startDragPosition = pos;
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        MouseDetect.OnOverDetected += Drag;
        MouseDetect.OnMiddleUpClickDetected += DragEnd;
    }

    void Drag(Vector3 pos)
    {
        transform.position = startDragPosition+( startDragPosition-pos);
    }
    void DragEnd(Vector3 pos)
    {
        MouseDetect.OnOverDetected -= Drag;
        MouseDetect.OnMiddleUpClickDetected -= DragEnd;
    }
}
