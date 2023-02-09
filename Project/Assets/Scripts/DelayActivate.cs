using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActivate : MonoBehaviour
{
    public GameObject obj;
    public float delay;
    public bool repeat = false;

    private void Start()
    {
        if (!repeat)
            StartCoroutine(DelayActive(delay));
        else
            InvokeRepeating("Active", delay, delay);
    }

    void Active()
    {
        obj.SendMessage("Active");
    }

    IEnumerator DelayActive(float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SendMessage("Active");
    }
}
