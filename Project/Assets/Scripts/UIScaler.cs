using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    CanvasScaler canvasScaler;
    int defResolusion = 170;

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    void Update()
    {
        float scale = GlobalSetting.settings.UIScale;
        canvasScaler.referenceResolution = new Vector2((defResolusion - scale) * 16, (defResolusion - scale) * 9);
    }
}
