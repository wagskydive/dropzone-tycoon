using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;

namespace ManagementScripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private string[] StatTypes;


        public Bank bank;

        public CharacterDataHolder Characters;

        internal List<int> ActiveCharacters = new List<int>();

        private void Awake()
        {
            bank = new Bank();
            Characters = new CharacterDataHolder(StatTypes);
        }

        private void Update()
        {
            if (ActiveCharacters.Count > 0)
            {
                CharacterTicker.TickCharacterList(Characters, ActiveCharacters, Time.time);
            }
        }

        public void ActivateCharacter(string characterName)
        {
            int characterIndex = CharacterDataSupplier.GetIndexFromName(Characters, characterName);
            if (!ActiveCharacters.Contains(characterIndex))
            {
                ActiveCharacters.Add(characterIndex);
            }
        }

        public void DeactivateCharacter(string characterName)
        {
            int characterIndex = CharacterDataSupplier.GetIndexFromName(Characters, characterName);
            if (ActiveCharacters.Contains(characterIndex))
            {
                ActiveCharacters.Remove(characterIndex);
            }
        }


    }
}
