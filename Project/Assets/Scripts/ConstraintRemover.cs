using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintRemover : MonoBehaviour
{
    public int index = 0;
    public GameObject connectedBody;
    public DistanceJoint2D distanceJoint2D;
    public FixedJoint2D fixedJoint2D;
    public SpringJoint2D springJoint2D;
    public HingeJoint2D hingeJoint2D;
    public Vector3 constraintLocalPos;
    public float dist;

    private void Start()
    {
        if (constraintLocalPos != new Vector3(0, 0, 0))
        {
            if (distanceJoint2D != null)
            {
                distanceJoint2D.connectedAnchor = distanceJoint2D.connectedBody.transform.InverseTransformPoint(distanceJoint2D.transform.TransformPoint(constraintLocalPos));
                if (TryGetComponent(out RopeRender rope))
                {
                    rope.localPos3 = distanceJoint2D.connectedAnchor;
                    rope.startDist = dist;
                }
                if (TryGetComponent(out FixedRopeRender fixRope))
                {
                    fixRope.localPos3 = distanceJoint2D.connectedAnchor;
                }
            }
            if (fixedJoint2D != null)
            {
                if (TryGetComponent(out FixedRopeRender fixRope))
                {
                    if (fixedJoint2D.connectedBody.gameObject != GlobalSetting.defaultConstraintBody)
                    {
                        fixRope.localPos3 = constraintLocalPos;
                    }
                    else
                    {
                        fixRope.localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(fixedJoint2D.transform.TransformPoint(constraintLocalPos));
                    }
                }
                fixedJoint2D.enableCollision = false;
            }
            if (springJoint2D != null)
            {
                springJoint2D.connectedAnchor = springJoint2D.connectedBody.transform.InverseTransformPoint(springJoint2D.transform.TransformPoint(constraintLocalPos));
                if (TryGetComponent(out FixedSpringRender springRope))
                {
                    springRope.localPos3 = springJoint2D.connectedBody.transform.InverseTransformPoint(springJoint2D.transform.TransformPoint(constraintLocalPos));
                    springRope.startDist = dist;
                }
                springJoint2D.enableCollision = false;
            }
        }

        if (distanceJoint2D != null)
        {
            distanceJoint2D.enableCollision = false;
            connectedBody = distanceJoint2D.connectedBody.gameObject;
        }
        if (fixedJoint2D != null)
        {
            fixedJoint2D.enableCollision = false;
            connectedBody = fixedJoint2D.connectedBody.gameObject;
        }
        if (springJoint2D != null)
        {
            springJoint2D.enableCollision = false;
            connectedBody = springJoint2D.connectedBody.gameObject;
        }
        if (hingeJoint2D != null)
        {
            connectedBody = hingeJoint2D.connectedBody.gameObject;
        }
    }

    private void Update()
    {
        switch (index)
        {
            case 0:
                if (distanceJoint2D == null)
                    Destroy(gameObject);
                else
                {
                    if (distanceJoint2D.connectedBody == null)
                        Destroy(gameObject);
                    else
                    {
                        constraintLocalPos = distanceJoint2D.transform.InverseTransformPoint(distanceJoint2D.connectedBody.transform.TransformPoint(distanceJoint2D.connectedAnchor));
                        dist = distanceJoint2D.distance;
                        distanceJoint2D.enableCollision = true;
                        if (TryGetComponent(out RopeRender r))
                        {
                            distanceJoint2D.distance = r.startDist;
                        }
                        else if (TryGetComponent(out FixedRopeRender f))
                        {
                            distanceJoint2D.distance = f.startDist;
                        }
                    }
                }
                break;
            case 1:
                if (fixedJoint2D == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (fixedJoint2D.connectedBody == null)
                        Destroy(gameObject);
                }
                break;
            case 2:
                if (springJoint2D == null)  
                    Destroy(gameObject);
                else
                {
                    if (springJoint2D.connectedBody == null)
                        Destroy(gameObject);
                    else
                    {
                        constraintLocalPos = springJoint2D.transform.InverseTransformPoint(springJoint2D.connectedBody.transform.TransformPoint(springJoint2D.connectedAnchor));
                        dist = springJoint2D.distance;
                        springJoint2D.enableCollision = true;
                        if (TryGetComponent(out FixedSpringRender f))
                        {
                            springJoint2D.distance = f.startDist;
                        }
                    }
                }
                break;
            case 3:
                if (hingeJoint2D == null)
                    Destroy(gameObject);
                else if (hingeJoint2D.connectedBody == null)
                    Destroy(gameObject);
                break;
        }
    }

    private void OnDestroy()
    {
        if (distanceJoint2D != null)
            Destroy(distanceJoint2D);
        if (fixedJoint2D != null)
            Destroy(fixedJoint2D);
        if (hingeJoint2D != null)
            Destroy(hingeJoint2D);
        if (springJoint2D != null)
            Destroy(springJoint2D);
    }
}
