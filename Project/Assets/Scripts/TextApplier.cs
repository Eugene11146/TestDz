using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextApplier : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Slider slider;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetValue(slider.value);
    }

    public void SetValue(float value)
    {
        value = Mathf.Round(value * 100) / 100f;
        textMesh.text = ""+value;
    }
}
