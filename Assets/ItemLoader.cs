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
        pathRoot = Application.persistentDataPath + "/";
        gameManager = FindObjectOfType<GameManager>();
        Loader.OnEdited += LoadTree;
        ModelLoader.OnEdited += LoadModel;
    }

    void LoadNodes(ItemType[] items, string nameP)
    {
        gameManager.LoadItemLibrary(items, nameP);
        gameObject.SetActive(false);
        ItemCreator.SetActive(true);
    }

    public void CreateItemTypesFromStringArray(string[] inputStrings)
    {
        gameManager.Library.AddItemsFromStringArray(inputStrings);
    }

    public void SaveLibraryButtonClick()
    {
        string path = Application.dataPath + "/Resources/Items/" + gameManager.skillTree.TreeName + ".itemlibrary";
        Debug.Log(path);
        FileSaver.LibraryToJson(path, gameManager.Library.allItems);
    }

    public void RenameFilesInFolderClick()
    {
        FileSaver.RenameFilesWithExtentionInFolder(pathRoot+FolderPathField.text, ExtentionField.text);
    }


    //public Sprite IconSpriteFromName(string itemTypeName)
    //{
    //
    //}

    public void LoadModel(string resourcePath)
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
        string[] names = AllItemsFromFBXFiles();
        AllItemsDropdown.PopulateDropDown(names);
    }

    public static string[] AllItemsFromFBXFiles()
    {
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
                    names.Add("Items/" + directoryInfos[j].Name + "/" + fileInfos[i].Name);
                    fullNames.Add(fileInfos[i].FullName);
                }

            }
        }

        return names.ToArray();
    }

    public void LoadLibraryFromJsonButtonPress()
    {

        var info = new DirectoryInfo(pathRoot);
        FileInfo[] fileInfos = info.GetFiles();
        List<string> files = new List<string>();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string extension = Path.GetExtension(fileInfos[i].Name);
            if (extension == ".itemlibrary")
            {
                string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extension.Length);
                files.Add(result);
            }


        }

        FilesDropdown.PopulateDropDown(files.ToArray());

        Loader.EnableEditMode();
    }




    public void ConfirmPathForFBXLoading()
    {
        gameManager.Library.AddItemsFromStringArray(CreateStringsFromFBXinFolder(FBXfolderPathInput.text));
    }

    public string[] CreateStringsFromFBXinFolder(string path)
    {
        var info = new DirectoryInfo(pathRoot+path);
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

    void LoadTree(string namePath)
    {


        string path = pathRoot + namePath + ".itemlibrary";
        LoadNodes(FileSaver.JsonToItemLibrary(path), namePath);
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
