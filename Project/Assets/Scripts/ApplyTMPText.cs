using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApplyTMPText : MonoBehaviour
{
    public TextMeshProUGUI thisTMP;
    Text thisText;

    private void Start()
    {
        thisText = GetComponent<Text>();
        Apply();
    }

    private void Update()
    {
        if (thisText.text != thisTMP.text)
        {
            Apply();
        }
    }

    public void Apply()
    {
        thisTMP.text = thisText.text;
    }
}
