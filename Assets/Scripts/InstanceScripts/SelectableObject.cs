using System;
using UnityEngine;



public abstract class SelectableObject : MonoBehaviour
{
    public static event Action<SelectableObject> OnObjectSelected;
    public static event Action<SelectableObject> OnMouseEnterDetected;
    public static event Action<SelectableObject> OnMouseExitDetected;



    private void OnMouseDown()
    {
        SelectObject();
    }
    private void OnMouseEnter()
    {
        OnMouseEnterDetected?.Invoke(this);
    }

    private void OnMouseExit()
    {
        OnMouseExitDetected?.Invoke(this);
    }


    public virtual void SelectObject()
    {
        OnObjectSelected?.Invoke(this);
    }

}
