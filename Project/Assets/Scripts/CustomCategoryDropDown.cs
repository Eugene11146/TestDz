using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CustomCategoryDropDown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public UnityEvent smoothScalerOpen;
    public UnityEvent smoothScalerClose;

    public void CheckCategory()
    {
        if (dropdown.options[dropdown.value].text == "Custom")
        {
            smoothScalerOpen.Invoke();
        }
        else
        {
            smoothScalerClose.Invoke();
        }
    }
}
