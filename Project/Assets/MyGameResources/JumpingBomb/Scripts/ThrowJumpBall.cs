using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Простой класс кидающий бомбу по нажатию на пробел
/// </summary>
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
            Instantiate(bomb, startPoint.position, startPoint.rotation).AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}
