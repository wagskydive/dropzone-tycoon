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
        TerrainMouseDetect.OnTerrainClickDetected += ShowUi;

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


    void ShowUi(Vector3 position, PointerEventData.InputButton button)
    {
        if (!isVisable)
        {
            SetVisable(true);
            transform.position = position;
            TerrainMouseDetect.OnTerrainClickDetected -= ShowUi;

        }
        else
        {
            SetVisable(false);
            TerrainMouseDetect.OnTerrainClickDetected += ShowUi;
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
            itemSpawnRequester.SpawnRequest(item, transform.position);

        }
        else
        {
            itemSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyItem(), transform.position);
        }

        SetVisable(false);
        TerrainMouseDetect.OnTerrainClickDetected += ShowUi;
    }

    public void CharacterButtonClick()
    {
        characterSpawnRequester.SpawnRequest(DummyObjects.ProvideDummyCharacter(), transform.position);

        SetVisable(false);
        TerrainMouseDetect.OnTerrainClickDetected += ShowUi;
    }

}
