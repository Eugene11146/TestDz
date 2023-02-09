using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Trap : MonoBehaviour
{
    public GameObject part1;
    public GameObject part2;
    public Animator animator;
    public Collider2D thisCollider;
    public float damage = 130f;
    public AudioClip closeSound;

    FixedJoint2D fixedJoint;
    float ticker = 2f;
    bool activated = false;
    bool trapped = false;

    void Update()
    {
        activated = -part1.transform.rotation.eulerAngles.z + transform.rotation.eulerAngles.z == 0;

        if (ticker > 0)
        {
            ticker -= Time.deltaTime;
        }

        if (fixedJoint != null && fixedJoint.connectedBody == null)
        {
            Destroy(fixedJoint);
            trapped = false;
        }
    }

    public void Active()
    {
        if (ticker <= 0)
        {
            ticker = 2f;
            animator.SetTrigger("Activate");

            if (trapped)
            {
                FixedJoint2D fix = fixedJoint;

                StartCoroutine(IgnoreCollision());
            }
        }
    }

    IEnumerator IgnoreCollision()
    {
        if (fixedJoint != null)
        {
            var collidersInChildren = fixedJoint.connectedBody.transform.root.GetComponentsInChildren<Collider2D>();
            var colliders = fixedJoint.connectedBody.GetComponents<Collider2D>();
            Destroy(fixedJoint);

            yield return new WaitForSeconds(0.4f);


            foreach (Collider2D coll in collidersInChildren)
            {
                if (coll != null)
                {
                    Physics2D.IgnoreCollision(coll, thisCollider, false);
                    Physics2D.IgnoreCollision(coll, part1.GetComponent<Collider2D>(), false);
                    Physics2D.IgnoreCollision(coll, part2.GetComponent<Collider2D>(), false);
                }
            }

            foreach (Collider2D coll in colliders)
            {
                if (coll != null)
                {
                    Physics2D.IgnoreCollision(coll, thisCollider, false);
                    Physics2D.IgnoreCollision(coll, part1.GetComponent<Collider2D>(), false);
                    Physics2D.IgnoreCollision(coll, part2.GetComponent<Collider2D>(), false);
                }
            }

            activated = true;
            trapped = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated && !trapped && collision.gameObject.layer != 7 && collision.gameObject.layer != 4
            && collision.gameObject.TryGetComponent(out Rigidbody2D rb) && !collision.attachedRigidbody.isKinematic && !collision.gameObject.TryGetComponent(out Trap tr))
        {
            GameObject emmiter = Instantiate(GlobalSetting.soundEmmiter, transform);
            emmiter.GetComponent<AudioSource>().clip = closeSound;
            emmiter.GetComponent<AudioSource>().Play();

            GetComponent<Rigidbody2D>().AddForce(transform.up * 1000);
            animator.SetTrigger("Close");

            fixedJoint = gameObject.AddComponent<FixedJoint2D>();
            fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();

            var colliders = collision.gameObject.transform.root.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D coll in colliders)
            {
                Physics2D.IgnoreCollision(coll, thisCollider);
                Physics2D.IgnoreCollision(coll, part1.GetComponent<Collider2D>());
                Physics2D.IgnoreCollision(coll, part2.GetComponent<Collider2D>());
            }
            colliders = collision.gameObject.GetComponents<Collider2D>();
            foreach (Collider2D coll in colliders)
            {
                Physics2D.IgnoreCollision(coll, thisCollider);
                Physics2D.IgnoreCollision(coll, part1.GetComponent<Collider2D>());
                Physics2D.IgnoreCollision(coll, part2.GetComponent<Collider2D>());
            }

            if (collision.TryGetComponent(out Breakable br))
            {
                br.Hit(damage);
            }

            activated = false;
            trapped = true;
        }
    }
}
