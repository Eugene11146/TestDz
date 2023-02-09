using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticRender : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public GameObject parent1;
    public GameObject parent3;
    public Vector2 localPos1;
    public Vector2 localPos3;
    public LineRenderer linerenderer;
    public float vertexCount = 12;
    public float Point2Yposition = 2;
    Vector3 lastPos1;
    Vector3 lastPos3;
    Vector3 targetPos;
    bool anchored = false;
    float lastDist;
    public float startDist;

    private void Start()
    {
        Point1.position = parent1.transform.TransformPoint(localPos1);
        Point3.position = Point1.position;
        if (parent3 != null)
            targetPos = parent3.transform.TransformPoint(localPos3);
        else
            targetPos = localPos3;
        startDist = Vector3.Magnitude(Point1.position - targetPos);
    }

    void Update()
    {
        if (Time.timeScale == 0 && (Vector3.Magnitude(lastPos1 - parent1.transform.position) > 0 || (parent3 != null && Vector3.Magnitude(lastPos3 - parent3.transform.position) > 0)))
        {
            startDist = Vector3.Magnitude(Point1.position - Point3.position);
        }
        if (Point1 != null && parent1 != null)
            Point1.position = parent1.transform.TransformPoint(localPos1);
        if (!anchored)
        {
            Point3.position = Vector3.Lerp(Point3.position, targetPos, 6 * Time.fixedUnscaledDeltaTime);
            if (parent3 != null)
                targetPos = parent3.transform.TransformPoint(localPos3);
            else
                targetPos = localPos3;
            if (Vector3.Magnitude(Point3.position - targetPos) < 0.001f)
                anchored = true;
        }
        else
        {
            if (parent3 != null)
                Point3.position = parent3.transform.TransformPoint(localPos3);
            else
                Point3.position = localPos3;
        }
        if ((Time.timeScale == 0 && Mathf.Abs(lastDist - startDist) > 0) || Vector3.Magnitude(Point1.position - Point3.position) > startDist)
            Point2.transform.position = new Vector3((Point1.position.x + Point3.position.x) / 2, (Point1.position.y + Point3.position.y) / 2);
        else
            Point2.transform.position = new Vector3((Point1.position.x + Point3.position.x) / 2,
                (Point1.position.y + Point3.position.y) / 2 - Mathf.Sqrt(Mathf.Abs(Mathf.Pow(startDist, 2) - Mathf.Pow(Vector3.Magnitude(Point1.position - Point3.position), 2))));
        var pointList = new List<Vector3>();

        for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {
            var tangent1 = Vector2.Lerp(Point1.position, Point2.position, ratio);
            var tangent2 = Vector2.Lerp(Point2.position, Point3.position, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);

            pointList.Add(curve);
        }

        linerenderer.positionCount = pointList.Count;
        linerenderer.SetPositions(pointList.ToArray());
        lastPos1 = parent1.transform.position;
        if (parent3 != null)
            lastPos3 = parent3.transform.position;
        if (Time.timeScale > 0)
            lastDist = startDist;
    }
}
