using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollisions : MonoBehaviour
{
    void Start()
    {
        var parts = GetComponentsInChildren<Collider2D>();
        for(int i = 0; i < parts.Length - 1; ++i)
        {
            for (int j = i + 1; j < parts.Length; ++j)
            {
                Physics2D.IgnoreCollision(parts[i].GetComponent<Collider2D>(), parts[j].GetComponent<Collider2D>());
            }
        }
    }
}
