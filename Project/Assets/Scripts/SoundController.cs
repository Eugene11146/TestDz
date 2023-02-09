using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public SoundPack[] pack;
    public AudioClip[] stabPack;
    public AudioMixerGroup group;
    public AudioClip[] fastBurn;
}

[System.Serializable]
public class SoundPack
{
    public string name;
    public AudioClip[] softImpact;
    public AudioClip[] bulletImpact;
    public AudioClip[] friction;
}
