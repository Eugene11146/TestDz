using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] sounds;

    public void Play()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
    }
}
