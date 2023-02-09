using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldToSlider : MonoBehaviour
{
    public TMP_InputField inputField;
    public Slider slider;
    public Vector2 minMax;
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
        inputField.text = Mathf.Clamp(float.Parse(inputField.text), minMax.x, minMax.y).ToString();
        slider.value = float.Parse(inputField.text);
    }
}
