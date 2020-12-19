using System;
using UnityEngine;



public abstract class SelectableObject : MonoBehaviour
{
    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action<SelectableObject> OnObjectDeselected;
    public static event Action<SelectableObject> OnMouseEnterDetected;
    public static event Action<SelectableObject> OnMouseExitDetected;

    public bool isSelected { get; private set; }

    public virtual void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isSelected)
            {
                SelectObject();
            }
        }
        else
        {
            if (isSelected)
            {
                DeselectObject();
            }
        }


    }

    public Bounds OuterBounds()
    {
        return BoundsMagic.CreateBoundsFromGameObject(gameObject);
    }

    public virtual void OnMouseEnter()
    {
        OnMouseEnterDetected?.Invoke(this);
    }
    public virtual void OnMouseExit()
    {
        OnMouseExitDetected?.Invoke(this);
    }
    public virtual void SelectObject()
    {
        isSelected = true;
        OnObjectSelected?.Invoke(this);
    }
    public virtual void DeselectObject()
    {
        isSelected = false;
        OnObjectDeselected?.Invoke(this);
    }

}
