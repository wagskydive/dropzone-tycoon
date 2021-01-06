using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IGiveInput
{
    public Vector4 GetInput()
    {
        return new Vector4(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Yaw"), 0);
    }
}
