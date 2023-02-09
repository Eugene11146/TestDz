using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
    public GameObject infoTutorial;
    public bool firstChild = false;

    public void Open()
    {
        if (firstChild)
            Instantiate(infoTutorial, GlobalSetting.mainCamera.transform.GetChild(0));
        else
            Instantiate(infoTutorial, GlobalSetting.mainCamera.transform.GetChild(0).GetChild(0));
    }
}
