using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraSoundEffect : MonoBehaviour
{
    public AudioMixer mixerGroup;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //mixerGroup.SetFloat("EffectsVolume", 0 - (Mathf.Clamp01(mainCamera.orthographicSize / 10 - 0.3f) * 80));
        mixerGroup.SetFloat("EffectsLowpass", 22000 * Mathf.Clamp01(1 / mainCamera.orthographicSize));
    }
}
