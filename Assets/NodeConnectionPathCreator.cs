using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class NodeConnectionPathCreator : PathSceneToolMultiples
{
    public GameObject pathPrefab;

    public float roadWidth = 4f;

    public bool flattenSurface;


    [Header("Material settings")]
    public Material pathMaterial;


    [SerializeField, HideInInspector]
    List<GameObject> meshHolder;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;


    protected override void PathUpdated()
    {
        if (pathCreators != null)
        {
            for (int i = 0; i < pathCreators.Count; i++)
            {
                AssignMeshComponents(i);
                AssignMaterials();
                CreateMesh(pathCreators[i].path);
            }

        }
    }
    public void CreateRequirementPath(Vector3 ownConnector, Vector3 requirementsPoints)
    {
        if(pathCreators == null)
        {
            pathCreators = new List<PathCreator>();
        }
        GameObject go = Instantiate(pathPrefab);
        PathCreator pathCre = go.GetComponent<PathCreator>();
        Vector3[] points = new Vector3[2];


        points[0] = ownConnector;
        points[1] = requirementsPoints;
        pathCre.bezierPath = new BezierPath(points,false, PathSpace.xy);
        pathCreators.Add(pathCre);
        int index = pathCreators.Count - 1;
        AssignMeshComponents(index);
        AssignMaterials();
        CreateMesh(pathCre.path);
        
    }



    void CreateMesh(VertexPath vPath)
    {
        Vector3[] verts = new Vector3[vPath.NumPoints * 8];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];

        int numTris = 2 * (vPath.NumPoints - 1) + ((vPath.isClosedLoop) ? 2 : 0);
        int[] roadTriangles = new int[numTris * 3];


        int vertIndex = 0;
        int triIndex = 0;

        // Vertices for the top of the road are layed out:
        // 0  1
        // 8  9
        // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
        int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
        int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

        bool usePathNormals = !(vPath.space == PathSpace.xyz && flattenSurface);

        for (int i = 0; i < vPath.NumPoints; i++)
        {
            Vector3 localUp = (usePathNormals) ? Vector3.Cross(vPath.GetTangent(i), vPath.GetNormal(i)) : vPath.up;
            Vector3 localRight = (usePathNormals) ? vPath.GetNormal(i) : Vector3.Cross(localUp, vPath.GetTangent(i));

            // Find position to left and right of current path vertex
            Vector3 vertSideA = vPath.GetPoint(i) - localRight * Mathf.Abs(roadWidth);
            Vector3 vertSideB = vPath.GetPoint(i) + localRight * Mathf.Abs(roadWidth);

            // Add top of road vertices
            verts[vertIndex + 0] = vertSideA;
            verts[vertIndex + 1] = vertSideB;
            // Add bottom of road vertices



            // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
            uvs[vertIndex + 0] = new Vector2(0, vPath.times[i]);
            uvs[vertIndex + 1] = new Vector2(1, vPath.times[i]);

            // Top of road normals
            normals[vertIndex + 0] = localUp;
            normals[vertIndex + 1] = localUp;


            // Set triangle indices
            if (i < vPath.NumPoints - 1 || vPath.isClosedLoop)
            {
                for (int j = 0; j < triangleMap.Length; j++)
                {
                    roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                    // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                 }


            }

            vertIndex += 8;
            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(roadTriangles, 0);

        mesh.RecalculateBounds();
    }

    // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    void AssignMeshComponents(int index)
    {

        if (meshHolder.Count <= index)
        {
            meshHolder.Add(new GameObject("Path Mesh Holder "+index.ToString()));
        }
        else if (meshHolder[index] == null)
        {
            meshHolder[index] = new GameObject("Path Mesh Holder " + index.ToString());
        }
        //meshHolder[index].transform.SetParent(transform);
        meshHolder[index].transform.rotation = Quaternion.identity;
        meshHolder[index].transform.position = Vector3.zero;
        meshHolder[index].transform.localScale = Vector3.one;
        

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder[index].gameObject.GetComponent<MeshFilter>())
        {
            meshHolder[index].gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder[index].GetComponent<MeshRenderer>())
        {
            meshHolder[index].gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder[index].GetComponent<MeshRenderer>();
        meshFilter = meshHolder[index].GetComponent<MeshFilter>();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        meshFilter.sharedMesh = mesh;
    }

    void AssignMaterials()
    {
        if (pathMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { pathMaterial};
        }
    }


}
