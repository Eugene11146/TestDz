using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusCloser : MonoBehaviour
{
    void Update()
    {
        var menus = GameObject.FindGameObjectsWithTag("ContextMenu");
        if (menus.Length > 1)
        {
            CloseMenus(1);
        }
    }

    public void CloseMenus(float substruct)
    {
        var menus = GameObject.FindGameObjectsWithTag("ContextMenu");
        for (int i = 0; i < menus.Length - substruct; ++i)
        {
            menus[i].GetComponent<ContextMenu>().Close();
        }
    }
}
