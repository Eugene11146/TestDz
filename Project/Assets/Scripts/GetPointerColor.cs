using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPointerColor : MonoBehaviour
{
    public ColorPreview colorPreview;
    public ColorToHex colorToHex;
    RectTransform rectTransform;
    Texture2D tex;
    Vector2 hitPos;
    float pixelPerUnit;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tex = rectTransform.GetComponent<Image>().sprite.texture;
        pixelPerUnit = rectTransform.GetComponent<Image>().sprite.pixelsPerUnit;
    }

    public void GetColor()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, GlobalSetting.mainCamera, out hitPos);
        hitPos = new Vector2(Mathf.Clamp(hitPos.x * (tex.width / rectTransform.rect.width) + 200, 0, 400),
                             Mathf.Clamp(hitPos.y * (tex.height / rectTransform.rect.height) + 200, 0, 400));
        Debug.Log(hitPos);
        Color col = tex.GetPixel((int)hitPos.x, (int)hitPos.y);
        colorPreview.color = new Vector3(col.r * 255, col.g * 255, col.b * 255);
        colorToHex.ChangeColor();
    }
}
