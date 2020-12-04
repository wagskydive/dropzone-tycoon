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

    public HotField hotField;

    public Text DisplayText;

    public void EnableEditMode()
    {
        SetEditMode(true);
    }

    public void DisableEditMode()
    {
        SetEditMode(false);
    }

    public void SetEditMode(bool editable)
    {
        editMode = editable;
        if (editable)
        {
            SetObjectsActive(staticUI, false);
            SetObjectsActive(editUI, true);
        }
        else
        {
            SetObjectsActive(editUI, false);
            SetObjectsActive(staticUI, true);
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
        DisplayText.text = text;
    }

    public virtual void AssignNode(string skillName, SkillNode skillNode)
    {
    }
}
