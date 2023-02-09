using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornPop : MonoBehaviour
{
    public GameObject cornEffect;

    Properties properties;
    int popCounter = 0;

    private void Start()
    {
        properties = GetComponent<Properties>();
    }

    void FixedUpdate()
    {
        int randomNumber = Random.Range(0, 80);
        if (properties.temperature > 100 && properties.isOnFire && popCounter < 130 && randomNumber == 3)
        {
            Vector3 randomPos = GetComponent<Collider2D>().ClosestPoint(transform.TransformPoint(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized));

            ParticleSystem particleSystem;
            particleSystem = Instantiate(cornEffect, randomPos, new Quaternion()).GetComponent<ParticleSystem>();
            particleSystem.Play();
            ++popCounter;
        }
    }
}
