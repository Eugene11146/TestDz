using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPreview : MonoBehaviour
{
    public Vector3 color;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        color = new Vector3(redSlider.value, greenSlider.value, blueSlider.value);
        image.color = new Color(color.x / 255f, color.y / 255f, color.z / 255f, 1);
    }
}
