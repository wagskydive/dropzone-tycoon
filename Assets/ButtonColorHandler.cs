using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class ButtonColorHandler : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {

       


            ColorBlock Block = buttons[i].colors;

            Color NormalColor = new Color(80, 140, 164, 255);
            Color HighlightedColor = new Color(145, 174, 193, 255);
            Color PressedColor = new Color(0, 79, 45, 255);
            Color SelectedColor = new Color(191, 215, 234, 255);
            Color DisabledColor = new Color(10, 135, 84, 255);
            Color ExtraColor = new Color(23, 95, 64, 255);



            Block.normalColor = new Color(80, 140, 164, 255);
            //Block.highlightedColor = HighlightedColor;
            //Block.pressedColor = PressedColor;
            //Block.selectedColor = SelectedColor;
            //Block.disabledColor = DisabledColor;
            //
            //Block.colorMultiplier = 1;


            buttons[i].gameObject.GetComponent<Image>().color = new Color(80, 140, 164, 255); ;




        }


    }
}
