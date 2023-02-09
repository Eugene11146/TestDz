using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float penetration;
    public float recoil;
    public float shootingDelay = 1;
    public float spread = 0;
    public float temperatureAddition = 0;
    public int bulletsPerShot = 1;
    public bool isShooting = false;
    public bool throwShell = false;
    public bool toggleActivation = false;
    public GameObject bullet;
    public GameObject particleSys;
    public GameObject muzzle;
    public Animator shootingAnimator;
    public AudioSource shootSound;
    public Transform bulletTrowerPos;
    public Transform particle;
    public Transform muzzleFlashTransform;
    public delegate void ShootDelegate();
    public event ShootDelegate onShoot;

    float ticker = 0;
    bool canShoot = true;

    private void Update()
    {
        if (shootSound != null)
        {
            shootSound.pitch = Time.timeScale;
        }
        if (isShooting || toggleActivation)
        {
            if (ticker <= 0)
            {
                Shoot();
            }
        }
        if (ticker > 0)
            ticker -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            canShoot = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            canShoot = true;
        }
    }

    void Shoot()
    {
        if (canShoot)
        {
            onShoot?.Invoke();

            GetComponent<Properties>().temperature += temperatureAddition;
            if (transform.lossyScale.x < 0)
            {
                if (muzzle != null)
                    Instantiate(muzzle, muzzleFlashTransform.position, Quaternion.Euler(muzzleFlashTransform.eulerAngles + new Vector3(0, 0, 180f)));
            }
            else
            {
                if (muzzle != null)
                    Instantiate(muzzle, muzzleFlashTransform.position, muzzleFlashTransform.rotation);
            }
            if (shootingAnimator != null)
                shootingAnimator.SetBool("Shoot", true);
            if (shootSound != null)
                shootSound.PlayOneShot(shootSound.clip);
            if (TryGetComponent(out Rigidbody2D r) && !GetComponent<Properties>().levitate)
            {
                r.AddForceAtPosition(transform.right * recoil * transform.lossyScale.x, bulletTrowerPos.position);
            }
            if (throwShell)
            {
                ThrowShell();
            }
            for (int i = 0; i < bulletsPerShot; ++i)
            {
                GameObject thisBullet = Instantiate(bullet, bulletTrowerPos.position, new Quaternion(0f, 0f, 0f, 0f));

                var colliders = GetComponents<Collider2D>();
                foreach (Collider2D coll in colliders)
                {
                    Physics2D.IgnoreCollision(thisBullet.GetComponent<Collider2D>(), coll);
                }

                if (thisBullet.TryGetComponent(out Bullet bulletScript))
                {
                    bulletScript.damage = damage;
                    bulletScript.penetration = penetration;
                    Physics2D.IgnoreCollision(thisBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    if (transform.lossyScale.x < 0)
                    {
                        bulletScript.angle = transform.eulerAngles.z + 180 + Random.Range(-spread, spread);
                    }
                    else
                    {
                        bulletScript.angle = transform.eulerAngles.z + Random.Range(-spread, spread);
                    }
                }
                else if (thisBullet.TryGetComponent(out Laser laserScript))
                {
                    laserScript.damage = damage;
                    Physics2D.IgnoreCollision(thisBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    if (transform.lossyScale.x < 0)
                    {
                        laserScript.angle = transform.eulerAngles.z + 180 + Random.Range(-spread, spread);
                    }
                    else
                    {
                        laserScript.angle = transform.eulerAngles.z + Random.Range(-spread, spread);
                    }
                }
            }
            ticker = shootingDelay;
        }
    }
    public void ThrowShell()
    {
        if (particleSys != null)
        {
            if (transform.lossyScale.x < 0)
            {
                Instantiate(particleSys, particle.position, Quaternion.Euler(particle.eulerAngles + new Vector3(0, 0, 180f)));
            }
            else
            {
                Instantiate(particleSys, particle.position, particle.rotation);
            }
        }
    }
    public void StartShooting()
    {
        isShooting = true;
    }
    public void StopShooting()
    {
        isShooting = false;
    }
    public void Active()
    {
        if (shootingAnimator == null || (shootingAnimator != null && !shootingAnimator.GetBool("Shoot")))
        {
            Shoot();
        }
    }
}
