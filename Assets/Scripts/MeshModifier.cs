using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Linq;

public class MeshModifier : MonoBehaviour
{

    // Initialize two new meshes in the scene
    public GameObject Original;


    private void Awake()
    {
        CreateWall(5);
    }

    void CreateWall(int wallLengt)
    {
        GameObject wallParent = new GameObject();


        GameObject Modifier = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Modifier.transform.position = new Vector3(.5f, 2, 0);

        Modifier.transform.localScale = new Vector3(.5f, 5, 1);
        GameObject middle = Instantiate(SubtractModifierFromOriginal(Original, Modifier));


        Modifier.transform.position = new Vector3(0, 2, 0);
        GameObject start = Instantiate(SubtractModifierFromOriginal(Original, Modifier));

        Modifier.transform.position = new Vector3(1, 2, 0);
        GameObject end = Instantiate(SubtractModifierFromOriginal(Original, Modifier));
        for (int i = 0; i < wallLengt; i++)
        {
            start.transform.SetParent(wallParent.transform);



            end.transform.SetParent(wallParent.transform);
            middle.transform.SetParent(wallParent.transform);
            end.transform.position = new Vector3(wallLengt-1, 0, 0);
            middle.transform.position = new Vector3(wallLengt - 1, 0, 0);


            GameObject startB = Instantiate(middle, wallParent.transform);
            startB.transform.position = new Vector3(.5f, 0, 0);

            startB.transform.SetParent(wallParent.transform);
            if (i > 0 && i < wallLengt - 1)
            {
                GameObject a= Instantiate(middle, wallParent.transform);
                a.transform.position = new Vector3(i, 0, 0);

                GameObject b = Instantiate(middle, wallParent.transform);
                b.transform.position = new Vector3(i+.5f, 0, 0);
            }

        }
        Destroy(Modifier);
    }


    public GameObject SubtractModifierFromOriginal(GameObject original, GameObject modifier)
    {
        CSG_Model result = Boolean.Intersect(original, modifier);

        // Create a gameObject to render the result
        GameObject composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        return composite;
    }
}
