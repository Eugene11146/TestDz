using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePreviewRender : MonoBehaviour
{
    public Transform Point1;
    public Transform Point3;
    public GameObject parent1;
    public Vector2 localPos1;
    public Vector2 localPos3;
    public LineRenderer linerenderer;
    public bool anchored = false;

    private void Start()
    {
        Point1.position = parent1.transform.TransformPoint(localPos1);
        Point3.position = parent1.transform.TransformPoint(localPos3);
    }

    void Update()
    {
        if (parent1 != null)
        {
            Point1.position = parent1.transform.TransformPoint(localPos1);
            Point3.position = parent1.transform.TransformPoint(localPos3);
            linerenderer.SetPosition(0, Point1.position);
            linerenderer.SetPosition(1, (Point1.position + Point3.position) / 2);
            linerenderer.SetPosition(2, Point3.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
