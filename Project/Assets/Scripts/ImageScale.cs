using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScale : MonoBehaviour
{
    public Vector2 factor = new Vector2(1, 1);
    public float multiplier = 700;

    RectTransform rectTransform;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        factor = new Vector2(image.sprite.rect.width, image.sprite.rect.height).normalized;
        rectTransform.sizeDelta = factor * multiplier * image.sprite.pixelsPerUnit / 50f;
    }
}
