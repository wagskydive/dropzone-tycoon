using System;
using UnityEngine;



public abstract class SelectableObject : MonoBehaviour
{
    public event Action<SelectableObject> OnClicked;
    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action<SelectableObject> OnObjectDeselected;
    public static event Action<SelectableObject> OnMouseEnterDetected;
    public static event Action<SelectableObject> OnMouseExitDetected;

    public static event Action OnSetAllSelectable;
    public static event Action OnSetAllNonSelectable;

    public Transform[] seats;

    public bool isSelected { get; private set; }

    public bool isCurrentlySelectable = false;

    private void Awake()
    {
        OnSetAllSelectable += SetSelectableTrue;
        OnSetAllNonSelectable += SetSelectableFalse;
        MouseDetect.OnRightClickDetected += MouseDetect_OnRightClickDetected;
    }

    private void MouseDetect_OnRightClickDetected(Vector3 obj)
    {
        if (isSelected)
        {
            DeselectObject();
        }
        isCurrentlySelectable = true;
    }

    public virtual void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isSelected && isCurrentlySelectable)
            {
                SelectObject();
            }
            OnClicked?.Invoke(this);
        }

    }


    public void SetAllSelectable()
    {
        OnSetAllSelectable?.Invoke();
    }
   
    public void SetAllNonSelectable()
    {
        OnSetAllNonSelectable?.Invoke();
    }

    void SetSelectableTrue()
    {
        SetSelectable(true);
    }
    
    void SetSelectableFalse()
    {
        SetSelectable(false);
    }


    public void SetSelectable(bool isSelectable)
    {
        isCurrentlySelectable = isSelectable;
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
        if (isCurrentlySelectable)
        {
            isSelected = true;

            OnObjectSelected?.Invoke(this);
            OnClicked?.Invoke(this);
        }

    }
    public virtual void DeselectObject()
    {
        isSelected = false;
        OnObjectDeselected?.Invoke(this);
    }

}
