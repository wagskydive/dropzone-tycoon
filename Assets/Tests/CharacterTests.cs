using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CharacterLogic;

namespace Tests
{
    public class CharacterTests
    {
        [Test]
        public void can_create_a_random_character_and_verify_exists()
        {
            string[] fakeStats = { "stat1", "stat2" };

            CharacterDataHolder characterDataHolder = new CharacterDataHolder(fakeStats);

            Character character = CharacterDataCreator.CreateRandomCharacter(characterDataHolder, 1, 2);

            Assert.NotNull(CharacterDataSupplier.AllCharacterNames(characterDataHolder));
            Assert.AreEqual(character.CharacterName, CharacterDataSupplier.AllCharacterNames(characterDataHolder)[CharacterDataSupplier.AllCharacterNames(characterDataHolder).Length-1]);


        }







    }
}
