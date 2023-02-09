using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMelt : MonoBehaviour
{
    public GameObject waterEffect;
    Properties properties;

    private void Start()
    {
        properties = GetComponent<Properties>();
    }

    private void Update()
    {
        if (properties.temperature > 50)
        {
            foreach (Transform child in transform)
            {
                child.SetParent(transform.parent);
            }
            Instantiate(waterEffect, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Instantiate(waterEffect, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
}
