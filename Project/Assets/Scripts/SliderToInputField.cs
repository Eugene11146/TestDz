using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public Slider slider;
    public bool applyOnStart = true;

    private void Start()
    {
        if (applyOnStart)
        {
            Apply();
        }
    }

    public void Apply()
    {
        string text = "";
        string sliderText = slider.value.ToString();
        for (int i = 0; i < sliderText.Length; ++i)
        {
            text += sliderText[i];
            if (i == 3)
            {
                break;
            }
        }
        inputField.text = text;
    }

    public void Apply(float value)
    {
        string text = "";
        string sliderText = value.ToString();
        for (int i = 0; i < sliderText.Length; ++i)
        {
            text += sliderText[i];
            if (i == 3)
            {
                break;
            }
        }
        inputField.text = text;
    }
}
