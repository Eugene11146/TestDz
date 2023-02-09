using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChildrenWidth : MonoBehaviour
{
    void Start()
    {
        var children = GetComponentsInChildren<RectTransform>();
        RectTransform thisRect = GetComponent<RectTransform>();

        foreach (RectTransform child in children)
        {
            child.sizeDelta = new Vector2(thisRect.rect.width, child.sizeDelta.y);
        }
    }
}
