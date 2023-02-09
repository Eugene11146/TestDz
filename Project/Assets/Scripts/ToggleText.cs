using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleText : MonoBehaviour
{
    public string textOn;
    public string textOff;
    public bool toggle;
    public TextMeshProUGUI tex;

    private void Start()
    {
        tex.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (toggle)
            tex.text = textOn;
        else
            tex.text = textOff;
    }
}
