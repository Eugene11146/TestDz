using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkRandomize : MonoBehaviour
{
    ParticleSystem.Particle[] part;
    public ParticleSystem system;
    public float max;
    public float min;
    float[] rand;

    private void Start()
    {
        rand = new float[system.main.maxParticles];
        for (int i = 0; i < system.main.maxParticles; ++i)
        {
            rand[i] = Random.Range(min, max);
        }
    }

    private void Update()
    {
        part = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(part);
        for (int i = 0; i < part.Length; ++i)
        {
            part[i].rotation = -transform.root.eulerAngles.z + rand[i] - 90;
        }
        system.SetParticles(part);
    }
}
