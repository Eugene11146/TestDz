using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalLayoutGroupUpdate : MonoBehaviour
{
    VerticalLayoutGroup verticalLayoutGroup;

    private void Start()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    void Update()
    {
        verticalLayoutGroup.spacing = Random.Range(0f, 0.000001f);
    }
}
