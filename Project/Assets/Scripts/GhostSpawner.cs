using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject GO;
    public Sprite ghostIcon;
    GameObject lastGO;

    public void Spawn()
    {
        Debug.Log("Ghost spawned");
        if (transform.parent.lossyScale.x > 0)
        {
            lastGO = Instantiate(GO, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
        }
        else
        {
            lastGO = Instantiate(GO, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            lastGO.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (lastGO.CompareTag("Ghost") && ghostIcon != null)
            lastGO.GetComponent<SpriteRenderer>().sprite = ghostIcon;
    }

    public void DestroyLast()
    {
        if (lastGO != null)
            Destroy(lastGO);
    }
}
