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
        internal ItemsLibrary Library = new ItemsLibrary("fallback");
        [SerializeField]
        internal List<Structure> allSavedStructures = new List<Structure>();



        MaterialManager materialManager;
     
        public Dictionary<string, GameObject[]> StretchObjects = new Dictionary<string, GameObject[]>();

        SelectableObject currentlySelected;

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
            materialManager = gameObject.AddComponent<MaterialManager>();
            gameObject.AddComponent<ItemLibraryLoader>().LoadLibraryFromJsonButtonPress(this, Application.dataPath + "/Resources/Items/", "DefaultItemsLibrary");

            ItemSpawner.OnItemSpawnerStart += SubscribeToItemSpawner;

            SelectableObject.OnObjectSelected += HandleSelection;

            //ItemsLibrary lib = 
            //LoadNewItemLibrary(FileSaver.JsonToItemLibrary(Application.dataPath + "/Resources/Items/", "DefaultItemsLibrary"));
        }


        private void HandleSelection(SelectableObject obj)
        {
            if(currentlySelected != null)
            {
                currentlySelected.DeselectObject();
            }
            currentlySelected = obj;
        }

        void CreateStretchGameObjects(ItemType baseItemType)
        {


            GameObject go = Instantiate(Resources.Load(baseItemType.ResourcePath)) as GameObject;


            GameObject[] stretchObjects = MeshModifier.WallObjects(go);
            int itemIndex = Library.IndexFromTypeName(baseItemType.TypeName);
            for (int i = 0; i < stretchObjects.Length; i++)
            {
                //str
            }
            

            StretchObjects.Add(baseItemType.TypeName, stretchObjects);
            //stretchObjects[0] = 
        }


        public void AddStructureToSavedList(Structure structure)
        {
            allSavedStructures.Add(structure);
        }

        void SubscribeToItemSpawner(ItemSpawner itemSpawner)
        {
            itemSpawner.OnItemSpawned += HandleSpawnedObject;
        }

        void HandleSpawnedObject(ItemObject itemObject)
        {
            materialManager.HandleObjectMaterials(itemObject.gameObject);
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
            CreateStretchGameObjects(Library.allItems[Library.IndexFromTypeName("Wall Window Slide")]);
            CreateStretchGameObjects(Library.allItems[Library.IndexFromTypeName("Wall Doorway")]);
            //newLibrary.
            //CreateStretchGameObjects(Library.allItems[Library.IndexFromTypeName("Wall Doorway")]);
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
