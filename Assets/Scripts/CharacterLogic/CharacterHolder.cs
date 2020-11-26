using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLogic
{
    public class CharacterHolder
    {
        public static event Action<Character, int> OnCharacterAddedToList;

        internal List<Character> AllCharacters { get; private set; }

        public CharacterHolder()
        {
            AllCharacters = new List<Character>();
            CharacterDataCreator.OnCharacterCreated += AddCharacter;
        }

        

        public void AddCharacter(Character character)
        {
            AllCharacters.Add(character);
            OnCharacterAddedToList?.Invoke(character, AllCharacters.Count - 1);
        }
    }
}
