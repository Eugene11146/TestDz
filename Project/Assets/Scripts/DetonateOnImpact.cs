using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateOnImpact : MonoBehaviour
{
    public Detonator detonator;
    public float detonateForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > detonateForce)
        {
            detonator.Detonate(transform.position);
        }
    }
}
