using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveButton : MonoBehaviour
{
    public GameObject[] effected;

    bool currentState;

    public void ClickButton()
    {
        currentState = !currentState;
        for (int i = 0; i < effected.Length; i++)
        {
            effected[i].SetActive(currentState);
        }
    }

}
