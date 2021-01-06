using System;
using System.Collections.Generic;
using UnityEngine;

public class GoToTaskUi : MonoBehaviour
{
    public event Action<Vector3> OnClick;
    public event Action OnRightClick;

    public MouseDetect mouseDetect;
    private void Awake()
    {

        MouseDetect.OnOverDetected += SetPositionFromMouse;
        MouseDetect.OnLeftClickDetected += ClickDetected;
        MouseDetect.OnRightClickDetected += RightClickDetected;
    }

    void ClickDetected(Vector3 position)
    {
        OnClick?.Invoke(position);
    }
    void RightClickDetected(Vector3 position)
    {
        OnRightClick?.Invoke();
    }


    void SetPositionFromMouse(Vector3 position)
    {

        transform.position = position;
    }

}
