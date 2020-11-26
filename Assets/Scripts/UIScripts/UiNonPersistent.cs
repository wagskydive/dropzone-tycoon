using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UiNonPersistent : MonoBehaviour
{
    private void Start()
    {
        SubscribeToRefresh();
    }

    public void CallRefresh()
    {
        UiManager.RefreshAll(gameObject);
    }

    public void SubscribeToRefresh()
    {
        UiManager.OnRefresh += HandleRefresh;
    }


    public void HandleRefresh(GameObject go)
    {
        if(go != gameObject || go.transform.IsChildOf(transform))
        {
            go.SetActive(false);
        }
        else if(go == gameObject)
        {
            go.SetActive(true);
        }
        
    }


}

