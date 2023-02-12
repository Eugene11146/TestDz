using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Контроллер бомбы-прыгуньи
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class JumpBomb : MonoBehaviour
{
    /// <summary>
    /// Зввук взрыва
    /// </summary>
    [SerializeField]
    public AudioSource AudioSource;
    
    /// <summary>
    /// Эффект взрыва
    /// </summary>
    [SerializeField]
    public GameObject ExplosionEffect;

    [SerializeField]
    private float force;

    [SerializeField]
    private float timeForDestroy;

    [SerializeField]
    private float speedToDestroy = 10;

    private Rigidbody2D rigidbody;

    private bool isBloweUp = default;

    private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

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
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        AudioSource.Play();
        rigidbody.transform.localScale = Vector3.zero;
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(timeForDestroy);
        Destroy(gameObject);
    }
}
