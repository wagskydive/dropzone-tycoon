using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLogic
{
    public class CharacterDataHolder
    {
        public static event Action<Character, int> OnCharacterAddedToList;

        internal List<Character> AllCharacters { get; private set; }

        internal List<int> ActiveCharacters;

        internal string[] StatNames; 

        public CharacterDataHolder(string[] statNames = null)
        {
            StatNames = statNames;
            AllCharacters = new List<Character>();
            CharacterDataCreator.OnCharacterCreated += AddCharacter;
        }

        




        internal void AddCharacter(Character character)
        {
            AllCharacters.Add(character);
            OnCharacterAddedToList?.Invoke(character, AllCharacters.Count - 1);
        }
    }
}
