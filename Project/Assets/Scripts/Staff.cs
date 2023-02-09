using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public GameObject freezeEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 8)
        {
            Instantiate(freezeEffect, transform.position, new Quaternion());
            var colliders = Physics2D.OverlapCircleAll(transform.position, 2);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Properties properties) && collider.gameObject != gameObject)
                {
                    properties.temperature = -127;
                }
            }
        }
    }
}
