using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseParentScale : MonoBehaviour
{
    RectTransform parent;
    RectTransform thisRect;


    void Start()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        thisRect = GetComponent<RectTransform>();
        StartCoroutine(delayedMove());
    }

    IEnumerator delayedMove()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        transform.position += new Vector3(0, thisRect.rect.height * 2, 0);
        thisRect.sizeDelta = new Vector2(thisRect.sizeDelta.x, parent.rect.height);
    }
}
