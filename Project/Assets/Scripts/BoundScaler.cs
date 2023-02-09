using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundScaler : MonoBehaviour
{
    private void Start()
    {
        float offsetSubstructor = 0.01f;
        BoxCollider2D[] boxColliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D boxCollider in boxColliders)
        {
            boxCollider.size = new Vector2(Mathf.Clamp(boxCollider.size.x - offsetSubstructor / transform.lossyScale.x, 0.003f, boxCollider.size.x),
                                            Mathf.Clamp(boxCollider.size.y - offsetSubstructor / transform.lossyScale.y, 0.003f, boxCollider.size.y));
        }
    }
}
