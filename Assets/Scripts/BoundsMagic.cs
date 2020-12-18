using UnityEngine;

public static class BoundsMagic
{
    public static Bounds CreateBoundsFromGameObject(GameObject childTransform)
    {
        Bounds bounds = new Bounds();
        bounds.Encapsulate(childTransform.GetComponent<Renderer>().bounds);

        Renderer[] childRenderers = childTransform.GetComponentsInChildren<Renderer>();
        if (childRenderers != null)
        {

            for (int i = 0; i < childRenderers.Length; i++)
            {
                bounds.Encapsulate(childRenderers[i].bounds);
            }

        }
        return bounds;

    }    public static Bounds CreateBoundsFromGameObjectFiletr(GameObject childTransform)
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
