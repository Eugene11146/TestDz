using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexToColor : MonoBehaviour
{
    public ColorPreview colorPreview;
    public ColorToHex colorToHex;
    public InputField inputField;
    public Vector3 color;

    public void Apply()
    {
        string clampedStr = "";
        inputField.text.ToUpper();
        foreach (char ch in inputField.text)
        {
            if ((ch > 'F' || ch < 'A') && (ch > '9' || ch < '0'))
            {
                if (ch < '0')
                    clampedStr += '0';
                else if (ch > 'F')
                    clampedStr += 'F';
            }
            else
            {
                clampedStr += ch;
            }
        }
        while (clampedStr.Length < 6)
            clampedStr += '0';
        inputField.text = clampedStr;
        color = new Vector3(hexPairToInt("" + inputField.text[0] + inputField.text[1]),
                                    hexPairToInt("" + inputField.text[2] + inputField.text[3]),
                                        hexPairToInt("" + inputField.text[4] + inputField.text[5]));
        colorPreview.color = color;
        colorToHex.ChangeColor();
    }


    int hexPairToInt(string hex)
    {
        int num = 0;
        if (hex[0] >= 'A')
        {
            num += (hex[0] - 55) * 16;
        }
        else
        {
            num += (hex[0] - 48) * 16;
        }
        if (hex[1] >= 'A')
        {
            num += (hex[1] - 55);
        }
        else
        {
            num += (hex[1] - 48);
        }
        return num;
    }
}
