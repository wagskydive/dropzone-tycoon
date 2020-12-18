using XNode;
using InventoryLogic;
using UnityEngine;

using XNodeEditor;

[CustomNodeEditor(typeof(ItemNode))]
public class ItemNodeEditor : NodeEditor
{
    private ItemNode itemNode;

    public override void OnHeaderGUI()
    {
        base.OnHeaderGUI();
    }

    public override void OnBodyGUI()
    {
        if (itemNode == null) itemNode = target as ItemNode;

        // Update serialized object's representation
        serializedObject.Update();



        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Catagory"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Description"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("recipe"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("ResourceOutput"));

        
        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }


}