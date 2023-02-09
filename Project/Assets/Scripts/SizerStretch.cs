using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizerStretch : MonoBehaviour
{
    public RectTransform mainRect;
    public GameObject trgObject;
    public GameObject mainObject;
    public float lerpTime = 0.2f;
    public bool vertical = true;
    public bool bothSides = false;
    Vector2 mainRectStartPos;
    float startDist;
    float startObjectDist;
    bool isStreching = false;
    bool inverse = false;

    float timeStartedLerping;
    float target = 70f;
    float start;

    private void Start()
    {
        timeStartedLerping = Time.unscaledTime;
        if (!bothSides)
        {
            if (vertical)
            {
                mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, 0);
            }
            else
            {
                mainRect.sizeDelta = new Vector2(0, mainRect.sizeDelta.y);
            }
        }
    }

    void Update()
    {
        trgObject = mainObject.GetComponent<SetUIObjectPos>().trgObject;
        if (mainObject.GetComponent<SetUIObjectPos>().isDetroying && target != 0)
        {
            Close();
        }
        if (!isStreching && !bothSides)
        {
            if (vertical)
            {
                if (Mathf.Abs(mainRect.sizeDelta.y - target) > 0)
                {
                    mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, Lerp(start, target, timeStartedLerping, lerpTime));
                }
            }
            else
            {
                if (Mathf.Abs(mainRect.sizeDelta.x - target) > 0)
                {
                    mainRect.sizeDelta = new Vector2(Lerp(start, target, timeStartedLerping, lerpTime), mainRect.sizeDelta.y);
                }
            }
        }
    }

    public void OnStretchStart()
    {
        isStreching = true;
        mainRectStartPos = Input.mousePosition;
        if (vertical)
        {
            startDist = mainRect.sizeDelta.y;
            startObjectDist = trgObject.transform.localScale.y;
        }
        else
        {
            startDist = mainRect.sizeDelta.x;
            startObjectDist = Mathf.Abs(trgObject.transform.localScale.x);
        }
        if (trgObject.transform.localScale.x < 0)
        {
            inverse = true;
        }
    }
    public void OnStretch()
    {
        if (!bothSides)
        {
            if (vertical)
            {
                mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, startDist + (GlobalSetting.mainCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - mainRectStartPos)
                                        * GlobalSetting.mainCamera.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta).y / 6);
                mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, Mathf.Clamp(mainRect.sizeDelta.y, 25, 115));
                trgObject.transform.localScale = new Vector2(trgObject.transform.localScale.x, (mainRect.sizeDelta.y - startDist) / 50 * GlobalSetting.mainCamera.orthographicSize + startObjectDist);
            }
            else
            {
                mainRect.sizeDelta = new Vector2(startDist + (GlobalSetting.mainCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - mainRectStartPos)
                                        * GlobalSetting.mainCamera.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta).x / 6, mainRect.sizeDelta.y);
                mainRect.sizeDelta = new Vector2(Mathf.Clamp(mainRect.sizeDelta.x, 25, 115), mainRect.sizeDelta.y);
                trgObject.transform.localScale = new Vector2((mainRect.sizeDelta.x - startDist) / 50 * GlobalSetting.mainCamera.orthographicSize + startObjectDist, trgObject.transform.localScale.y);
            }
        }
        else
        {
            float size = startDist + (GlobalSetting.mainCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - mainRectStartPos)
                            * GlobalSetting.mainCamera.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta).x / 6;
            trgObject.transform.localScale = new Vector2((size - startDist) / 50 * GlobalSetting.mainCamera.orthographicSize + startObjectDist,
                (size - startDist) / 50 * GlobalSetting.mainCamera.orthographicSize + startObjectDist);
        }
        trgObject.transform.localScale = new Vector2(Mathf.Clamp(Mathf.Abs(trgObject.transform.localScale.x), 0.3f, 30), Mathf.Clamp(trgObject.transform.localScale.y, 0.3f, 30));
        if (inverse)
        {
            trgObject.transform.localScale = new Vector2(-trgObject.transform.localScale.x, trgObject.transform.localScale.y);
        }
    }
    public void OnStretchEnd()
    {
        if (trgObject.TryGetComponent(out AutoMass auto))
        {
            auto.CalculateMass();
        }
        if (inverse)
            inverse = false;
        timeStartedLerping = Time.unscaledTime;
        if (vertical)
            start = mainRect.sizeDelta.y;
        else
            start = mainRect.sizeDelta.x;
        isStreching = false;
    }
    float Lerp(float start, float end, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
    public void Close()
    {
        if (!bothSides)
        {
            timeStartedLerping = Time.unscaledTime;
            start = target;
            target = 0;
        }
    }
}
