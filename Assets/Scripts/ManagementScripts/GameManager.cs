using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;
using SkillsLogic;

namespace ManagementScripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private string[] StatTypes;

        public List<Skill> allSkills = new List<Skill>();

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

        public bool ActivateCharacterReturnWasActive(string characterName)
        {
            int characterIndex = CharacterDataSupplier.GetIndexFromName(Characters, characterName);
            if (!ActiveCharacters.Contains(characterIndex))
            {
                ActiveCharacters.Add(characterIndex);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool DeactivateCharacterReturnWasAcrive(string characterName)
        {
            int characterIndex = CharacterDataSupplier.GetIndexFromName(Characters, characterName);
            if (ActiveCharacters.Contains(characterIndex))
            {
                ActiveCharacters.Remove(characterIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIfCharacterIsActive(string characterName)
        {

            int characterIndex = CharacterDataSupplier.GetIndexFromName(Characters, characterName);
            return ActiveCharacters.Contains(characterIndex);


        }


        }
}
