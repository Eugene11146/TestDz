using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDebugText : MonoBehaviour
{
    UISmoothSlide uiSmoothSlide;
    TextMeshProUGUI textMesh;
    Color defaultColor;

    private void Start()
    {
        uiSmoothSlide = GetComponent<UISmoothSlide>();
        textMesh = GetComponent<TextMeshProUGUI>();
        defaultColor = textMesh.color;
    }

    public void Log(string text)
    {
        textMesh.color = defaultColor;
        textMesh.text = text;
        GetComponent<UISmoothSlide>().Open();
        StartCoroutine(Close(4));
    }

    public void Log(string text, Color color)
    {
        textMesh.color = color;
        textMesh.text = text;
        GetComponent<UISmoothSlide>().Open();
        StartCoroutine(Close(4));
    }

    IEnumerator Close(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        GetComponent<UISmoothSlide>().Close();
    }
}
