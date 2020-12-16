using ManagementScripts;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLibraryLoader : MonoBehaviour
{   
    public void LoadLibraryFromJsonButtonPress(GameManager gameManager, string pathRoot, string fileName = "DefaultItemsLibrary")
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
    }
}
