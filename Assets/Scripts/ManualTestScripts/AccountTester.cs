using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;
using InventoryLogic;
using ManagementScripts;
using StatsLogic;

public class AccountTester : MonoBehaviour
{
    [SerializeField]
    private int accountAmount;

    [SerializeField]
    private int balanceRangeMin;

    [SerializeField]
    private int balanceRangeMax;

    [SerializeField]
    private int characterAmount;


    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        CreateTestCharacters();
        CreateTestAccounts();
    }

    private void CreateTestAccounts()
    {
        FinanceLogic.Bank bank = gameManager.bank;
        for (int i = 0; i < accountAmount; i++)
        {
            //FinanceLogic.FinancialDataCreator.CreateNewAccount(bank, UnityEngine.Random.Range(0, 1000000000).ToString(),UnityEngine.Random.Range(balanceRangeMin, balanceRangeMax));
            FinanceLogic.FinancialDataCreator.CreateNewAccount(bank,"Builder",100);
        }
    }

    private void CreateTestCharacters()
    {
        Bank bank = gameManager.bank;
        CharacterDataHolder characterDataHolder = gameManager.Characters;

        for (int i = 0; i < characterAmount; i++)
        {
            Character character = CharacterDataCreator.CreateRandomCharacter(characterDataHolder, Random.Range(0, 9999999), Random.Range(0, 9999999));
            CharacterDataCreator.CreateCharacterAccount(bank, character);

            SetCharcterStatsRandomMulipliers(character);
            SetCharcterStatsTreshholds(character);
        }

    }

    private static void SetCharcterStatsRandomMulipliers(Character character)
    {
        for (int j = 0; j < character.stats.Length; j++)
        {
            StatsHandler.SetCurrentMultiplier(character.stats[j], Random.Range(-.1f, .1f), Time.time);
        }
    }

    private static void SetCharcterStatsTreshholds(Character character)
    {
        for (int j = 0; j < character.stats.Length; j++)
        {
            StatsHandler.SetTreshhold(character.stats[j], Random.Range(0, 1f));
        }
    }


    void MultiplierForAllStats(Character character)
    {
        
    }

}
