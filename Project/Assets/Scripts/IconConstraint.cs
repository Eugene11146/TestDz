using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconConstraint : MonoBehaviour
{
    public ConstraintController controller;
    public Sprite[] icons;
    Image render;
    
    private void Start()
    {
        render = GetComponent<Image>();
    }

    private void Update()
    {
        render.sprite = icons[controller.selectedID];
    }
}
