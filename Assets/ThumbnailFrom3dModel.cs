using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ThumbnailFrom3dModel : MonoBehaviour
{
    Image image;

    public ImageSaver imageSaver;

    Texture2D currentImage;

    private void Start()
    {
        image = GetComponent<Image>();
        //SetNewImage();
    }
    

    public void SetNewImage()
    {
        currentImage = imageSaver.VirtualPhoto();
        float sqr = 512;
        //Sprite sprite = Sprite.Create(currentImage, new Rect(0, 0, sqr, sqr), Vector2.zero,);
        image.material.mainTexture = currentImage;
    }
    
}
