using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void GameExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
