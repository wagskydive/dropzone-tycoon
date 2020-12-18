using CharacterLogic;
using ManagementScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBarCreator : MonoBehaviour
{


    public GameObject StatsBarPrefab;

    List<GameObject> ObjectPool = new List<GameObject>();

    private void Awake()
    {
        CharacterButton.OnCharacterButtonClick += SetStatsBarsFromString;
    }

    void SetStatsBarsFromString(string name)
    {
        CharacterDataHolder characterHolder = FindObjectOfType<GameManager>().Characters;
        CreateCharacterStats(CharacterDataSupplier.GetIndexFromName(characterHolder, name));
    }

    public void CreateCharacterStats(int index)
    {
        CharacterDataHolder characterHolder = FindObjectOfType<GameManager>().Characters;
        Character character = CharacterDataSupplier.GetCharacterFromIndex(characterHolder, index);

        for (int i = 0; i < character.stats.Length; i++)
        {
                      
            GameObject go;
            if (ObjectPool.Count > i)
            {
                go = ObjectPool[i];
            }
            else
            {
                go = Instantiate(StatsBarPrefab, transform.parent);

                ObjectPool.Add(go);
                go = ObjectPool[i];
            }
            if (!ObjectPool[i].activeSelf)
            {
                ObjectPool[i].SetActive(true);
            }

            ObjectPool[i].GetComponent<StatBar>().HookUp(character.stats[i]);

        }
        if (ObjectPool.Count > character.stats.Length)
        {
            for (int i = character.stats.Length; i < ObjectPool.Count; i++)
            {
                ObjectPool[i].SetActive(false);
            }
        }
    }


}
