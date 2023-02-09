using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeVolume : MonoBehaviour
{
    public AudioMixerGroup group;

    public void ChangeGroupVolume(float value)
    {
        group.audioMixer.SetFloat(group.name + "Volume", value * 0.8f - 80);
    }
}
