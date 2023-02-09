using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : MonoBehaviour
{
    public GameObject blade;
    public Material bladeMaterial;
    public Collider2D bladeCollider;
    public ContactFilter2D filter;

    SpriteRenderer spriteRenderer;
    Material loaclBladeMaterial;
    Properties properties;
    bool isActive = false;

    private void Start()
    {
        spriteRenderer = blade.GetComponent<SpriteRenderer>();
        loaclBladeMaterial = Instantiate(bladeMaterial);
        spriteRenderer.material = loaclBladeMaterial;
        properties = GetComponent<Properties>();
    }

    private void Update()
    {
        blade.SetActive(isActive);
        loaclBladeMaterial.SetFloat("_Speed", Mathf.Clamp01(properties.velocity.magnitude * 5f));
        Collider2D[] colliders = new Collider2D[0];
        Physics2D.OverlapCollider(bladeCollider, filter, colliders);

        foreach (Collider2D collider in colliders)
        {

        }
    }

    public void Active()
    {
        isActive = true;
    }
}
