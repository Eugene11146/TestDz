using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRopeRender : MonoBehaviour
{
    public Transform Point1;
    public Transform Point3;
    public GameObject parent1;
    public GameObject parent3;
    public LineRenderer linerenderer;
    public Vector2 localPos1;
    public Vector2 localPos3;
    public float startDist;

    private void Start()
    {
        Point1.position = (Vector2)parent1.transform.TransformPoint(localPos1);
        Point3.position = (Vector2)parent3.transform.TransformPoint(localPos3);
        startDist = Vector3.Magnitude(Point1.position - Point3.position);
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            startDist = Vector3.Magnitude(Point1.position - Point3.position);
        }
        Point1.position = (Vector2)parent1.transform.TransformPoint(localPos1);
        Point3.position = (Vector2)parent3.transform.TransformPoint(localPos3);
        linerenderer.SetPosition(0, Point1.position);
        linerenderer.SetPosition(1, Point3.position);
    }
}
