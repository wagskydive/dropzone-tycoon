﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Collider))]
public class MouseDetect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Vector3, Transform> OnLeftClickDetected;
    public static event Action<Vector3, Transform> OnRightClickDetected;
    public static event Action<Vector3, Transform> OnMiddleClickDetected;
    public static event Action<Vector3, Transform> OnLeftUpClickDetected;
    public static event Action<Vector3, Transform> OnRightUpClickDetected;
    public static event Action<Vector3, Transform> OnMiddleUpClickDetected;
    public static event Action<Vector3> OnOverDetected;


    Camera camera;

    [SerializeField]
    private bool ActiveMousePositionDetector;

    bool mouseIsOver;

    private void Awake()
    {
        camera = Camera.main;
    }

    public void ActivateMouseOverDetect(bool active)
    {
        ActiveMousePositionDetector = active;
    }

    Vector3 TerrainHit()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray);
        


        if (hits != null && hits.Length > 0)
        {
            if (hits[0].collider.isTrigger)
            {
                return Vector3.zero;
            }
            else
            {
                return hits[0].point;
            }



            if (hits[hits.Length - 1].collider.isTrigger)
            {
                return Vector3.zero;
            }
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == gameObject.GetComponent<Collider>())
                {

                    return hits[i].point;
                }
            }

        }
        return Vector3.zero;
    }

    private void Update()
    {
        if(ActiveMousePositionDetector)
        {
            Vector3 hit = TerrainHit();
            if(hit != Vector3.zero)
            {
                OnOverDetected?.Invoke(hit);
            }

        }

        else if(ActiveMousePositionDetector)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray);
            OnOverDetected?.Invoke(ray.origin+ray.direction*25);
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (mouseIsOver)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClickDetected?.Invoke(TerrainHit(), transform);
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClickDetected?.Invoke(TerrainHit(), transform);
            }
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                OnMiddleClickDetected?.Invoke(TerrainHit(), transform);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftUpClickDetected?.Invoke(TerrainHit(), transform);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightUpClickDetected?.Invoke(TerrainHit(), transform);
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnMiddleUpClickDetected?.Invoke(TerrainHit(), transform);
        }
    }
}
