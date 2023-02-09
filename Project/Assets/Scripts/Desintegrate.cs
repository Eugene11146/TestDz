using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desintegrate : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (TryGetComponent(out SpriteRenderer s))
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        else
        {
            Destroy(GetComponent<Desintegrate>());
        }
    }

    private void Update()
    {
        if (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(Mathf.Clamp01(spriteRenderer.color.r - Time.deltaTime), Mathf.Clamp01(spriteRenderer.color.g - Time.deltaTime),
                Mathf.Clamp01(spriteRenderer.color.b - Time.deltaTime), Mathf.Clamp01(spriteRenderer.color.a - Time.deltaTime * 0.9f));
        }
        else
        {
            var children = transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i].CompareTag("Particle"))
                {
                    children[i].GetComponent<ParticleSystem>().Stop();
                }
                if (children[i].parent == transform)
                    children[i].SetParent(transform.parent);
            }
            Destroy(gameObject);
        }
    }
}
