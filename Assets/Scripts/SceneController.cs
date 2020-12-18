using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();


    public void LoadSystemsScenes()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("SystemsScene", LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("3dIconScene", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());

    }

    public void HandleObjectFromOtherScene(UnityEngine.Object obj)
    {
        GameObject go = Instantiate((GameObject)obj);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;

        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                yield return null;
            }
        }
        
    }

    public void CreateSceneBridge()
    {

    }

    public void LoadStateMachineScenes()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("StateMachines", LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("3dIconScene", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 30, 150, 30), "State Machine Scenes"))
        {
            LoadStateMachineScenes();

        }

        if (GUI.Button(new Rect(20, 60, 150, 30), "SystemScenes"))
        {
            LoadSystemsScenes();
        }
    }


}