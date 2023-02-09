using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISmoothColor : MonoBehaviour
{
    public GameObject deactivatedObject;
    public Animator animator;
    public Color targetColor;
    public Color startColor;
    public Color target;
    public Color start;
    public float timeStartedLerping;
    public bool deactivateOnClose;
    public float lerpTime;
    public bool customLerpSpeed = false;
    public bool deactivate;

    Image image;
    bool toggle = true;
    bool started = false;

    private void Start()
    {
        image = GetComponent<Image>();
        startColor = image.color;
        target = startColor;
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
        if (image.color - target != new Color(0, 0, 0, 0))
        {
            if (customLerpSpeed)
                image.color = Lerp(start, target, timeStartedLerping, lerpTime);
            else
                image.color = Lerp(start, target, timeStartedLerping, GlobalSetting.menuLerpTime);
        }

        if (deactivate && started)
        {
            if (deactivateOnClose)
            {
                deactivatedObject.SetActive(image.color != startColor);
            }
            else
            {
                deactivatedObject.SetActive(image.color != targetColor);
            }
        }
    }

    public void Open()
    {
        timeStartedLerping = Time.unscaledTime;
        target = targetColor;
        start = image.color;
    }
    public void Close()
    {
        timeStartedLerping = Time.unscaledTime;
        target = startColor;
        start = image.color;
    }
    public void Toggle()
    {
        if (target == startColor)
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

    Color Lerp(Color startColor, Color endPos, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        Color result = Color.Lerp(startColor, endPos, Mathf.Pow(Mathf.SmoothStep(0, 1, percentageComplete), 0.5f));

        return result;
    }
}
