using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotion : MonoBehaviour
{

    public float slowMotionScale;
    public float fastMotionScale;
    public float timeStartedLerping;
    public float lerpTime = 1;
    public Slider slider;
    public float target = 1;
    public float start = 0;

    Camera mainCamera;
    SettingsData settingsData;
    AdsManager adsManager;

    private void Start()
    {
        mainCamera = Camera.main;
        settingsData = GlobalSetting.settings;
        adsManager = mainCamera.GetComponent<AdsManager>();
    }

    void Update()
    {
        slowMotionScale = settingsData.slowMotionScale;
        if (slider != null)
            slider.value = Time.timeScale;
        if (Mathf.Abs(Time.timeScale - target) > 0)
        {
            Time.timeScale = Lerp(start, target, timeStartedLerping, lerpTime);
        }
        if (adsManager.paused)
        {
            Time.timeScale = 0;
        }
    }

    public void ToggleSlow()
    {
        timeStartedLerping = Time.unscaledTime;
        start = Time.timeScale;
        if (target != slowMotionScale)
        {
            target = slowMotionScale;
        }
        else
            target = 1;
    }
    public void setScale(float scale)
    {
        timeStartedLerping = Time.unscaledTime;
        start = Time.timeScale;
        target = scale;
    }
    public void Fast()
    {
        timeStartedLerping = Time.unscaledTime;
        start = Time.timeScale;
        if (target != 0)
            target = fastMotionScale;
    }
    public void Play()
    {
        timeStartedLerping = Time.unscaledTime;
        start = Time.timeScale;
        target = 1;
    }
    public void Pause()
    {
        timeStartedLerping = Time.unscaledTime;
        start = Time.timeScale;
        target = 0;
    }
    public void Toggle()
    {
        if (target != 0)
            Pause();
        else
            Play();
    }

    float Lerp(float start, float end, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
