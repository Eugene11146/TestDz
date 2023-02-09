using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numpad : MonoBehaviour
{
    AudioSource audioSource;
    AudioSource audioSourceError;
    string code = "";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceError = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (code.Length >= 4)
        {
            if (code == "2022")
            {
                //Пасхалка
            }
            else
            {
                code = "";
                audioSourceError.PlayOneShot(audioSourceError.clip);
            }
        }
    }

    public void one()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.5f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '1';
        }
    }
    public void two()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.55f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '2';
        }
    }
    public void three()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.6f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '3';
        }
    }
    public void four()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.65f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '4';
        }
    }
    public void five()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.7f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '5';
        }
    }
    public void six()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.75f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '6';
        }
    }
    public void seven()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.8f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '7';
        }
    }
    public void eight()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.85f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '8';
        }
    }
    public void nine()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.9f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '9';
        }
    }
    public void zero()
    {
        if (code.Length < 4)
        {
            audioSource.pitch = 0.95f;
            audioSource.PlayOneShot(audioSource.clip);
            code += '0';
        }
    }
}
