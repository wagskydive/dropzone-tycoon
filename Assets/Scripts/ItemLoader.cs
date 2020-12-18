using InventoryLogic;
using ManagementScripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoader : MonoBehaviour
{
    public InputField NameInput;

    public InputField FBXfolderPathInput;



    public InputField FolderPathField;
    public InputField ExtentionField;

    public GameObject ItemCreator;

    public DropdownHandler FilesDropdown;

    GameManager gameManager;

    public Editable Loader;

    public Editable ModelLoader;
    public DropdownHandler AllItemsDropdown;

    public string fileName;

    string pathRoot;

    public GameObject iconObject;

    public ImageSaver imageSaver;

    private void Start()
    {
        pathRoot = Application.dataPath + "/Resources/Items/";
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        Loader.OnEdited += LoadTreeFromJson;
        ModelLoader.OnEdited += LoadIconModel;
    }

    void LoadNodesFromLibrary(ItemsLibrary library)
    {
        gameManager.LoadNewItemLibrary(library);
        gameObject.SetActive(false);
        ItemCreator.SetActive(true);
    }

    public void CreateItemTypesFromStringArray(string[] inputStrings)
    {
        gameManager.Library.AddItemsFromStringArray(inputStrings);
    }

    public void SaveLibraryButtonClick()
    {
        string path = pathRoot + gameManager.Library.LibraryName + ".json";
        Debug.Log(path);
        FileSaver.WriteLibraryToJson(path, gameManager.Library);
    }

    public void RenameFilesInFolderClick()
    {
        FileSaver.RenameFilesWithExtentionInFolder(pathRoot + FolderPathField.text, ExtentionField.text);
    }


    //public Sprite IconSpriteFromName(string itemTypeName)
    //{
    //
    //}

    public void LoadIconModel(string resourcePath)
    {
        string extension = Path.GetExtension(resourcePath);
        string itemName = Path.GetFileName(resourcePath);

        itemName = itemName.Substring(0, itemName.Length - extension.Length);
        resourcePath = resourcePath.Substring(0, resourcePath.Length - extension.Length);

        GameObject go = Instantiate(Resources.Load(resourcePath)) as GameObject;

        Debug.Log(resourcePath);
        //loadedObject = new GameObject();

        iconObject.GetComponent<IconObject>().SetNewObject(go);
    }

    public void UnLoadModel()
    {
        iconObject.GetComponent<IconObject>().UnLoadObject();
    }

    public void LoadModelButtonClick()
    {

        //string[] names = AllNewItemsFromFBXFiles();
        //AllItemsDropdown.PopulateDropDown(names);
    }

    public static ItemType[] AllNewItemsFromFBXFiles(ItemsLibrary library = null)
    {
        List<ItemType> itemTypes = new List<ItemType>();
        DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath + "/Resources/Items/");

        DirectoryInfo[] directoryInfos = levelDirectoryPath.GetDirectories();
        List<string> directories = new List<string>();


        //FileInfo[] fileInfos = levelDirectoryPath.GetFiles();
        List<string> names = new List<string>();
        List<string> fullNames = new List<string>();

        for (int j = 0; j < directoryInfos.Length; j++)
        {

            FileInfo[] fileInfos = directoryInfos[j].GetFiles();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                string extension = Path.GetExtension(fileInfos[i].Name);
                if (extension == ".fbx")
                {

                    if (library == null || !library.HasItemWithName(fileInfos[i].Name))
                    {

                        ItemType itemType = new ItemType(fileInfos[i].Name, directoryInfos[j].Name + "/", fileInfos[i].Directory.Name);

                        itemTypes.Add(itemType);
                    }



                    //names.Add(directoryInfos[j].Name + "/" + fileInfos[i].Name);
                    //fullNames.Add(fileInfos[i].FullName);
                }

            }
        }

        return itemTypes.ToArray();
    }

    public void LoadLibraryFromJsonButtonPress(string fileName = "DefaultItemsLibrary")
    {

        var info = new DirectoryInfo(pathRoot);
        FileInfo[] fileInfos = info.GetFiles();
        List<string> files = new List<string>();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string extension = Path.GetExtension(fileInfos[i].Name);
            if (extension == ".json")
            {
                string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extension.Length);
                files.Add(result);
            }


        }

        for (int i = 0; i < files.Count; i++)
        {
            if (files[i] == fileName)
            {
                gameManager.LoadNewItemLibrary(FileSaver.JsonToItemLibrary(pathRoot + fileName + ".json", fileName));
                return;
            }
        }

        //FilesDropdown.PopulateDropDown(files.ToArray());
        //
        //Loader.EnableEditMode();
    }




    public void ConfirmPathForFBXLoading()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        gameManager.Library.AddItemsFromItemTypeArray(AllNewItemsFromFBXFiles());
        gameManager.Library.ChangeLibraryName("DefaultItemsLibrary");

    }

    public string[] CreateStringsFromFBXinFolder(string path)
    {
        var info = new DirectoryInfo(pathRoot + path);
        FileInfo[] fileInfos = info.GetFiles();
        if (fileInfos != null)
        {


            List<string> files = new List<string>();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                string extension = Path.GetExtension(fileInfos[i].Name);
                if (extension == ".fbx")
                {
                    string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extension.Length);
                    files.Add(result);
                }


            }
            //Debug.Log(files.ToString());
            return files.ToArray();
        }
        Debug.Log("invalid folder or no files in folder");
        return null;
    }

    void LoadTreeFromJson(string namePath)
    {



        LoadNodesFromLibrary(FileSaver.JsonToItemLibrary(pathRoot, namePath));
    }



    public void CreatNewTreeButtonPress()
    {
        if (NameInput.text == null || NameInput.text == "")
        {
            return;
        }
        else
        {
            gameManager.NewTree(NameInput.text);
            gameObject.SetActive(false);
            ItemCreator.SetActive(true);
        }
    }
}
