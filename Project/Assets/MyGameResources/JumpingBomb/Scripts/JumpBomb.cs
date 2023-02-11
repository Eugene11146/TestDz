using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpBomb : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource;
    
    [SerializeField]
    public GameObject explosionEffect;

    [SerializeField]
    private float force;

    [SerializeField]
    private float timeForDestroy;

    [SerializeField]
    private float speedToDestroy = 10;

    private Rigidbody2D rigidbody;

    private bool isBloweUp = default;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x * force, rigidbody.velocity.y);
    }

    private void Update()
    {
        if (rigidbody.velocity.x >= speedToDestroy && !isBloweUp)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        isBloweUp = true;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        audioSource.Play();
        rigidbody.transform.localScale = Vector3.zero;
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(timeForDestroy);
        Destroy(gameObject);
    }

}
