using UnityEngine;

public static class ColliderAdder
{



    public static Bounds AddMeshCollidersInChildren(GameObject parentObject)
    {
        MeshRenderer parentRenderer = parentObject.GetComponent<MeshRenderer>();
        Bounds meshBounds = new Bounds();
        if (parentRenderer != null)
        {
            meshBounds = parentRenderer.bounds;
        }
        
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            MeshRenderer meshRenderer = parentObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>();
            meshBounds = meshRenderer.bounds;
            if (meshRenderer != null)
            {
                MeshCollider meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshBounds.Encapsulate(AddMeshCollidersInChildren(meshRenderer.gameObject));
            }
        }

        return meshBounds;
    }
}
