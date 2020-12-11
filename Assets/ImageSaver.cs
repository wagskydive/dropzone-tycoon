using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(Camera))]
public class ImageSaver : MonoBehaviour
{
    Camera cam;
    public CinemachineTargetHandler targetHandler;
    public ItemLoader itemLoader;
    private void Start()
    {

        cam = GetComponent<Camera>();
        //targetHandler.OnNewTargetSet += SaveIfSaveAll;

    }

    public void SaveAll()
    {
        allItems = ItemLoader.AllItemsFromFBXFiles();
        isSaving = true;
    }
    string[] allItems = new string[0];
    bool isSaving;
    int allSaveInt = 0;

    int restFrames = 4;

    int restFrameCount = 0;
    private void Update()
    {
        if (isSaving && allSaveInt < allItems.Length)
        {
            if(restFrameCount > restFrames)
            {
                itemLoader.LoadModel(allItems[allSaveInt]);
                SaveItemImage(allItems[allSaveInt]);
                allSaveInt++;
                restFrameCount = 0;
                //itemLoader.UnLoadModel();
            }
            restFrameCount++;

        }
        if(isSaving && allSaveInt >= allItems.Length)
        {
            isSaving = false;
        }
    }

    private void SaveItemImage(string itemPath, bool overwrite = true)
    {
        

        string extension = Path.GetExtension(itemPath);

        string pngPath = itemPath.Substring(0, itemPath.Length - extension.Length) + ".png";



        if (!File.Exists(pngPath) || overwrite)
        {
            SaveImage(pngPath);
        }
    }



    public Texture2D VirtualPhoto(int sqr = 512, float aspect = 1.0f)
    {

        cam = GetComponent<Camera>();
        cam.aspect = aspect;
        // recall that the height is now the "actual" size from now on
        RenderTexture prevTexture = cam.targetTexture;

        RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
        // the 24 can be 0,16,24, formats like
        // RenderTextureFormat.Default, ARGB32 etc.

        cam.targetTexture = tempRT;
        cam.Render();

        RenderTexture.active = tempRT;
        Texture2D virtualPhoto =
            new Texture2D(sqr, sqr, TextureFormat.RGBA32, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        cam.targetTexture = prevTexture;
        Destroy(tempRT);
        return virtualPhoto;

    }

    public void SaveImage(string itemName)
    {

        cam = GetComponent<Camera>();
        // capture the virtuCam and save it as a square PNG.

        int sqr = 512;

        cam.aspect = 1.0f;
        // recall that the height is now the "actual" size from now on
        //RenderTexture prevTexture = cam.targetTexture;

        RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
        // the 24 can be 0,16,24, formats like
        // RenderTextureFormat.Default, ARGB32 etc.

        cam.targetTexture = tempRT;
        cam.Render();

        RenderTexture.active = tempRT;
        Texture2D virtualPhoto =
            new Texture2D(sqr, sqr, TextureFormat.RGBA32, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        cam.targetTexture = null;
        Destroy(tempRT);

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(
            OurTempSquareImageLocation(itemName), bytes);
        // virtualCam.SetActive(false); ... no great need for this.

        // now use the image, 
        //UseFileImageAt(OurTempSquareImageLocation());

    }

    private string OurTempSquareImageLocation(string path, string itemName)
    {
        string r = Application.persistentDataPath+path + itemName+".png";
        return r;
    }

    private string OurTempSquareImageLocation(string itemName)
    {
        string r = Application.dataPath + "/Resources/" + itemName;
        return r;
    }

    
}

