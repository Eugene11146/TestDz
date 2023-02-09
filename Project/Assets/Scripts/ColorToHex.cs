using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorToHex : MonoBehaviour
{
    public ColorPreview colorPreview;
    public InputField inputField;
    public string colorHex;

    Vector3 lastColor;

    private void Update()
    {
        if (lastColor != colorPreview.color)
        {
            ChangeColor();
        } 
    }

    public void ChangeColor()
    {
        colorHex = "";
        colorHex += intToHexPair((int)colorPreview.color.x);
        colorHex += intToHexPair((int)colorPreview.color.y);
        colorHex += intToHexPair((int)colorPreview.color.z);

        colorPreview.redSlider.value = colorPreview.color.x;
        colorPreview.greenSlider.value = colorPreview.color.y;
        colorPreview.blueSlider.value = colorPreview.color.z;

        lastColor = colorPreview.color;

        inputField.text = colorHex;
    }

    string intToHexPair(int num)
    {
        string hex = "";
        int dec = 0;
        dec += num / 16;
        num -= dec * 16;

        if (dec > 9)
        {
            hex += (char)(dec + 55);
        }
        else
        {
            hex += (char)(dec + 48);
        }
        if (num > 9)
        {
            hex += (char)(num + 55);
        }
        else
        {
            hex += (char)(num + 48);
        }

        return hex;
    }
}
