using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider),typeof(Renderer))]
public class ItemCollider :MonoBehaviour
{
    BoxCollider boxCollider;
    Renderer renderer;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        renderer = gameObject.GetComponent<Renderer>();
        boxCollider.size = renderer.bounds.size;
        //boxCollider.center = renderer.bounds.center;
    }

}
