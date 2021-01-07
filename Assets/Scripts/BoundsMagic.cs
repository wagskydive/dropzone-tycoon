using UnityEngine;

public static class BoundsMagic
{
    public static Bounds CreateBoundsFromGameObject(GameObject subject, Transform parent = null)
    {
        Bounds bounds = new Bounds();
        Renderer renderer = subject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds rendererBounds = subject.GetComponent<Renderer>().bounds;
            if (parent != null)
            {
                rendererBounds.center -= parent.position;
            }
            //rendererBounds.center -= subject.transform.parent.position; 
            bounds.Encapsulate(rendererBounds);
        }

        if (subject.transform.childCount > 0)
        {
            Renderer[] childRenderers = subject.GetComponentsInChildren<Renderer>();
            if (childRenderers != null)
            {

                for (int i = 0; i < childRenderers.Length; i++)
                {
                    if (childRenderers[i] != renderer)
                    {
                        bounds.Encapsulate(CreateBoundsFromGameObject(childRenderers[i].gameObject, subject.transform));

                    }
                }

            }
        }

        return bounds;

    }

    public static Bounds CreateBoundsFromGameObjectFilter(GameObject childTransform)
    {
        Bounds bounds = new Bounds();
        bounds.Encapsulate(childTransform.GetComponent<MeshFilter>().mesh.bounds);

        MeshFilter[] childMeshFilters = childTransform.GetComponentsInChildren<MeshFilter>();
        if (childMeshFilters != null)
        {

            for (int i = 0; i < childMeshFilters.Length; i++)
            {
                bounds.Encapsulate(childMeshFilters[i].mesh.bounds);
            }

        }
        return bounds;

    }
}
