using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using FinanceLogic;
using StatsLogic;
using SpawnLogic;


namespace CharacterLogic
{
    [Serializable]
    public class Character : ISpawnable
    {
        [SerializeField]
        public string CharacterName { get; private set; }
        public string FinancialAccountID { get; private set; }

        public Stat[] stats { get; private set; }

        internal Inventory inventory;




        public Character(string name, string[] statNames)
        {
            CharacterName = name;
            stats = StatsHandler.CreateStats(statNames);
            inventory = new Inventory();            
        }



        internal void SetFinancialAccountID(string id)
        {
            FinancialAccountID = id;
        }

        internal void SetStat(int index, float value)
        {
            StatsHandler.SetStatValue(stats[index],value);
        }

        internal void TickStats(float currentTime)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if(stats[i].ValueChangePerSecond != 0)
                {
                    StatsHandler.Tick(stats[i], currentTime);
                }                
            }
        }

        public string ResourcePath()
        {
            return "Characters/DefaultCharacter";
        }
    }
}
