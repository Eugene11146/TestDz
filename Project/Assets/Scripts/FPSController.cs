using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    public Text fpsText;
    public Text fpsText2;
    public int limit = 1000;
    public float deltaTime;
    public bool VSync = false;

    void Update()
    {
        if (VSync)
            Application.targetFrameRate = limit;
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime * Time.timeScale;
        fpsText.text = Mathf.Ceil(fps).ToString();
        fpsText2.text = Mathf.Ceil(fps).ToString();
    }
}
