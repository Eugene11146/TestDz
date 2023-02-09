using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineDetonate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Water"))
            gameObject.SendMessage("Active");
    }
}
