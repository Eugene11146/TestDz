using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    bool toggle = false;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void Update()
    {
        rb.freezeRotation = toggle;
    }

    public void Active()
    {
        toggle = !toggle;
    }
}
