using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BladeScript : MonoBehaviour
{
    public List<Rigidbody2D> hitObjects;
    public Transform hitPos;
    public Collider2D[] blades;
    public Collider2D trigger;
    public SoundController soundController;
    public AudioSource source;
    public Vector2 velocity;
    public float damage = 100;
    public float rotation = 0;
    public Vector2 lastPos;


    private void Start()
    {
        soundController = Camera.main.GetComponent<SoundController>();
    }

    private void Update()
    { 
        source.pitch = Time.timeScale;
        lastPos = hitPos.position;
    }

    private void FixedUpdate()
    {
        velocity = lastPos - (Vector2)hitPos.position;
        var fritions = GetComponents<FrictionJoint2D>();
        for (int i = 0; i < fritions.Length; ++i)
        {
            if (fritions[i].connectedBody == null)
            {
                Destroy(fritions[i]);
            }
        }
        var sliders = GetComponents<SliderJoint2D>();
        foreach(SliderJoint2D slider in sliders)
        {
            if (slider.connectedBody == null && hitObjects.Count > 0)
            {
                hitObjects.RemoveAt(hitObjects.FindIndex(x => x == null));
                Destroy(slider);
            }
        }
    }

    public void AddSlider(Collider2D collision)
    {
        bool b = true;
        if (collision.CompareTag("Ragdoll"))
        {
            for (int i = 0; i < hitObjects.Count; ++i)
            {
                if (collision == hitObjects[i])
                {
                    b = false;
                    break;
                }
            }
        }
        if (b)
        {
            if (collision.sharedMaterial != null)
            {
                if (collision.sharedMaterial.name[0] == 'D' /*Wood*/ || collision.sharedMaterial.name[0] == '3' /*Flesh*/ || collision.sharedMaterial.name[0] == '4' /*Fruit*/)
                {
                    SliderJoint2D slider;
                    FrictionJoint2D friction;
                    if (transform.parent != null && transform.root.TryGetComponent(out Rigidbody2D r))
                    {
                        GameObject main = transform.root.gameObject;
                        hitObjects.Add(collision.GetComponent<Rigidbody2D>());
                        slider = main.AddComponent<SliderJoint2D>();
                        friction = main.AddComponent<FrictionJoint2D>();
                    }
                    else
                    {
                        hitObjects.Add(collision.GetComponent<Rigidbody2D>());
                        slider = gameObject.AddComponent<SliderJoint2D>();
                        friction = gameObject.AddComponent<FrictionJoint2D>();
                    }
                    friction.maxForce = 5;
                    friction.enableCollision = true;
                    friction.connectedBody = collision.GetComponent<Rigidbody2D>();
                    slider.connectedBody = collision.GetComponent<Rigidbody2D>();
                    slider.autoConfigureAngle = false;
                    slider.angle = -90 + rotation;
                    slider.enableCollision = true;

                    Vector2 dir = new Vector2(Mathf.Cos((90 + rotation) * Mathf.Deg2Rad), Mathf.Sin((90 + rotation) * Mathf.Deg2Rad));
                    slider.connectedAnchor = collision.transform.InverseTransformPoint(transform.TransformPoint(dir));
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D r))
        {
            if (hitObjects.Find(x => x == r))
            {
                foreach (Collider2D blade in blades)
                    Physics2D.IgnoreCollision(blade, collision.collider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 dir = new Vector2(Mathf.Cos((hitPos.rotation.eulerAngles.z + 90 + rotation) * Mathf.Deg2Rad), Mathf.Sin((hitPos.rotation.eulerAngles.z + 90 + rotation) * Mathf.Deg2Rad));
        collision.TryGetComponent(out Rigidbody2D collisionRigidbody);
        TryGetComponent(out Rigidbody2D localRigidbody);
        if (Vector3.Dot(dir, Vector3.Normalize(localRigidbody.velocity)) > 0.4f || (collisionRigidbody != null && -Vector3.Dot(dir, Vector3.Normalize(collisionRigidbody.velocity)) > 0.4f) 
            && ((trigger != null && collision.GetComponent<Collider2D>() == trigger) || trigger == null))
        {
            if (collision != null && !collision.CompareTag("Bound"))
            {
                if (hitObjects.Count > 0)
                {
                    if (!hitObjects.Find(x => x == collision.GetComponent<Rigidbody2D>()))
                    {
                        if (collision.sharedMaterial != null && 
                            (collision.sharedMaterial.name[0] == 'D' /*Wood*/ || collision.sharedMaterial.name[0] == '3' /*Flesh*/ || collision.sharedMaterial.name[0] == '4' /*Fruit*/ || collision.sharedMaterial.name[0] == '1' /*Cloth*/))
                            source.PlayOneShot(soundController.stabPack[Random.Range(0, soundController.stabPack.Length)]);
                        AddSlider(collision);
                        if (collision.gameObject.TryGetComponent(out Rigidbody2D otherRigidbody))
                        {
                            otherRigidbody.AddForce(dir * -200);
                        }
                        if (TryGetComponent(out Rigidbody2D rigidbody))
                        {
                            rigidbody.AddForce(dir * 200);
                        }
                    }
                }
                else
                {
                    if (collision.sharedMaterial != null && 
                        (collision.sharedMaterial.name[0] == 'D' /*Wood*/ || collision.sharedMaterial.name[0] == '3' /*Flesh*/ || collision.sharedMaterial.name[0] == '4' /*Fruit*/ || collision.sharedMaterial.name[0] == '1' /*Cloth*/))
                        source.PlayOneShot(soundController.stabPack[Random.Range(0, soundController.stabPack.Length)]);
                    AddSlider(collision);
                    if (collision.gameObject.TryGetComponent(out Rigidbody2D otherRigidbody))
                    {
                        otherRigidbody.AddForce(dir * -200);
                    }
                    if (TryGetComponent(out Rigidbody2D rigidbody))
                    {
                        rigidbody.AddForce(dir * 200);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hitObjects.Count > 0)
        {
            if (hitObjects.Find(x => x == collision.GetComponent<Rigidbody2D>()))
            {
                var fritions = GetComponents<FrictionJoint2D>();
                if (transform.parent == null)
                    fritions = GetComponents<FrictionJoint2D>();
                else
                    fritions = transform.parent.GetComponents<FrictionJoint2D>();
                for (int i = 0; i < fritions.Length; ++i)
                {
                    if (fritions[i].connectedBody == collision.GetComponent<Rigidbody2D>())
                    {
                        Destroy(fritions[i]);
                        break;

                    }
                }
                var sliders = GetComponents<SliderJoint2D>();
                if (transform.parent == null)
                {
                    sliders = GetComponents<SliderJoint2D>();
                }
                else
                {
                    sliders = transform.parent.GetComponents<SliderJoint2D>();
                }
                for (int i = 0; i < sliders.Length; ++i)
                {
                    if (sliders[i].connectedBody == collision.GetComponent<Rigidbody2D>())
                    {
                        if (collision.CompareTag("Ragdoll"))
                        {
                            var parts = collision.transform.root.GetComponentsInChildren<Collider2D>();
                            for (int j = 0; j < parts.Length - 1; ++j)
                            {
                                foreach (Collider2D blade in blades)
                                    Physics2D.IgnoreCollision(blade, parts[j], false);
                            }
                        }
                        foreach (Collider2D blade in blades)
                            Physics2D.IgnoreCollision(blade, collision.GetComponent<Collider2D>(), false);
                        hitObjects.RemoveAt(hitObjects.FindIndex(x => x == collision.GetComponent<Rigidbody2D>()));
                        Destroy(sliders[i]);
                        break;

                    }
                }
            }
        }
    }
}
