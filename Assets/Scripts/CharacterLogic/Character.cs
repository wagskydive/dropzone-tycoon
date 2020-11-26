using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;
using FinanceLogic;



namespace CharacterLogic
{
    public class Character
    {
        public string CharacterName { get; private set; }
        public string FinancialAccountID { get; private set; }

        public Stat[] stats { get; private set; }

        internal Inventory inventory;

        public Character(string name, string[] statNames)
        {
            CharacterName = name;
            stats = CreateStats(statNames);
            inventory = new Inventory();            
        }

        Stat[] CreateStats(string[] statNames)
        {
            Stat[] stats = new Stat[statNames.Length];
            for (int i = 0; i < statNames.Length; i++)
            {
                Stat stat = new Stat(statNames[i]);
                stats[i] = stat;
            }
            return stats;
        }

        internal void SetFinancialAccountID(string id)
        {
            FinancialAccountID = id;
        }

        internal void SetStat(int index, float value)
        {
            stats[index].SetStatValue(value);
        }

        internal void TickStats(float currentTime)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if(stats[i].ValueChangePerSecond != 0 && stats[i].HasThreshhold)
                {
                    stats[i].Tick(currentTime);
                }
                
            }
        }

    }
}
