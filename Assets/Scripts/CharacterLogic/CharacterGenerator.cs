using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLogic
{

    public static class CharacterGenerator
    {
        public static Character CreateRandomCharacter()
        {
            Random firstRandom = new Random();
            Random lastRandom = new Random();

            string[] firstNames = Enum.GetNames(typeof(FirstNamesDataBase));
            string[] lastNames = Enum.GetNames(typeof(LastNamesDataBase));

            int firstIndex = firstRandom.Next(firstNames.Length);
            int lastIndex = lastRandom.Next(lastNames.Length);

            string nameString = firstNames[firstIndex] + " " + lastNames[lastIndex];

            Character character = new Character(nameString);

            return character;
        }

    }
}
