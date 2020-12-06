using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Editable : MonoBehaviour
{
    public virtual event Action<string> OnEdited;

    public List<GameObject> editUI;
    public List<GameObject> staticUI;

    public bool editMode = false;

    public virtual bool addMode { get; private set; }

    public HotField hotField;

    public Text DisplayText;

    public void EnableEditMode()
    {
        SetEditMode(true);

    }

    public void SetAddMode(bool mode)
    {
        addMode = mode;
    }

    public void DisableEditMode()
    {
        SetEditMode(false);
    }

    public void ToggleEditMode()
    {
        SetEditMode(!editMode);
    }

    public void SetEditMode(bool editable)
    {
        editMode = editable;
        if (editable)
        {
            if(staticUI != null && staticUI.Count > 0)
            {
                SetObjectsActive(staticUI, false);
            }
            if (editUI != null && editUI.Count > 0)
            {
                SetObjectsActive(editUI, true);
            }
                
        }
        else
        {
            if (staticUI != null && staticUI.Count > 0)
            {
                SetObjectsActive(staticUI, true);
            }
            if (editUI != null && editUI.Count > 0)
            {
                SetObjectsActive(editUI, false);
            }
        }
    }

    void SetObjectsActive(List<GameObject> objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }

    public virtual void EditConfirmed()
    {
        DisableEditMode();

        OnEdited?.Invoke(hotField.field);
    }
    public virtual void SetDisplaytext(string text)
    {
        if(DisplayText != null)
        {
            DisplayText.text = text;
        }        
    }

    public virtual void AssignNode(string skillName, SkillNode skillNode)
    {
        Unsubscribe(OnEdited);
    }


    void Unsubscribe(Action<string> myDlgHandler)
    {
        Delegate[] clientList = myDlgHandler.GetInvocationList();
        foreach (var d in clientList)
            myDlgHandler -= (d as Action<string>);

        if (myDlgHandler != null)
            foreach (var d in myDlgHandler.GetInvocationList())
                myDlgHandler -= (d as Action<string>);
    }

}
