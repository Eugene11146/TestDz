using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainerScaler : MonoBehaviour
{
    public GameObject empty;
    GameObject thisEmpty;
    float mainHeight;
    float thisHeight;
    bool wasEnabled = false;

    void OnEnable()
    {
        StartCoroutine(Delayed());
    }

    public void ResolveScale()
    {
        wasEnabled = false;
        StartCoroutine(Delayed());
    }

    IEnumerator Delayed()
    {
        yield return new WaitForSecondsRealtime(0.03f);

        if (!wasEnabled)
        {
            mainHeight = transform.parent.GetComponent<RectTransform>().rect.height;
            thisHeight = GetComponent<RectTransform>().rect.height;

            if (thisHeight < mainHeight)
            {
                if (thisEmpty == null)
                    thisEmpty = Instantiate(empty, transform);
                else
                {
                    thisHeight = GetComponent<RectTransform>().rect.height - thisEmpty.GetComponent<RectTransform>().rect.height;
                    Destroy(thisEmpty);
                    thisEmpty = Instantiate(empty, transform);
                }
                thisEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(100, mainHeight - thisHeight);
            }

            wasEnabled = true;
        }
    }
}
