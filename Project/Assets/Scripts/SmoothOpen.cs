using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothOpen : MonoBehaviour
{
    public GameObject deactivatedObject;
    public bool deactivate;
    public bool deactivateOnClose;
    public Vector2 targetSize;
    public Vector2 target;
    public Vector2 startSize;
    public Vector2 start;
    public float timeStartedLerping;
    public float lerpTime;
    public Animator animator;
    public RectTransform rectTransform;
    public bool customLerpSpeed = false;
    bool toggle = true;
    bool started = false;

    private void Awake()
    {
        startSize = rectTransform.sizeDelta;
        target = startSize;
        if (deactivate && deactivateOnClose)
        {
            StartCoroutine(delayedDeactivate());
        }
    }

    IEnumerator delayedDeactivate()
    {
        yield return new WaitForSecondsRealtime(0.02f);

        started = true;
    }

    void Update()
    {
        if (Vector3.Magnitude(rectTransform.anchoredPosition - target) > 0)
        {
            if (customLerpSpeed)
                rectTransform.sizeDelta = Lerp(start, target, timeStartedLerping, lerpTime);
            else
                rectTransform.sizeDelta = Lerp(start, target, timeStartedLerping, GlobalSetting.menuLerpTime);
        }

        if (deactivate && started)
        {
            if (deactivateOnClose)
            {
                deactivatedObject.SetActive(rectTransform.anchoredPosition != startSize);
            }
            else
            {
                deactivatedObject.SetActive(rectTransform.anchoredPosition != startSize + targetSize);
            }
        }
    }

    public void Open()
    {
        timeStartedLerping = Time.unscaledTime;
        target = startSize + targetSize;
        start = rectTransform.sizeDelta;
    }
    public void Close()
    {
        timeStartedLerping = Time.unscaledTime;
        target = startSize;
        start = rectTransform.sizeDelta;
    }
    public void Toggle()
    {
        if (target == startSize)
        {
            Open();
            if (animator != null)
                animator.SetBool("Toggle", toggle);
        }
        else
        {
            Close();
            if (animator != null)
                animator.SetBool("Toggle", toggle);
        }
        toggle = !toggle;
    }

    Vector3 Lerp(Vector3 startPos, Vector3 endPos, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        Vector3 result = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
