using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileCategoryFromType : MonoBehaviour
{
    public List<TypeCategories> typeCategories;
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown typeDropdown;

    bool wasSet = false;

    private void Start()
    {
        if (!wasSet)
        {
            Set();
        }
    }

    public void Set()
    {
        categoryDropdown.ClearOptions();
        categoryDropdown.AddOptions(typeCategories[typeDropdown.value].categories);
        wasSet = true;
    }
    public void Set(int value)
    {
        categoryDropdown.ClearOptions();
        categoryDropdown.AddOptions(typeCategories[typeDropdown.value].categories);
        categoryDropdown.value = value;
        wasSet = true;
    }
}

[System.Serializable]
public struct TypeCategories
{
    public string name;
    public List<string> categories;
}

