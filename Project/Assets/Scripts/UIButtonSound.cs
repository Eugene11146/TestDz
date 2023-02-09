using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIButtonSound : MonoBehaviour
{
    public void Play()
    {
        GlobalSetting.UISoundSource.PlayOneShot(GlobalSetting.UISoundSource.clip);
    }
}
