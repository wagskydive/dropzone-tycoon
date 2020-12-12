using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ThumbnailFrom3dModel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    public event Action<Vector3> OnThumbnailDrag;
    Image image;

    public ImageSaver imageSaver;

    Texture2D currentImage;

    bool isInside;
    bool isClicking;

    bool dragStarted;
    Vector3 lastMousePosition;

    private void Start()
    {
        image = GetComponent<Image>();

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
            OnThumbnailDrag?.Invoke(Input.mousePosition - lastMousePosition);

            lastMousePosition = Input.mousePosition;
        }
    }

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
}
