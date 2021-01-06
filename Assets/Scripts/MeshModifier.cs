using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Linq;
using System;

public class MeshModifier : MonoBehaviour
{
    //public GameObject Original;


    //private void Awake()
    //{
    //    CreateWall(5, Original);
    //}

    public static GameObject CreateWall(int wallLength, GameObject original)
    {

        GameObject[] wallObjectPre = WallObjects(original);
        GameObject wallParent = new GameObject();
        wallParent.name = "wall";

        if (wallLength == 1)
        {
            GameObject go = Instantiate(original, wallParent.transform);
        }
        if(wallLength == 2)
        {
            GameObject go1 = Instantiate(wallObjectPre[2], wallParent.transform);
            
            GameObject go2 = Instantiate(wallObjectPre[3], wallParent.transform);
            go1.transform.Translate(new Vector3(1, 0, 0));
        }
        if(wallLength > 2)
        {
            GameObject go1 = Instantiate(wallObjectPre[2], wallParent.transform);
            wallLength--;
            for (int i = 1; i < wallLength-1; i++)
            {
                GameObject go2 = Instantiate(wallObjectPre[1], wallParent.transform);
                go2.transform.Translate(new Vector3(i, 0, 0));
            }
            GameObject go3 = Instantiate(wallObjectPre[3], wallParent.transform);
            go3.transform.Translate(new Vector3(wallLength-1, 0, 0));
        }



        return wallParent;

    }
    public static GameObject[] WallObjects(GameObject original)
    {
        GameObject[] objects = new GameObject[4];
        objects[0] = original;
    
        GameObject Modifier = GameObject.CreatePrimitive(PrimitiveType.Cube);


        Modifier.transform.position = new Vector3(.5f, 2, 0);
        Modifier.transform.localScale = new Vector3(.2f, 5, 5);

        GameObject middleSmallSection = IntersectModifierWithOriginal(original, Modifier);
        middleSmallSection.name = " middleSmallSection ";

        GameObject middle = IntersectModifierWithOriginal(original, Modifier);

        MiddleSectionStretchPart(middle);
        middle.name = "Middle";

        objects[1] = middle;


        Modifier.transform.position = new Vector3(-.4f, 2, 0);
        Modifier.transform.localScale = new Vector3(2, 5, 5);
        GameObject start = IntersectModifierWithOriginal(original, Modifier);
        start.name = "Start";
        GameObject startStretchOne = Instantiate(middleSmallSection, start.transform);
        startStretchOne.transform.position = new Vector3(.2f, 0, 0);
        startStretchOne.name = "startStretchOne";

        GameObject startStretchTwo = Instantiate(middleSmallSection, start.transform);
        startStretchTwo.transform.position = new Vector3(.4f, 0, 0);
        startStretchTwo.name = "startStretchTwo";


        objects[2] = start;


        Modifier.transform.position = new Vector3(1.4f, 2, 0);
        //Modifier.transform.localScale = new Vector3(.3f, 5, 5);
        GameObject end = IntersectModifierWithOriginal(original, Modifier);
        end.name = "End";
        GameObject endStretchOne = Instantiate(middleSmallSection, end.transform);
        endStretchOne.transform.position = new Vector3(-.2f, 0, 0);
        endStretchOne.name = "endStretchOne";

        GameObject endStretchTwo = Instantiate(middleSmallSection, end.transform);
        endStretchTwo.transform.position = new Vector3(-.4f, 0, 0);
        endStretchTwo.name = "endStretchTwo";

        objects[3] = end;



        Destroy(Modifier);
        return objects;
    }

    private static void MiddleSectionStretchPart(GameObject ParentObject)
    {

        GameObject go1 = Instantiate(ParentObject);

        go1.transform.Translate(-.4f, 0, 0);
        GameObject go2 = Instantiate(ParentObject);
        go2.transform.Translate(-.2f, 0, 0);
        GameObject go3 = Instantiate(ParentObject);
        go3.transform.Translate(.2f, 0, 0);
        GameObject go4 = Instantiate(ParentObject);
        go4.transform.Translate(.4f, 0, 0);

        go1.transform.SetParent(ParentObject.transform);
        go2.transform.SetParent(ParentObject.transform);
        go3.transform.SetParent(ParentObject.transform);
        go4.transform.SetParent(ParentObject.transform);
    }

    public static GameObject IntersectModifierWithOriginal(GameObject original, GameObject modifier)
    {
        CSG_Model result = Parabox.CSG.Boolean.Intersect(original, modifier);

        // Create a gameObject to render the result
        GameObject composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        return composite;
    }

    public GameObject AddModifierToOriginal(GameObject original, GameObject modifier)
    {
        CSG_Model result = Parabox.CSG.Boolean.Union(original, modifier);

        // Create a gameObject to render the result
        GameObject composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        return composite;
    }
}
