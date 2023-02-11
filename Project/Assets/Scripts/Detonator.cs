using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    public float explosionRadius;
    public float explosionPower;
    public float explosionDamage;
    public float delay = 3f;
    public float temperatureAddition = 0;
    public bool flash = false;
    public AudioSource detonateSound;
    public AudioClip explosionSound;
    public GameObject explosionSoundEmitter;
    public GameObject explosionEffect;
    public GameObject explosionEffectUnderWater;
    public int raysCount = 360;
    public GranadeActivation anim;
    Splash splash;
    bool acivated = false;

    private void Start()
    {
        splash = GetComponent<Splash>();
    }

    private void Update()
    {
        if (detonateSound != null)
        {
            detonateSound.pitch = Time.timeScale;
        }
    }

    public void Active()
    {
        if (!acivated)
        {
            if (anim != null)
                anim.Activate();
            if (detonateSound != null && !detonateSound.isPlaying)
                detonateSound.Play();
            if (delay > 0)
                StartCoroutine(Delay(delay));
            else
                Detonate(transform.position);
            gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
        acivated = true;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);

        Detonate(transform.position);
    }

    public void Detonate(Vector2 explosionPos)
    {
        if (anim == null)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        if (splash.isUnderWater)
            Instantiate(explosionEffectUnderWater, transform.position, new Quaternion());
        else
            Instantiate(explosionEffect, transform.position, new Quaternion());
        GameObject emitter;
        if (splash.isUnderWater)
            emitter = Instantiate(explosionSoundEmitter, transform.position + new Vector3(0, 0, 100), new Quaternion());
        else
            emitter = Instantiate(explosionSoundEmitter, transform.position, new Quaternion());
        emitter.GetComponent<AudioSource>().clip = explosionSound;
        emitter.GetComponent<AudioSource>().pitch = Time.timeScale;
        emitter.GetComponent<AudioSource>().Play();

        var Colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in Colliders)
            Destroy(collider);

        for (int i = 0; i < raysCount; ++i)
        {
            Vector2 dir = new Vector2(Mathf.Cos(360f / raysCount * i * Mathf.Deg2Rad), (Mathf.Sin(360f / raysCount * i * Mathf.Deg2Rad)));
            RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, explosionRadius, GlobalSetting.explosionLayerMask);
            Collider2D coll = ray.collider;
            if (coll != null)
            {
                if (coll.TryGetComponent(out Properties properties))
                {
                    properties.temperature += temperatureAddition * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius);
                }
                else if (coll.transform.root.TryGetComponent(out Properties parentProperties))
                {
                    parentProperties.temperature += temperatureAddition * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius);
                }
            }
            if (coll != null && coll.transform.parent != null && coll.transform.parent.TryGetComponent(out Rigidbody2D crb) && coll.sharedMaterial != null && coll.sharedMaterial.name == "Cloth")
            {
                if (explosionDamage * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius) > 0)
                { 
                    if (flash)
                    {
                        coll.transform.parent.SendMessage("Flash", SendMessageOptions.DontRequireReceiver);
                    }
                    crb.AddForce(((Vector2)crb.transform.position - explosionPos) * explosionPower, ForceMode2D.Impulse);
                    if (coll.transform.parent.CompareTag("Breakable"))
                    {
                        if (coll.transform.parent.TryGetComponent(out Breakable br))
                        {
                            br.Hit(explosionDamage * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius));
                        }
                    }
                }
            }
            if (coll != null && coll.TryGetComponent(out Rigidbody2D rb))
            {
                if (explosionDamage * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius) > 0)
                {
                    if (flash)
                    {
                        coll.SendMessage("Flash", SendMessageOptions.DontRequireReceiver);
                    }
                    rb.AddForce(((Vector2)rb.transform.position - explosionPos) * explosionPower, ForceMode2D.Impulse);
                    if (coll.CompareTag("Breakable"))
                    {
                        if (coll.TryGetComponent(out Breakable br))
                        {
                            br.Hit(explosionDamage * (1 - ((Vector2)coll.transform.position - explosionPos).magnitude / explosionRadius));
                        }
                    }
                }
            }
        }
        Destroy(gameObject);


    }

}
