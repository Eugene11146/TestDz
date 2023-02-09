using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldFoolTest : MonoBehaviour
{
    public string defaultText = "Untitled";

    TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void Set()
    {
        string text = "";
        for (int i = 0; i < inputField.text.Length; ++i)
        {
            if ((inputField.text[i] >= 'A' && inputField.text[i] <= 'Z') || (inputField.text[i] >= 'a' && inputField.text[i] <= 'z') || (inputField.text[i] >= '0' && inputField.text[i] <= '9') || inputField.text[i] == ' ')
            {
                text += inputField.text[i];
            }
        }
        if (text == "")
        {
            text = defaultText;
        }

        inputField.text = text;
    }
}
