using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    public GameObject sprayEffect;
    public GameObject underWaterSprayEffect;
    public Transform sprayTransform;
    public AudioSource loopSource;
    public AudioSource startSource;
    public AudioSource stopSource;
    public Splash splash;
    public bool isShooting = false;
    public bool toggleActivation = false;

    public string effectName;

    GameObject thisEffect;
    bool lastWater—ondition = false;
    bool isShootingLate = false;

    private void Update()
    {
        loopSource.pitch = Time.timeScale;
        startSource.pitch = Time.timeScale;
        stopSource.pitch = Time.timeScale;
        if (isShooting || toggleActivation)
        {
            if (!isShootingLate)
            {
                isShootingLate = true;
                loopSource.Play();
                startSource.PlayOneShot(startSource.clip);
            }
            if (thisEffect == null)
            {
                SpawnEffect();
            }
            else
            {
                if (TryGetComponent(out ParticleEffect particleEffect))
                    particleEffect.effectName = effectName;
                if (splash != null && splash.isUnderWater != lastWater—ondition)
                {
                    Destroy(thisEffect);
                    SpawnEffect();
                }
            }
        }
        else
        {
            if (isShootingLate)
            {
                isShootingLate = false;
                loopSource.Stop();
                stopSource.PlayOneShot(stopSource.clip);
            }
            if (thisEffect != null)
            {
                thisEffect.GetComponent<ParticleSystem>().Stop();
            }
        }
        if (splash != null)
            lastWater—ondition = splash.isUnderWater;
    }
    void SpawnEffect()
    {
        if ((splash != null && !splash.isUnderWater) || underWaterSprayEffect == null)
            thisEffect = Instantiate(sprayEffect, sprayTransform.position, sprayTransform.rotation);
        else
            thisEffect = Instantiate(underWaterSprayEffect, sprayTransform.position, sprayTransform.rotation);
        thisEffect.transform.SetParent(transform);
        thisEffect.transform.localScale = thisEffect.transform.lossyScale;
    }
}

