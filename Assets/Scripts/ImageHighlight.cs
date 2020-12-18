using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageHighlight : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private void Awake()
    {
        if(image == null)
        {
            image = gameObject.GetComponent<Image>();
            
        }
        image.enabled = false;
        transform.parent.GetComponent<IHoverUi>().OnHover += SetImageEnabled;
    }

    void SetImageEnabled(bool enabled)
    {
        image.enabled = enabled;
    }
}
