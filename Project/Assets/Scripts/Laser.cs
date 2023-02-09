using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 40;
    public float angle = 0;
    public float speed = 10f;
    public float maxAngle = 95;
    public GameObject HitEffectSpark;
    Rigidbody2D rigbody;

    private void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rigbody.velocity = new Vector2(Mathf.Cos(angle * 0.0175f), Mathf.Sin(angle * 0.0175f)) * speed;
        if (speed < 0.3f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }

    void ImpactEffect(Collision2D collision, bool reversed)
    {
        if (reversed)
            angle += 180;
        if (collision.collider.sharedMaterial != null)
        {
            Vector2 pos;
            if (reversed)
            {
                pos = collision.collider.ClosestPoint(transform.position + transform.right);
                transform.position = pos;
            }
            else
            {
                pos = collision.collider.ClosestPoint(transform.position);
            }
            Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));
            if (collision.collider.sharedMaterial.name[0] == '3' /*Flesh*/)
            {
                Destroy(gameObject);
            }
            Instantiate(HitEffectSpark, transform.position, new Quaternion());
            Destroy(gameObject);
        }
        if (reversed)
            angle -= 180;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.sharedMaterial != null && !collision.transform.CompareTag("Bullet"))
        {
            if (collision.collider.CompareTag("Breakable"))
            {
                if (TryGetComponent(out Breakable br))
                    br.Hit(damage);
            }
            ImpactEffect(collision, false);
        }
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    }
}
