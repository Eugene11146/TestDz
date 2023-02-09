using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeActivation : MonoBehaviour
{
    public Sprite mainGranade;
    public ParticleSystem[] particles;

    public void Activate()
    {
        GetComponent<SpriteRenderer>().sprite = mainGranade;
        foreach (ParticleSystem part in particles)
        {
            part.Play();
        }
    }
}
