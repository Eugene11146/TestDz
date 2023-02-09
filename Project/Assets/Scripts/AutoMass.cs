using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMass : MonoBehaviour
{
    public float mass;
    public float unFloatingMass;
    public float floatingMass;
    public float massAddition;
    Rigidbody2D rigidBody;

    //*5
    float woodMass = 12.8f;
    float metalMass = 31.2f;
    float fleshMass = 1;
    float weaponWoodMass = 13.2f;
    float fruitMass = 13.2f;
    float dishesMass = 18f;
    float rubberMass = 16.8f;
    float paperMass = 13.6f;
    float glassMass = 125f;
    float clothMass = 10.75f;
    float concreteMass = 25f;

    void Start()
    {
        CalculateMass();
    }

    Vector3 Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public void CalculateMass()
    {
        float offsetSubstructor = 0.0191855f;

        floatingMass = 0;
        unFloatingMass = 0;
        mass = 0;
        rigidBody = GetComponent<Rigidbody2D>();
        BoxCollider2D[] boxColliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D coll in boxColliders)
        {
            Vector2 colliderSize = coll.size + new Vector2(offsetSubstructor / Abs(transform.lossyScale).x, offsetSubstructor / Abs(transform.lossyScale).y);
            mass += getMassByMaterial(coll, colliderSize.x * colliderSize.y);
        }

        CircleCollider2D[] circleColliders = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D coll in circleColliders)
        {
            mass += getMassByMaterial(coll, Mathf.Pow(coll.radius, 2) * Mathf.PI);
        }

        CapsuleCollider2D[] capsuleColliders = GetComponents<CapsuleCollider2D>();
        foreach (CapsuleCollider2D coll in capsuleColliders)
        {
            if (coll.direction == CapsuleDirection2D.Horizontal)
                mass += getMassByMaterial(coll, (coll.size.x - coll.size.y) * coll.size.y + Mathf.Pow(coll.size.y / 2, 2) * Mathf.PI);
            else
                mass += getMassByMaterial(coll, (coll.size.y - coll.size.x) * coll.size.x + Mathf.Pow(coll.size.x/ 2, 2) * Mathf.PI);
        }

        PolygonCollider2D[] polygonColliders = GetComponents<PolygonCollider2D>();
        foreach (PolygonCollider2D coll in polygonColliders)
        {
            int n = coll.points.Length;
            float res = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    res += coll.points[i].x * (coll.points[n - 1].y - coll.points[i + 1].y);
                }
                else
                  if (i == n - 1)
                {
                    res += coll.points[i].x * (coll.points[i - 1].y - coll.points[0].y);
                }
                else
                {
                    res += coll.points[i].x * (coll.points[i - 1].y - coll.points[i + 1].y);
                }
            }
            mass += getMassByMaterial(coll, Mathf.Abs(res / 2));
        }

        if (rigidBody != null)
            rigidBody.mass = Mathf.Abs(mass * rigidBody.transform.localScale.x * rigidBody.transform.localScale.y) + massAddition;
    }

    float getMassByMaterial(Collider2D coll, float thisMass)
    {
        if (coll.sharedMaterial != null)
        {
            switch (coll.sharedMaterial.name[0])
            {
                case 'D': //Wood
                    thisMass *= woodMass;
                    floatingMass += thisMass;
                    break;
                case '8': //Metal
                    thisMass *= metalMass;
                    unFloatingMass += thisMass;
                    break;
                case '3': //Flesh
                    thisMass *= fleshMass;
                    floatingMass += thisMass;
                    break;
                case 'C': //Weapon wood
                    thisMass *= weaponWoodMass;
                    floatingMass = thisMass;
                    break;
                case '4': //Fruit
                    thisMass *= fruitMass;
                    floatingMass = thisMass;
                    break;
                case '2': //Dishes
                    thisMass *= dishesMass;
                    unFloatingMass += thisMass;
                    break;
                case 'A': //Rubber
                    thisMass *= rubberMass;
                    unFloatingMass += thisMass;
                    break;
                case '9': //Paper
                    thisMass *= paperMass;
                    floatingMass = thisMass;
                    break;
                case '5': //Glass
                    thisMass *= glassMass;
                    floatingMass = thisMass;
                    break;
                case '7': //Kevlar
                    thisMass *= clothMass;
                    floatingMass = thisMass;
                    break;
                case '1': //Cloth
                    thisMass *= clothMass;
                    floatingMass = thisMass;
                    break;
                case 'F': //Concrete
                    thisMass *= concreteMass;
                    floatingMass = thisMass;
                    break;
            }
        }
        return thisMass;
    }
}
