using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLocalRotation : MonoBehaviour
{
    ParticleSystem.Particle[] part = new ParticleSystem.Particle[1];
    public ParticleSystem system;

    private void Update()
    {
        system.GetParticles(part);
        part[0].rotation = -transform.eulerAngles.z;
        system.SetParticles(part);
    }
}
