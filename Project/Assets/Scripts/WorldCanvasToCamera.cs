using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasToCamera : MonoBehaviour
{
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.sizeDelta = new Vector2(GlobalSetting.mainCamera.pixelRect.size.x / GlobalSetting.mainCamera.pixelRect.size.y * 415, 415);
    }
}
