using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotField : MonoBehaviour
{
    public string field;

    public void ValueChange(string val)
    {
        field = val;
    }
}
