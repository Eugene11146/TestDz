using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public bool isUnderWater = false;
    public bool effect = true;
    public float scale = 1f;
    SpriteRenderer spriteRenderer;
    float splashDelay = 0.1f;
    float ticker;

    GameObject lastSplashEffect;

    private void Start()
    {
        ticker = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (TryGetComponent(out Properties properties) &&  !properties.enabled)
        {
            properties.enabled = true;
        }
    }

    private void Update()
    {
        if (ticker > 0)
            ticker -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isUnderWater = true;
            ShowSplash();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isUnderWater = false;
            ShowSplash();
        }
    }

    void ShowSplash()
    {
        if (ticker <= 0 && TryGetComponent(out Rigidbody2D rigBody) && effect && rigBody.velocity.magnitude > 0.5f)
        {
            PlaySound(GlobalSetting.splashSounds[Random.Range(0, GlobalSetting.splashSounds.Length)], Mathf.Clamp(rigBody.velocity.magnitude / 100f, 0, 0.5f));
            GameObject splash = Instantiate(GlobalSetting.splashEffect, new Vector3(transform.position.x, GlobalSetting.waterLevel, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            lastSplashEffect = splash;

            splash.transform.localScale = new Vector3(scale, scale, 1);
            float pixelMuliply = (2500 / spriteRenderer.sprite.pixelsPerUnit / spriteRenderer.sprite.pixelsPerUnit);
            var shape = splash.GetComponent<ParticleSystem>().shape;
            shape.radius = (transform.right * new Vector2(spriteRenderer.sprite.rect.size.x, spriteRenderer.sprite.rect.size.y)).magnitude / 80 * scale * pixelMuliply;
            float rotation = Mathf.Atan2(rigBody.velocity.normalized.x, rigBody.velocity.normalized.y) * Mathf.Rad2Deg;
            if (rotation > 90)
            {
                rotation -= 180;
                rotation *= -1;
            }
            else if (rotation < -90)
            {
                rotation += 180;
                rotation *= -1;
            }
            shape.rotation = new Vector3(0, Mathf.Clamp(rotation, -70, 70), 0);

            var mainParticle = splash.GetComponent<ParticleSystem>().main;
            var startSpeed = mainParticle.startSpeed;
            startSpeed.constantMin = Mathf.Clamp(Mathf.Abs(rigBody.velocity.y) * scale / 15, 0, 25);
            startSpeed.constantMax = Mathf.Clamp(Mathf.Abs(rigBody.velocity.y) * scale / 10, 0, 25);
            mainParticle.startSpeed = startSpeed;
            mainParticle.maxParticles = Mathf.Clamp((int)(transform.right * new Vector2(spriteRenderer.sprite.rect.size.x, spriteRenderer.sprite.rect.size.y) * pixelMuliply).magnitude / 5, 2, 7);

            mainParticle.startSize = Mathf.Clamp((transform.right * new Vector2(spriteRenderer.sprite.rect.size.x, spriteRenderer.sprite.rect.size.y)).magnitude / 40 * scale * pixelMuliply, 0.2f, 2000);

            var mainChildParticle = splash.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            mainChildParticle.startSizeX = mainParticle.startSize.constant * 2;
            mainChildParticle.startSizeY = mainChildParticle.startSizeY.constant + mainParticle.startSize.constant * 0.1f;

            //splash.GetComponent<ParticleSystemRenderer>().maxParticleSize = Mathf.Clamp((transform.right * new Vector2(spriteRenderer.sprite.rect.size.x, spriteRenderer.sprite.rect.size.y)).magnitude / 800 * scale, 0.2f, 2000);
            //splash.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().maxParticleSize = Mathf.Clamp(splash.GetComponent<ParticleSystemRenderer>().maxParticleSize * scale, 0.2f, 2000);

            splash.GetComponent<ParticleSystem>().Play();
            ticker = splashDelay;
        }
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        GameObject emmiter = Instantiate(GlobalSetting.soundEmmiter, transform);
        emmiter.GetComponent<AudioSource>().clip = clip;
        emmiter.GetComponent<AudioSource>().volume = volume;
        emmiter.GetComponent<AudioSource>().Play();
    }

    private void OnDestroy()
    {
        if (lastSplashEffect != null)
        {
            Destroy(lastSplashEffect);
        }
    }
}
