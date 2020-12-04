using UnityEngine;
using PathCreation;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract class PathSceneToolMultiples : MonoBehaviour
{
    public event System.Action onDestroyed;
    public List<PathCreator> pathCreators;
    public bool autoUpdate = true;

    protected VertexPath[] path
    {
        get
        {
            return returnPathsFromCreators();
        }
    }

    VertexPath[] returnPathsFromCreators()
    {
        VertexPath[] pths = new VertexPath[pathCreators.Count];
        for (int i = 0; i < pathCreators.Count; i++)
        {
            pths[i] = pathCreators[i].path;
        }
        return pths;
    }

    public void TriggerUpdate()
    {
        PathUpdated();
    }


    protected virtual void OnDestroy()
    {
        if (onDestroyed != null)
        {
            onDestroyed();
        }
    }

    protected abstract void PathUpdated();
}
