using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;
using SkillsLogic;
using InventoryLogic;

namespace ManagementScripts
{
    public class GameManager : MonoBehaviour
    {
        public event Action<SkillTree> OnOldSkillWillBeDestroyed;
        public event Action<SkillTree> OnNewSkillTreeCreated;

        public event Action<ItemsLibrary> OnNewLibraryCreated;

        [SerializeField]
        private string[] StatTypes;

        //public List<Skill> allSkills = new List<Skill>();

        public SkillTree skillTree;

        public Bank bank;

        public CharacterDataHolder Characters;

        internal List<int> ActiveCharacters = new List<int>();
        internal ItemsLibrary Library;

        private void Awake()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            if (gameManager != null && gameManager != this)
            {
                Destroy(gameObject);
                return;
            }
            bank = new Bank();
            Characters = new CharacterDataHolder(StatTypes);

            gameObject.AddComponent<ItemLibraryLoader>().LoadLibraryFromJsonButtonPress(this, Application.dataPath + "/Resources/Items/", "DefaultItemsLibrary");

            //ItemsLibrary lib = 
            //LoadNewItemLibrary(FileSaver.JsonToItemLibrary(Application.dataPath + "/Resources/Items/", "DefaultItemsLibrary"));
        }

        private void Update()
        {
            if (ActiveCharacters.Count > 0)
            {
                CharacterTicker.TickCharacterList(Characters, ActiveCharacters, Time.time);
            }
        }

        public void NewTree(string tName)
        {
            //OnOldSkillWillBeDestroyed?.Invoke(skillTree);
            skillTree = new SkillTree(tName);
            skillTree.TreeName = tName;
            OnNewSkillTreeCreated?.Invoke(skillTree);
        }

        public void LoadSkillTree(Skill[] loadedTree, string tName)
        {
            //OnOldSkillWillBeDestroyed?.Invoke(skillTree);
            skillTree = new SkillTree(tName, false);
            skillTree.ResetTree(loadedTree);
            skillTree.TreeName = tName;
            OnNewSkillTreeCreated?.Invoke(skillTree);
        }

        public void LoadNewItemLibrary(ItemsLibrary newLibrary)
        {
            Library = newLibrary;
            OnNewLibraryCreated?.Invoke(Library);
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
