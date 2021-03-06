﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ColorPallet
{
    //public static Color NormalColor { get => new Color(80,140,164,255); }
    //public static Color HighlightedColor { get => new Color(145,174,193, 255); }
    //public static Color PressedColor { get => new Color(0, 79, 45, 255); }
    //public static Color SelectedColor { get => new Color(191, 215, 234, 255); }
    //public static Color DisabledColor { get => new Color(10, 135, 84, 255); }
    //public static Color ExtraColor { get => new Color(23,95,64,255); }

    //public static ColorBlock Block { get => GetColorBlock(); } 
    

    public static ColorBlock GetColorBlock()
    {
        Color NormalColor = new Color(80, 140, 164, 255); 
        Color HighlightedColor=new Color(145, 174, 193, 255);
        Color PressedColor=new Color(0, 79, 45, 255); 
        Color SelectedColor=new Color(191, 215, 234, 255); 
        Color DisabledColor=new Color(10, 135, 84, 255); 
        Color ExtraColor =new Color(23, 95, 64, 255); 


        ColorBlock Block = new ColorBlock();
        Block.normalColor = NormalColor;
        Block.highlightedColor = HighlightedColor;
        Block.pressedColor = PressedColor;
        Block.selectedColor = SelectedColor;
        Block.disabledColor = DisabledColor;

        Block.colorMultiplier = 1;


        return Block;
    }
}
