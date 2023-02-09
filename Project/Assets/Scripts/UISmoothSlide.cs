using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISmoothSlide : MonoBehaviour
{
    public GameObject deactivatedObject;
    public bool deactivate;
    public bool deactivateOnClose;
    public Vector2 targetPos;
    public Vector2 target;
    public Vector2 startPos;
    public Vector2 start;
    public float timeStartedLerping;
    public float lerpTime;
    public Animator animator;
    public RectTransform rectTransform;
    public bool customLerpSpeed = false;
    bool toggle = true;
    bool started = false;

    private void Start()
    {
        startPos = rectTransform.anchoredPosition;
        target = startPos;
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
                rectTransform.anchoredPosition = Lerp(start, target, timeStartedLerping, lerpTime);
            else
                rectTransform.anchoredPosition = Lerp(start, target, timeStartedLerping, GlobalSetting.menuLerpTime);
        }

        if (deactivate && started)
        {
            if (deactivateOnClose)
            {
                deactivatedObject.SetActive(rectTransform.anchoredPosition != startPos);
            }
            else
            {
                deactivatedObject.SetActive(rectTransform.anchoredPosition != startPos + targetPos);
            }
        }
    }

    public void Open()
    {
        timeStartedLerping = Time.unscaledTime;
        target = startPos + targetPos;
        start = rectTransform.anchoredPosition;
    }
    public void Close()
    {
        timeStartedLerping = Time.unscaledTime;
        target = startPos;
        start = rectTransform.anchoredPosition;
    }
    public void Toggle()
    {
        if (target == startPos)
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

        Vector3 result = Vector3.Lerp(startPos, endPos, Mathf.Pow(Mathf.SmoothStep(0, 1, percentageComplete), 0.5f));

        return result;
    }
}
