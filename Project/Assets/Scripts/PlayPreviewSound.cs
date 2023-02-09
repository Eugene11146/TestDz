using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayPreviewSound : MonoBehaviour
{
    public AudioSource source;
    float ticker = 0;

    private void Update()
    {
        if (ticker > 0)
        {
            ticker -= Time.unscaledDeltaTime;
        }
    }

    public void Play()
    {
        if (ticker <= 0)
        {
            source.PlayOneShot(source.clip);
            ticker = 0.1f;
        }
    }
}
