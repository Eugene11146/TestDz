using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintSelect : MonoBehaviour
{
    public int ID = 0;
    public GameObject main;
    ConstraintController selectedID;

    private void Start()
    {
        selectedID = main.GetComponent<ConstraintController>();
    }

    public void Select()
    {
        selectedID.selectedID = ID;
        selectedID.GetComponent<UISmoothSlide>().Close();
    }
}
