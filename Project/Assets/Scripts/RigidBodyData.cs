using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyData : MonoBehaviour
{
    Rigidbody2D rigbody2D;
    public float mass;
    public Vector2 centreOfMass;

    public void Remember()
    {
        rigbody2D = GetComponent<Rigidbody2D>();
        mass = rigbody2D.mass;
        centreOfMass = rigbody2D.centerOfMass;
    }
}
