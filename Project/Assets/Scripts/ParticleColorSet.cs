using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorSet : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public Color color;

    private void Awake()
    {
        Set();
    }

    public void Set()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            var main = particleSystem.main;
            main.startColor = new Color(color.r, color.g, color.b, main.startColor.color.a);
        }
    }
}
