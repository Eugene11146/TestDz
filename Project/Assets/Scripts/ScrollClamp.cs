using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollClamp : MonoBehaviour
{
    public bool vertical = true;
    public bool inverse;
    public float buttonsSize = 130;
    public float maxButtonsCount = 3;
    public ScrollRect scrollRect;
    RectTransform rectTransform;
    float decelerationRate = 0;

    private void Start()
    {
        decelerationRate = scrollRect.decelerationRate;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (vertical)
        {

            if (!inverse)
            {
                if (rectTransform.anchoredPosition.y > (transform.childCount - maxButtonsCount) * buttonsSize ||
                rectTransform.anchoredPosition.y < 0)
                {
                    scrollRect.decelerationRate = 0;
                }
                else
                {
                    scrollRect.decelerationRate = decelerationRate;
                }
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                    Mathf.Clamp(rectTransform.anchoredPosition.y, 0, (transform.childCount - maxButtonsCount) * buttonsSize));
            }
            else
            {
                if (rectTransform.anchoredPosition.y < (transform.childCount - maxButtonsCount) * -buttonsSize ||
                rectTransform.anchoredPosition.y > 0)
                {
                    scrollRect.decelerationRate = 0;
                }
                else
                {
                    scrollRect.decelerationRate = decelerationRate;
                }
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                    Mathf.Clamp(rectTransform.anchoredPosition.y * -1, 0, (transform.childCount - maxButtonsCount) * buttonsSize) * -1);
            }
        }
        else
        {
            if (!inverse)
            {
                if (rectTransform.anchoredPosition.x > (transform.childCount - maxButtonsCount) * buttonsSize ||
                rectTransform.anchoredPosition.x < 0)
                {
                    scrollRect.decelerationRate = 0;
                }
                else
                {
                    scrollRect.decelerationRate = decelerationRate;
                }
                rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, 0, (transform.childCount - maxButtonsCount) * buttonsSize),
                    rectTransform.anchoredPosition.y);
            }
            else
            {
                if (rectTransform.anchoredPosition.x < (transform.childCount - maxButtonsCount) * -buttonsSize ||
                rectTransform.anchoredPosition.x > 0)
                {
                    scrollRect.decelerationRate = 0;
                }
                else
                {
                    scrollRect.decelerationRate = decelerationRate;
                }
                rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x * -1, 0, (transform.childCount - maxButtonsCount) * buttonsSize) * -1,
                    rectTransform.anchoredPosition.y);
            }
        }
    }
}
