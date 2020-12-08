using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Editable),true)]
public class EditableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var editable = target as Editable;
        editable.SetEditMode(editable.editMode);
    }

}
