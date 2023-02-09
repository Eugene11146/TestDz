using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyContainerRemover : MonoBehaviour
{
    void Update()
    {
        if (gameObject.transform.name == "CopyContainer(Clone)")
        {
            GlobalSetting.spawnMenu.Paste(gameObject);
        }
    }
}
