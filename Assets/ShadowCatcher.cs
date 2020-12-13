using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCatcher : MonoBehaviour
{
    public void SetPositionToBottomOfBounds(Bounds bounds)
    {
        transform.localPosition = Vector3.down * bounds.extents.y;
    }
}
