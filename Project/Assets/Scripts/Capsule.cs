using UnityEngine;
using System.Collections;

public class Capsule : MonoBehaviour
{
    public int segments;
    public int angleAddition = -1;
    public float xradius;
    public float yradius;
    LineRenderer line;

    void Update()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        CreatePoints();
    }


    void CreatePoints()
    {
        float x = 0;
        float y = 0;
        float z = 0f;
        float yradius = this.yradius - 2.6f;
        float xradius = this.xradius - 2.6f;

        float angle = 1f;

        for (int i = 0; i < (segments + 1); i++)
        {
            if (xradius > yradius)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * (angle + angleAddition + 90)) * yradius;
                y = Mathf.Cos(Mathf.Deg2Rad * (angle + angleAddition + 90)) * yradius;
                if (i < segments / 4f || i > segments - (segments / 4f))
                {
                    x += (xradius - yradius) * 2;
                }
                x -= xradius - yradius;
            }
            else if (yradius > xradius)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * (angle + angleAddition)) * xradius;
                y = Mathf.Cos(Mathf.Deg2Rad * (angle + angleAddition)) * xradius;
                if (i < segments / 4f || i > segments - (segments / 4f))
                {
                    y += (yradius - xradius) * 2;
                }
                y -= yradius - xradius;
            }
            else
            {
                x = Mathf.Sin(Mathf.Deg2Rad * (angle + angleAddition)) * xradius;
                y = Mathf.Cos(Mathf.Deg2Rad * (angle + angleAddition)) * yradius;
            }
            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
        if (xradius > yradius)
        {
            line.SetPosition((int)Mathf.Round(segments / 4f) - 1, new Vector3(line.GetPosition((int)Mathf.Round(segments / 4f) - 1).x, -yradius, z));
            line.SetPosition((int)Mathf.Round(segments / 4f), new Vector3(line.GetPosition((int)Mathf.Round(segments / 4f)).x, -yradius, z));

            line.SetPosition(segments - ((int)Mathf.Round(segments / 4f) - 1), new Vector3(line.GetPosition(segments - ((int)Mathf.Round(segments / 4f) - 1)).x, yradius, z));
            line.SetPosition(segments - ((int)Mathf.Round(segments / 4f)), new Vector3(line.GetPosition(segments - ((int)Mathf.Round(segments / 4f))).x, yradius, z));
        }
        else if (yradius >= xradius)
        {
            line.SetPosition((int)Mathf.Round(segments / 4f) - 1, new Vector3(xradius, line.GetPosition((int)Mathf.Round(segments / 4f) - 1).y, z));
            line.SetPosition((int)Mathf.Round(segments / 4f), new Vector3(xradius, line.GetPosition((int)Mathf.Round(segments / 4f)).y, z));

            line.SetPosition(segments - ((int)Mathf.Round(segments / 4f) - 1), new Vector3(-xradius, line.GetPosition(segments - ((int)Mathf.Round(segments / 4f) - 1)).y, z));
            line.SetPosition(segments - ((int)Mathf.Round(segments / 4f)), new Vector3(-xradius, line.GetPosition(segments - ((int)Mathf.Round(segments / 4f))).y, z));
        }
    }
}