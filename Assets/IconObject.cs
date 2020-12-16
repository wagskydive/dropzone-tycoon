using System;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;

public class IconObject : MonoBehaviour
{
    public static event Action OnObjectSet;

    public CinemachineTargetHandler cinemachineTargetHandler;
    public ShadowCatcher shadowCatcher;


    [SerializeField]
    private float rotateSpeed;

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed/10, 0));
    }

    public void RotateFromMouseDrag(Vector3 movement)
    {
        rotateSpeed = movement.x;
    }

    public void SetItem(ItemType itemType)
    {
        GameObject go = Instantiate(Resources.Load(itemType.ResourcePath)) as GameObject;
        SetNewObject(go);

    }

    public void SetNewObject(GameObject go)
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        go.transform.SetParent(transform);
        //go.transform.localRotation = Quaternion.identity;
        
        Renderer renderer = go.GetComponent<Renderer>();
        Vector3 diff = Vector3.zero;
        if (renderer != null)
        {
            Bounds bounds = cinemachineTargetHandler.CreateBoundsFromTransform(go.transform);
            cinemachineTargetHandler.SetTargetGroupTargetsAndRadius(bounds);
            if (renderer.bounds.center != Vector3.zero)
            {
                diff = -renderer.bounds.center;
            }
            shadowCatcher.SetPositionToBottomOfBounds(bounds);
        }

        go.transform.localPosition = Vector3.zero + diff;
        OnObjectSet?.Invoke();
        
   }


    public void UnLoadObject()
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
    }
}