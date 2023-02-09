using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIObjectPos : MonoBehaviour
{
    public GameObject trgObject;
    public float lerpTime = 0.2f;
    public bool isDetroying = false;
    RectTransform rectTrasform;
    float timeStartedLerping;
    float target = 1000f;
    float start;

    private void Start()
    {
        rectTrasform = GetComponent<RectTransform>();
        timeStartedLerping = Time.unscaledTime;
        rectTrasform.sizeDelta = new Vector2(0, 0);
    }

    void Update()
    {
        if (isDetroying && target != 0)
        {
            timeStartedLerping = Time.unscaledTime;
            start = target;
            target = 0;
        }
        if (Mathf.Abs(rectTrasform.sizeDelta.x - target) > 0)
        {
            float lerp = Lerp(start, target, timeStartedLerping, lerpTime);
            rectTrasform.sizeDelta = new Vector2(lerp, lerp);
        }
        if (trgObject != null)
        {
            rectTrasform.position = GlobalSetting.mainCamera.WorldToScreenPoint(trgObject.transform.position);
            transform.GetChild(0).rotation = trgObject.transform.rotation;
        }
        else
        {
            isDetroying = true;
        }
        if (rectTrasform.sizeDelta.x == 0)
            Destroy(gameObject);
    }

    float Lerp(float start, float end, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
