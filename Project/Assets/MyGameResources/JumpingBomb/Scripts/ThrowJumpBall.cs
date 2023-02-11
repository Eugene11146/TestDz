using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowJumpBall : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Rigidbody2D bomb;

    [SerializeField]
    private Vector2 forceVector;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space;");
            Instantiate(bomb, startPoint.position, startPoint.rotation).AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}
