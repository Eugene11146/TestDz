using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    public void StopTimeF()
    {
        Time.timeScale = 0;
    }
    public void StartTime()
    {
        Time.timeScale = 1;
    }
}
