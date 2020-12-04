using UnityEngine;
using UnityEditor;
using PathCreation;

namespace PathCreation.Examples
{
    //[CustomEditor(typeof(PathSceneToolMultiples), true)]
    public class PathSceneToolMultiplesEditor : Editor
    {
        //protected PathSceneToolMultiples pathTool;
        //bool isSubscribed;
        //
        //public override void OnInspectorGUI()
        //{
        //    using (var check = new EditorGUI.ChangeCheckScope())
        //    {
        //        DrawDefaultInspector();
        //
        //        if (check.changed)
        //        {
        //            if (!isSubscribed)
        //            {
        //                TryFindPathCreator();
        //                Subscribe();
        //            }
        //
        //            if (pathTool.autoUpdate)
        //            {
        //                TriggerUpdate();
        //
        //            }
        //        }
        //    }
        //
        //    if (GUILayout.Button("Manual Update"))
        //    {
        //        if (TryFindPathCreator())
        //        {
        //            TriggerUpdate();
        //            SceneView.RepaintAll();
        //        }
        //    }
        //
        //}
        //
        //
        //void TriggerUpdate()
        //{
        //    if (pathTool.pathCreators != null)
        //    {
        //        pathTool.TriggerUpdate();
        //    }
        //}
        //
        //
        //protected virtual void OnPathModified()
        //{
        //    if (pathTool.autoUpdate)
        //    {
        //        TriggerUpdate();
        //    }
        //}
        //
        //protected virtual void OnEnable()
        //{
        //    pathTool = (PathSceneToolMultiples)target;
        //    pathTool.onDestroyed += OnToolDestroyed;
        //
        //    if (TryFindPathCreator())
        //    {
        //        Subscribe();
        //        TriggerUpdate();
        //    }
        //}
        //
        //void OnToolDestroyed()
        //{
        //    if (pathTool != null)
        //    {
        //        for (int i = 0; i < pathTool.pathCreators.Count; i++)
        //        {
        //            pathTool.pathCreators[i].pathUpdated -= OnPathModified;
        //        }
        //        
        //    }
        //}
        //
        //
        //protected virtual void Subscribe()
        //{
        //    if (pathTool.pathCreators != null)
        //    {
        //        isSubscribed = true;
        //        for (int i = 0; i < pathTool.pathCreators.Count; i++)
        //        {
        //            pathTool.pathCreators[i].pathUpdated -= OnPathModified;
        //            pathTool.pathCreators[i].pathUpdated += OnPathModified;
        //        }
        //    }
        //}

        //bool TryFindPathCreator()
        //{
        //    // Try find a path creator in the scene, if one is not already assigned
        //    if (pathTool.pathCreators == null)
        //    {
        //        if (pathTool.GetComponent<PathCreator>() != null)
        //        {
        //            pathTool.pathCreators.Add(pathTool.GetComponent<PathCreator>());
        //        }
        //        else if (FindObjectOfType<PathCreator>())
        //        {
        //            pathTool.pathCreators.Add(FindObjectOfType<PathCreator>());
        //        }
        //    }
        //    return pathTool.pathCreators != null;
        //}
    }
}