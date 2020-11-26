using System;
using FinanceLogic;

namespace CharacterLogic
{

    public static class CharacterGenerator
    {
        public static event Action<Character> OnCharacterCreated;

        public static Character CreateRandomCharacter(int seedFirstName, int seedLastName)
        {
            Random firstRandom = new Random(seedFirstName);
            Random lastRandom = new Random(seedLastName);

            string[] firstNames = Enum.GetNames(typeof(FirstNamesDataBase));
            string[] lastNames = Enum.GetNames(typeof(LastNamesDataBase));

            int firstIndex = firstRandom.Next(firstNames.Length);
            int lastIndex = lastRandom.Next(lastNames.Length);

            string nameString = firstNames[firstIndex] + " " + lastNames[lastIndex];

            Character character = new Character(nameString);


            OnCharacterCreated?.Invoke(character);

            return character;
        }

        public static void CreateCharacterAccount(Bank bank, Character character)
        {
            character.SetFinancialAccountID(FinancialDataCreator.CreateNewAccount(bank, character.CharacterName));
        }

    }
}
