using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    public event Action<Enum, int> OnOptionClick;

    public Enum options;
    public int index;

    [SerializeField]
    Text buttonText;

    public void OptionButtonClick()
    {
        OnOptionClick?.Invoke(options,index);
    }

    internal void SetupButton(Enum enumarable, int _index)
    {
        var values = Enum.GetNames(enumarable.GetType());
        buttonText.text = values[_index];
        options = enumarable;
        index = _index;
    }
}
