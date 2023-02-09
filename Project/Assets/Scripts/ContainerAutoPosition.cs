using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerAutoPosition : MonoBehaviour
{
    RectTransform rectTransform;

    void Update()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -rectTransform.sizeDelta.y / 2);
    }
}
