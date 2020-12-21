using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsLogic;


namespace CharacterLogic
{
    public static class CharacterTicker
    {
        public static void TickCharacterList(CharacterDataHolder characterHolder, List<int> characters, float currentTime)
        {
            for (int i = 0; i < characters.Count; i++)
            {

                characterHolder.AllCharacters[characters[i]].TickStats(currentTime);
            }
        }

        public static void TickAllCharacters(CharacterDataHolder characterHolder, float currentTime)
        {
            for (int i = 0; i < characterHolder.AllCharacters.Count; i++)
            {
                characterHolder.AllCharacters[i].TickStats(currentTime);
            }
        }

        public static void TickCharacter(CharacterDataHolder characterHolder, int index, float currentTime)
        {
            characterHolder.AllCharacters[index].TickStats(currentTime);
        }
    }
}
