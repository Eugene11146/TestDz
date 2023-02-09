using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject gibsContainer;
    public GameObject soundEmitter;
    public ParticleSystem onDestroyEffect;
    public AudioClip[] breakSounds;
    public float health = 50f;
    public float breakForce = 50f;
    bool spawned = false;
    bool breaked = false;
    Vector3 lastVelocity;

    private void Update()
    {
        lastVelocity = GetComponent<Rigidbody2D>().velocity;
        if (GetComponent<Properties>().thisMaterial.GetFloat("_BurnScale") == 1)
        {
            Break();
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Break();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float hitPower;
            hitPower = collision.relativeVelocity.magnitude;
        if (collision.relativeVelocity.magnitude > breakForce && !spawned && !collision.collider.CompareTag("Bullet"))
        {
            lastVelocity = collision.relativeVelocity.normalized * hitPower;
            if (!collision.transform.TryGetComponent(out Rigidbody2D rigidBody))
            {
                lastVelocity *= -1;
            }
            if (collision.gameObject.CompareTag("Bullet"))
                lastVelocity /= 8;
            Break();
        }
    }

    public void Break()
    {
        if (!breaked)
        {
            foreach (Transform child in transform)
            {
                if (!child.CompareTag("Editor") && !child.TryGetComponent(out RigidBodyData f) && !child.CompareTag("Particle") && !child.CompareTag("EffectParticle"))
                    child.SetParent(null);
                else
                    Destroy(child.gameObject);
            }

            GameObject emitter = Instantiate(soundEmitter, transform.position, new Quaternion());
            emitter.GetComponent<AudioSource>().clip = breakSounds[Random.Range(0, breakSounds.Length)];
            emitter.GetComponent<AudioSource>().outputAudioMixerGroup = GlobalSetting.effectsAudioMixerGroup;
            emitter.GetComponent<AudioSource>().pitch = Time.timeScale;
            emitter.GetComponent<AudioSource>().Play();
            if (onDestroyEffect != null)
            {
                GameObject particle = Instantiate(onDestroyEffect, transform.position, transform.rotation).gameObject;
                particle.transform.localScale = transform.lossyScale;
                var shape = particle.GetComponent<ParticleSystem>().shape;
                shape.texture = GetComponent<SpriteRenderer>().sprite.texture;
                shape.sprite = GetComponent<SpriteRenderer>().sprite;
                particle.GetComponent<ParticleSystem>().Play();
            }

            if (gibsContainer != null)
            {
                GameObject gibs = Instantiate(gibsContainer, transform.position, transform.rotation);
                gibs.transform.localScale = transform.localScale;
                var gibsRB = gibs.GetComponentsInChildren<Rigidbody2D>();

                var colliders = gibs.GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; ++i)
                {
                    for (int j = i + 1; j < colliders.Length; ++j)
                    {
                        Physics2D.IgnoreCollision(colliders[i], colliders[j]);
                    }
                }

                foreach (Rigidbody2D gib in gibsRB)
                {
                    gib.velocity = GetComponent<Rigidbody2D>().velocity;
                    gib.GetComponent<Properties>().thisMaterial.SetFloat("_BurnScale", GetComponent<Properties>().thisMaterial.GetFloat("_BurnScale"));
                    gib.GetComponent<Properties>().temperature = GetComponent<Properties>().temperature;
                }
                int childCount = gibs.transform.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    gibs.transform.GetChild(0).SetParent(null);
                }

                Destroy(gibs);
            }

            spawned = true;
            breaked = true;
            Destroy(gameObject);
        }
    }
}
