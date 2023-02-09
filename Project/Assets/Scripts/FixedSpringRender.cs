using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedSpringRender : MonoBehaviour
{
    public Transform Point1;
    public Transform Point3;
    public GameObject parent1;
    public GameObject parent3;
    public Vector2 localPos1;
    public Vector2 localPos3;
    public LineRenderer linerenderer;
    public float startDist;

    Material thisMaterial;

    private void Start()
    {
        Point1.position = (Vector2)parent1.transform.TransformPoint(localPos1);
        Point3.position = (Vector2)parent3.transform.TransformPoint(localPos3);
        startDist = Vector3.Magnitude(Point1.position - Point3.position);
        thisMaterial = Instantiate(GetComponent<LineRenderer>().material);
        GetComponent<LineRenderer>().material = thisMaterial;
        thisMaterial.mainTextureScale = new Vector2((Point1.position - Point3.position).magnitude * 10, 1);
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            startDist = Vector3.Magnitude(Point1.position - Point3.position);
        }
        Point1.position = (Vector2)parent1.transform.TransformPoint(localPos1);
        if (parent3 != null)
            Point3.position = (Vector2)parent3.transform.TransformPoint(localPos3);
        linerenderer.SetPosition(0, Point1.position);
        if (parent3 != null)
            linerenderer.SetPosition(1, Point3.position);
    }
}
