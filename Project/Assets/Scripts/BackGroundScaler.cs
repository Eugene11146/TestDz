using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScaler : MonoBehaviour
{
    public float backGroundTiling = 1f;
    RectTransform rectTransform;
    Material backGroundMaterial;

    private void Start()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
        backGroundMaterial = GetComponent<Image>().material;
        backGroundMaterial.mainTextureScale = rectTransform.sizeDelta.normalized * backGroundTiling * rectTransform.sizeDelta.magnitude;
    }
}
