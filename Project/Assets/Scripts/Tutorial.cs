using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public float lerpTime = 0.3f;
    Vector3 startScale;
    Vector3 targetScale;
    float timeStartedLerping;
    bool isClosing = false;

    void Start()
    {
        timeStartedLerping = Time.unscaledTime;
        targetScale = new Vector3(1, 1, 1);
        startScale = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(0, 0, 0);
    }

    void Update()
    {
        transform.localScale = Lerp(startScale, targetScale, timeStartedLerping, lerpTime);
        if (transform.localScale == new Vector3(0, 0, 0) && isClosing)
        {
            Destroy(gameObject);
        }
    }

    public void Close()
    {
        if (!isClosing)
        {
            GlobalSetting.UISoundSource.PlayOneShot(GlobalSetting.UISoundSource.clip);
            timeStartedLerping = Time.unscaledTime;
            startScale = transform.localScale;
            targetScale = new Vector3(0f, 0f, 0f);
            isClosing = true;
        }
    }

    Vector3 Lerp(Vector3 startPos, Vector3 endPos, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        Vector3 result = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
