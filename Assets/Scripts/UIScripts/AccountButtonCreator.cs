using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccountButtonCreator : MonoBehaviour, IDisplayText
{
    public GameObject AccountButtonPrefab;

    List<GameObject> ButtonPool = new List<GameObject>();

    public void Display(string[] texts)
    {
        
        for (int i = 0; i < texts.Length; i++)
        {
            GameObject go;
            if (ButtonPool.Count > i)
            {
                go = ButtonPool[i];
            }
            else
            {
                go = Instantiate(AccountButtonPrefab, transform.parent);
                ButtonPool.Add(go);
                go = ButtonPool[i];
            }
            if (!ButtonPool[i].activeSelf)
            {
                ButtonPool[i].SetActive(true);
            }
            ButtonPool[i].GetComponent<AccountButton>().SetButtonID(texts[i]);
        }
        if(ButtonPool.Count > texts.Length)
        {
            for (int i = texts.Length; i < ButtonPool.Count; i++)
            {
                ButtonPool[i].SetActive(false);
            }
        }
    }

}
