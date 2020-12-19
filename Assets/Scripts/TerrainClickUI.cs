using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterLogic;
using InventoryLogic;
using SpawnLogic;
using ManagementScripts;
using UnityEngine.EventSystems;

public class TerrainClickUI : MonoBehaviour
{


    bool isVisable;

    ISpawnRequester itemSpawnRequester;
    ISpawnRequester characterSpawnRequester;

    public Spawner itemSpawner;
    public Spawner characterSpawner;

    GameManager gameManager;

    void Start()
    {
        MouseDetect.OnLeftClickDetected += ShowUi;

        itemSpawnRequester = gameObject.AddComponent<SpawnRequester>();
        itemSpawner.AddSpawnRequester(itemSpawnRequester);

        characterSpawnRequester = gameObject.AddComponent<SpawnRequester>();
        characterSpawner.AddSpawnRequester(characterSpawnRequester);
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
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
            MouseDetect.OnLeftClickDetected -= ShowUi;

        }
        else
        {
            SetVisable(false);
            MouseDetect.OnLeftClickDetected += ShowUi;
        }

    }




    public void ItemButtonClick()
    {
        if(gameManager != null && gameManager.Library != null)
        {
            int allItemsCount = gameManager.Library.allItems.Count;
            int random = UnityEngine.Random.Range(0, allItemsCount);
            ItemType type = gameManager.Library.allItems[random];
            Item item = new Item(type);
            itemSpawnRequester.SpawnRequest(item, transform);

        }
        else
        {
            itemSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyItem(), transform);
        }

        SetVisable(false);
        MouseDetect.OnLeftClickDetected += ShowUi;
    }

    public void CharacterButtonClick()
    {
        characterSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyCharacter(), transform);

        SetVisable(false);
        MouseDetect.OnLeftClickDetected += ShowUi;
    }

}
