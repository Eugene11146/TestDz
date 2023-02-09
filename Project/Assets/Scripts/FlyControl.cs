using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControl : MonoBehaviour
{
    Rigidbody2D rigBody;
    void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 dir = rigBody.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Debug.Log(((-angle - rigBody.rotation - 90) * (rigBody.velocity.magnitude / 3000) - rigBody.angularVelocity / 7000) * Mathf.Clamp01(rigBody.velocity.magnitude * 2));
        rigBody.AddTorque(((angle - rigBody.rotation) * (rigBody.velocity.magnitude / 3000) - rigBody.angularVelocity / 7000) * Mathf.Clamp01(rigBody.velocity.magnitude * 2));
    }
}
