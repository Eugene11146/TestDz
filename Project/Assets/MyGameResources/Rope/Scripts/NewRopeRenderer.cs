using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Рендер для отрисовки  веревки
/// </summary>
public class NewRopeRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer linerenderer;

    [SerializeField]
    private Vector2 localPos1;

    [SerializeField]
    private Vector2 localPos3;

    private RopeNode ropeNode;

    private Vector3 point1;

    private Vector3 point3;

    private float startDist;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            startDist = Vector3.Magnitude(point1 - point3);
        }

        if (!ropeNode)
        {
            ropeNode = GetComponent<RopeNode>();
        }

        point1 = (Vector2)gameObject.transform.TransformPoint(localPos1);
        point3 = (Vector2)ropeNode.RightBond.transform.TransformPoint(localPos3);
        linerenderer.SetPosition(0, point1);
        linerenderer.SetPosition(1, point3);
    }
}
   
