using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSound : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        source.pitch = Time.timeScale;
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
