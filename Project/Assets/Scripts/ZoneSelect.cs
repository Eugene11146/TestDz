using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSelect : MonoBehaviour
{
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<SpriteRenderer>().color = Color.green;
        GameObject[] go = FindObjectsOfType<GameObject>();
        for (int i = 0; i < go.Length; ++i)
        {
            mainCamera.GetComponent<CameraMovementForPhone>().selectedObjects.Clear();
            //if (go[i].transform.position)
            mainCamera.GetComponent<CameraMovementForPhone>().selectedObjects.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
