using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeFitterClamped : MonoBehaviour
{
    public RectTransform viewPort;
    public GameObject container;
    public bool verticalSize = true;
    public bool horizontalSize = true;

    RectTransform rectTransform;
    RectTransform childsRectTransform;
    float verticalValue = 0;
    float horizontalValue = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        childsRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (verticalSize)
        {
            verticalValue = Mathf.Clamp(childsRectTransform.rect.height, 0, viewPort.rect.height);
        }
        else
        {
            verticalValue = 0;
        }
        if (horizontalSize)
        {
            horizontalValue = Mathf.Clamp(childsRectTransform.rect.width, 0, viewPort.rect.width);
        }
        else
        {
            horizontalValue = 0;
        }
        rectTransform.sizeDelta = new Vector2(horizontalValue, verticalValue);
        rectTransform.anchoredPosition = new Vector2(0, rectTransform.rect.height / -2);
    }
}
