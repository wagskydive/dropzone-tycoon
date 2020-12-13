using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterLogic;
using InventoryLogic;
using SpawnLogic;

public class TerrainClickUI : MonoBehaviour
{


    bool isVisable;

    ISpawnRequester itemSpawnRequester;
    ISpawnRequester characterSpawnRequester;

    public Spawner itemSpawner;
    public Spawner characterSpawner;

    void Start()
    {
        TerrainClickDetect.OnTerrainClickDetected += ShowUi;

        itemSpawnRequester = gameObject.AddComponent<SpawnRequester>();
        itemSpawner.AddSpawnRequester(itemSpawnRequester);

        characterSpawnRequester = gameObject.AddComponent<SpawnRequester>();
        characterSpawner.AddSpawnRequester(characterSpawnRequester);

    }

    void SetVisable(bool vis)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(vis);
        }
        isVisable = vis;
    }


    void ShowUi(Vector3 position)
    {
        if (!isVisable)
        {
            SetVisable(true);
            transform.position = position;
            TerrainClickDetect.OnTerrainClickDetected -= ShowUi;

        }
        else
        {
            SetVisable(false);
            TerrainClickDetect.OnTerrainClickDetected += ShowUi;
        }

    }


    public void ItemButtonClick()
    {
        itemSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyItem(), transform.position);
        SetVisable(false);
    }

    public void CharacterButtonClick()
    {
        characterSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyCharacter(), transform.position);

        SetVisable(false);
    }

}
