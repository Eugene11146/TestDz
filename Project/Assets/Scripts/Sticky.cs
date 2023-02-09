using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    public bool activated = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.sharedMaterial != null && collision.collider.sharedMaterial.name != "Ground" && activated)
        {
            DistanceJoint2D dist = gameObject.AddComponent<DistanceJoint2D>();
            dist.connectedBody = collision.rigidbody;
            FixedJoint2D fix = gameObject.AddComponent<FixedJoint2D>();
            fix.connectedBody = collision.rigidbody;
        }
    }

    public void Activate()
    {
        if (!activated)
            activated = true;
    }
}
