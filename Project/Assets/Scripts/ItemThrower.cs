using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemThrower : MonoBehaviour
{
    public Animator shootingAnimator;
    public GameObject obj;
    public Transform objThrower;
    public AudioSource shootSound;
    public float rotationAddition = 90f;
    public float force = 100f;
    public float shootingDelay = 1f;
    public float shootingSpread = 0;
    public bool isShooting = false;
    public bool toggleActivation = false;
    float ticker = 0;

    private void Update()
    {
        if (shootSound != null)
            shootSound.pitch = Time.timeScale;
        if (isShooting || toggleActivation)
        {
            if (shootingDelay != 0)
            {
                if (ticker <= 0)
                {
                    Active();
                }
                ticker -= Time.deltaTime;
            }
            else
            {
                Activate();
            }
        }
    }

    public void Active()
    {
        if (shootingAnimator != null)
        {
            if (!shootingAnimator.GetBool("Shoot"))
            {
                ticker = shootingDelay;
                shootingAnimator.SetBool("Shoot", true);
                Shot();
            }
        }
        else
        {
            Shot();
        }
    }

    public void Shot()
    {
        GameObject thisObj = Instantiate(obj, objThrower.position, objThrower.rotation);
        if (transform.lossyScale.x < 0)
            thisObj.transform.eulerAngles = objThrower.eulerAngles + new Vector3(0, 0, 180);
        if (shootSound != null)
            shootSound.PlayOneShot(shootSound.clip);
        Rigidbody2D rigbody = thisObj.GetComponent<Rigidbody2D>();
        var colliders = thisObj.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
        }
        colliders = thisObj.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
        }
        Vector3 shotDir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (objThrower.eulerAngles.z + rotationAddition + Random.Range(-shootingSpread, shootingSpread))),
                                            Mathf.Sin(Mathf.Deg2Rad * (objThrower.eulerAngles.z + rotationAddition + Random.Range(-shootingSpread, shootingSpread))), 0) * Mathf.Sign(transform.lossyScale.x);
        rigbody.AddForce(shotDir * force);
    }

    public void Activate()
    {
        shootingAnimator.SetBool("Shoot", true);
    }
}
