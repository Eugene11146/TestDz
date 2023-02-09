using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSettingsWindow : MonoBehaviour
{
    public List<GameObject> windows = new List<GameObject>();

    public void ChangeWindow(int index)
    {
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
        windows[index].SetActive(true);
    }
}
