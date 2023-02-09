using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashRandomizer : MonoBehaviour
{
    public Sprite[] muzzleFlash;
    public float duration = 0.1f;
    float ticker = 0;

    void Update()
    {
        if (ticker <= 0)
            GetComponent<SpriteRenderer>().sprite = null;
        else
            ticker -= Time.deltaTime;
    }

    public void Shot()
    {
        ticker = duration;
        GetComponent<SpriteRenderer>().sprite = muzzleFlash[Random.Range(0, muzzleFlash.Length - 1)];
    }
}
