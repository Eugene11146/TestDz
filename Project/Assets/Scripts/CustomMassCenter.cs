using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMassCenter : MonoBehaviour
{
    public Vector3 mass;
    void Start()
    {
        GetComponent<Rigidbody2D>().centerOfMass = mass;
    }
}
