using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextDisplayer : MonoBehaviour, ITextDisplayer
{
    [SerializeField]
    private Text textComponent;
    private void Start()
    {
        textComponent = gameObject.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        textComponent.text = text;
    }

}
