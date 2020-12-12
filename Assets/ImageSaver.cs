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

    public GameObject iconObject;
    private void Start()
    {

        cam = GetComponent<Camera>();
        //targetHandler.OnNewTargetSet += SaveIfSaveAll;

    }

    public void SaveAll()
    {

        allItems = ItemLoader.AllItemsFromFBXFiles();
        itemLoader.CreateItemTypesFromStringArray(allItems);
        isSaving = true;
    }
    string[] allItems = new string[0];
    bool isSaving;
    int allSaveInt = 0;

    int restFrames = 5;

    int restFrameCount = 0;
    private void LateUpdate()
    {
        if (isSaving && allSaveInt < allItems.Length)
        {
            if (restFrameCount == 1)
            {
                //iconObject.SetActive(true);
                itemLoader.LoadModel(allItems[allSaveInt]);


                //itemLoader.UnLoadModel();
            }
            else if (restFrameCount == 2)
            {
                SaveItemImage(allItems[allSaveInt]);
                allSaveInt++;
            }
            else if (restFrameCount == 3)
            {
            }
            else if (restFrameCount == 4)
            {
                
               
            }
            else if (restFrameCount == 5)
            {
                restFrameCount = 0;
            }
            restFrameCount++;

        }
        if (isSaving && allSaveInt >= allItems.Length)
        {
            isSaving = false;
        }
    }

    private void SaveItemImage(string itemPath, bool overwrite = true)
    {


        string extension = Path.GetExtension(itemPath);

        string pngPath = itemPath.Substring(0, itemPath.Length - extension.Length)+"_Icon";


        if (!File.Exists(pngPath + ".png"))
        {
            SaveImage(pngPath);
        }
        else if (overwrite)
        {
            //File.Delete(pngPath);
            //SaveImage(pngPath);
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
    RenderTexture originalRenderTexture;

    public void SaveImage(string itemName, int sqr =128)
    {
        originalRenderTexture = cam.targetTexture;
        cam = GetComponent<Camera>();
        // capture the virtuCam and save it as a square PNG.

        

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

        RenderTexture.active = originalRenderTexture; //can help avoid errors 
        //cam.Render();
        cam.targetTexture = originalRenderTexture;
        cam.Render();
        Destroy(tempRT);
        Texture2D resized = virtualPhoto;
        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        string path = OurTempSquareImageLocation(itemName);

        File.WriteAllBytes(path, bytes);



        //byte[] bytes128;
        //
        //resized.Resize(128, 128);
        //bytes128 = resized.EncodeToPNG();
        //
        //string path128 = OurTempSquareImageLocation(itemName+"_128px");
        //File.WriteAllBytes(path128, bytes128);


        // virtualCam.SetActive(false); ... no great need for this.

        // now use the image, 
        //UseFileImageAt(OurTempSquareImageLocation());

    }

    private string OurTempSquareImageLocation(string path, string itemName)
    {
        string r = Application.dataPath+ path + itemName + ".png";
        return r;
    }

    private string OurTempSquareImageLocation(string itemName)
    {
        string r = Application.dataPath + "/Resources/" + itemName + ".png";
        return r;
    }


}

