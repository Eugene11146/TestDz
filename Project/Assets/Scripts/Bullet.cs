using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 40;
    public float penetration = 10f;
    public float angle = 0;
    public float speed = 10f;
    public float maxAngle = 95;
    public GameObject MetalSpark;
    public GameObject WoodGib;
    public GameObject HitSmoke;
    public GameObject DefaultHit;
    public ParticleSystem particle;
    float thisPenetration;
    Vector2 lastPos;
    Rigidbody2D rigbody;

    private void Start()
    {
        thisPenetration = penetration / 20;
        rigbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rigbody.velocity = new Vector2(Mathf.Cos(angle * 0.0175f), Mathf.Sin(angle * 0.0175f)) * speed;
        if (speed < 0.3f)
            Destroy(gameObject);
    }

    IEnumerator effectDelay()
    {
        yield return new WaitForSeconds(0.005f);

        if (particle != null)
            particle.Play();
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            StartCoroutine(effectDelay());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (speed > 0)
                speed -= speed * 150f / penetration * Time.deltaTime;
            else
                speed = 0;
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
            switch (collision.collider.sharedMaterial.name[0])
            {
                case '6': //Ground
                    Destroy(gameObject);
                    break;
                case '3': //Flesh
                    break;
                case '8': //Metal
                    Instantiate(MetalSpark, pos, rot);
                    break;
                case 'D': //Wood
                    Instantiate(WoodGib, pos, rot);
                    break;
                case 'C': //Weapon wood
                    Instantiate(WoodGib, pos, rot);
                    break;
                case '2': //Dishes
                    break;
                case '4': //Fruit
                    break;
                case '9': //Paper
                    break;
                case 'F': //Concrete
                    break;
                case 'A': //Rubber
                    Instantiate(HitSmoke, pos, rot);
                    break;
                default:
                    GameObject particle = Instantiate(DefaultHit, pos, rot);
                    particle.transform.localScale = transform.lossyScale;
                    var shape = particle.GetComponent<ParticleSystem>().shape;
                    shape.texture = collision.collider.GetComponent<SpriteRenderer>().sprite.texture;
                    shape.sprite = collision.collider.GetComponent<SpriteRenderer>().sprite;
                    particle.GetComponent<ParticleSystem>().Play();
                    break;
            }
        }
        if (reversed)
            angle -= 180;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        lastPos = collision.GetContact(collision.contactCount - 1).point;
        if (collision.collider.sharedMaterial != null && !collision.transform.CompareTag("Bullet"))
        {
            ImpactEffect(collision, false);
            if (collision.collider.sharedMaterial != null)
                hitPenetration(collision.collider.sharedMaterial.name, 0.005f);
            if (collision.collider.TryGetComponent(out Breakable br))
            {
                br.Hit(damage);
            }
        }
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.sharedMaterial != null)
        {
            hitPenetration(collision.collider.sharedMaterial.name, (lastPos - collision.collider.ClosestPoint(transform.position)).magnitude);
            if (thisPenetration > 0)
                ImpactEffect(collision, true);
        }
        else
        {
            if (!collision.transform.CompareTag("Bullet"))
                Destroy(gameObject);
        }
    }

    void hitPenetration(string s, float dist)
    {
        float p = thisPenetration;
        switch (s[0])
        {
            case '6': //Ground
                Destroy(gameObject);
                break;
            case '3': //Flesh
                thisPenetration -= dist * 12;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '8': //Metal
                thisPenetration -= dist * 180f;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case 'D': //Wood
                thisPenetration -= dist * 30f;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case 'C': //Weapon wood
                thisPenetration -= dist * 35f;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '4': //Fruit
                thisPenetration -= dist;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '2': //Dishes
                thisPenetration -= dist * 15;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case 'A': //Rubber
                thisPenetration -= dist * 18;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '9': //Paper
                thisPenetration -= dist * 32;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '1': //Cloth
                thisPenetration -= dist * 10;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '7': //Kevlar
                thisPenetration -= dist * 100;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case '5': //Glass
                thisPenetration -= dist * 45;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            case 'F': //Concrete
                thisPenetration -= dist * 145;
                if (thisPenetration <= 0)
                    Destroy(gameObject);
                break;
            default:
                Debug.Log("No material");
                Destroy(gameObject);
                break;
        }
    }
}
