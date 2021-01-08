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

    [SerializeField]
    public Transform[] seats;

    public int SeatOccupationBinaryNumber = 0;

    public bool isSelected { get; private set; }

    public bool isCurrentlySelectable = false;

    public virtual void Awake()
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

    public bool IsSeatFree(int seatIndex)
    {
        int check = SeatOccupationBinaryNumber / ((seatIndex + 1) * (seatIndex + 1));
        if (check % 2 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int FindFreeSeat(bool nonDriver = false)
    {
        int startInt = 0;
        if (nonDriver)
        {
            startInt = 1;
        }
        for (int i = startInt; i < seats.Length; i++)
        {
            if (IsSeatFree(i))
            {
                return i;
            }
             
        }
        return -1;
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
