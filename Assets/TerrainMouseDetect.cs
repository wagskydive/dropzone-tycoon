using System;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(TerrainCollider))]
public class TerrainMouseDetect : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Vector3, Transform> OnTerrainLeftClickDetected;
    public static event Action<Vector3> OnTerrainOverDetected;


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
                OnTerrainOverDetected?.Invoke(hit);
            }

        }

        else if(ActiveMousePositionDetector)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray);
            OnTerrainOverDetected?.Invoke(ray.origin+ray.direction*25);
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
                OnTerrainLeftClickDetected?.Invoke(TerrainHit(), transform);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }


}
