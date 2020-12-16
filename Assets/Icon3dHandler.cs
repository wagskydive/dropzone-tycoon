using UnityEngine;
using UnityEngine.EventSystems;

public class Icon3dHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{

    [SerializeField]
    public IconObject iconObject;

    bool isInside;
    bool isClicking;

    bool dragStarted;
    Vector3 lastMousePosition;
    public void OnPointerEnter(PointerEventData eventData)
    {

        isInside = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragStarted = false;
        isClicking = false;
    }

    private void Update()
    {
        if (isClicking)
        {
            if (!dragStarted)
            {
                lastMousePosition = Input.mousePosition;
                dragStarted = true;
            }
            iconObject.RotateFromMouseDrag(Input.mousePosition - lastMousePosition);

            lastMousePosition = Input.mousePosition;
        }
    }
}
