using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchUI : MonoBehaviour
{
    public GameObject columnsPrefab;

    GameObject curCol;
    GameObject stam;

    GameObject[] splits;



    public void SetupBranch(GameObject st, GameObject[] splts = null)
    {
        stam = st;
        splits = splts;
        stam.transform.SetParent(transform);
        curCol = Instantiate(columnsPrefab, transform);
        curCol.SetActive(true);
        if (!isRoot())
        {
            for (int i = 0; i < splits.Length; i++)
            {
                splits[i].transform.SetParent(curCol.transform);
            }
        }
        if (splits != null)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta *= new Vector2(1, splits.Length);


            curCol.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
        }


    }

    public bool isRoot()
    {
        if (splits == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
