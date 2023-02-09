using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundController : MonoBehaviour
{
    AudioSource source;
    AudioSource frictionSource;
    Rigidbody2D rigidBody;
    Camera mainCamera;
    float lastAngularVelocity;

    private void Start()
    {
        var sources = GetComponents<AudioSource>();
        foreach (AudioSource audio in sources)
        {
            Destroy(audio);
        }
        mainCamera = Camera.main;
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mainCamera.GetComponent<SoundController>().group;
        source.spatialBlend = 1;
        source.minDistance = 10;
        if (frictionSource == null)
            frictionSource = gameObject.AddComponent<AudioSource>();
        frictionSource.outputAudioMixerGroup = mainCamera.GetComponent<SoundController>().group;
        frictionSource.spatialBlend = 1;
        frictionSource.minDistance = 10;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        source.pitch = Time.timeScale;
        frictionSource.pitch = Time.timeScale;
        if (rigidBody != null)
        {
            if (rigidBody.velocity.magnitude <= 0.3f || Mathf.Abs(rigidBody.angularVelocity) >= 15f)
            {
                frictionSource.Stop();
                frictionSource.loop = false;
            }
            lastAngularVelocity = rigidBody.angularVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidBody != null)
        {
            if (collision.collider.CompareTag("Bullet"))
            {
                PlayBulletImpact(collision.otherCollider.sharedMaterial.name[1] - 65);
            }
            else if (((collision.relativeVelocity != null && collision.relativeVelocity.magnitude > 1) || Mathf.Abs(rigidBody.angularVelocity - lastAngularVelocity) > 100)
                && collision.collider.sharedMaterial != null && collision.collider.sharedMaterial.name != "Flesh" && collision.otherCollider.sharedMaterial != null)
            {
                if (collision.otherCollider.sharedMaterial.name[0] == '8')
                {
                    switch (collision.otherCollider.tag)
                    {
                        case "Mechanic":
                            PlayImpact(1, collision.relativeVelocity.magnitude);
                            break;
                        case "Weapon":
                            PlayImpact(2, collision.relativeVelocity.magnitude);
                            break;
                        case "MelleeWeapon":
                            PlayImpact(3, collision.relativeVelocity.magnitude);
                            break;
                        default:
                            PlayImpact(6, collision.relativeVelocity.magnitude);
                            break;
                    }
                }
                else
                {
                    PlayImpact(collision.otherCollider.sharedMaterial.name[1] - 65, collision.relativeVelocity.magnitude);
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rigidBody != null)
        {
            if (rigidBody.velocity.magnitude > 0.3f && Mathf.Abs(rigidBody.angularVelocity) < 15f)
            {
                if ((collision.gameObject.TryGetComponent(out Rigidbody2D rigBody) && ((rigBody.velocity - rigidBody.velocity).magnitude > 0) || !collision.gameObject.TryGetComponent(out Rigidbody2D r))
                    && collision.otherCollider.sharedMaterial != null)
                {
                    PlayFriction(collision.otherCollider.sharedMaterial.name[1] - 65, rigidBody.velocity.magnitude);
                }
            }
            else
            {
                if (frictionSource.loop)
                {
                    frictionSource.loop = false;
                    frictionSource.Stop();
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (frictionSource != null && frictionSource.loop)
        {
            frictionSource.loop = false;
            frictionSource.Stop();
        }
    }
    void PlayBulletImpact(int index)
    {
        source.clip = mainCamera.GetComponent<SoundController>().pack[index].bulletImpact[
            Mathf.Clamp(Random.Range(0, mainCamera.GetComponent<SoundController>().pack[index].bulletImpact.Length), 0, mainCamera.GetComponent<SoundController>().pack[index].bulletImpact.Length - 1)];
        source.volume = 0.3f;
        source.Play();
    }
    void PlayImpact(int index, float power)
    {
        source.clip = mainCamera.GetComponent<SoundController>().pack[index].softImpact[
            Mathf.Clamp(Random.Range(0, mainCamera.GetComponent<SoundController>().pack[index].softImpact.Length), 0, mainCamera.GetComponent<SoundController>().pack[index].softImpact.Length - 1)];
        source.volume = Mathf.Clamp(power / 20f, 0f, 0.3f);
        source.Play();
    }
    void PlayFriction(int index, float power)
    {
        frictionSource.clip = mainCamera.GetComponent<SoundController>().pack[index].friction[
            Mathf.Clamp(Random.Range(0, mainCamera.GetComponent<SoundController>().pack[index].friction.Length), 0, mainCamera.GetComponent<SoundController>().pack[index].friction.Length - 1)];
        frictionSource.volume = Mathf.Clamp(power / 20f, 0f, 0.3f);
        if (!frictionSource.loop)
        {
            frictionSource.Play();
            frictionSource.loop = true;
        }
    }
}


