using System;
using UnityEngine;
using UnityEngine.EventSystems;



public class MouseDetect : MonoBehaviour
{
    public static event Action<Vector3> OnLeftClickDetected;
    public static event Action<Vector3> OnRightClickDetected;
    public static event Action<Vector3> OnMiddleClickDetected;
    public static event Action<Vector3> OnLeftUpClickDetected;
    public static event Action<Vector3> OnRightUpClickDetected;
    public static event Action<Vector3> OnMiddleUpClickDetected;
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

    Vector3 NonTriggerColliderHit()
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

        }
        return Vector3.zero;
    }

    private void Update()
    {
        if (ActiveMousePositionDetector)
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnOverDetected?.Invoke(hit);
            }

        }

        //else if(ActiveMousePositionDetector)
        //{
        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //    Physics.Raycast(ray);
        //    OnOverDetected?.Invoke(ray.origin+ray.direction*25);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnLeftClickDetected?.Invoke(hit);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnRightClickDetected?.Invoke(hit);
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnMiddleClickDetected?.Invoke(hit);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnLeftUpClickDetected?.Invoke(hit);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnRightUpClickDetected?.Invoke(hit);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            Vector3 hit = NonTriggerColliderHit();
            if (hit != Vector3.zero)
            {
                OnMiddleUpClickDetected?.Invoke(hit);
            }
        }

    }

}
