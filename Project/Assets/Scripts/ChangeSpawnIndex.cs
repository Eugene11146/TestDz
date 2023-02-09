using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpawnIndex : MonoBehaviour
{
    public int index;
    public GameObject menu;

    public void ChangeIndex()
    {
        menu.GetComponent<SpawnMenuSorting>().index = index;
    }
}
