using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollider : MonoBehaviour
{
    public GameObject coll;
    public LineRenderer line;
    ConstraintController controller;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        controller = GameObject.Find("ConstraintButton").GetComponent<ConstraintController>();
        coll.gameObject.SetActive(false);
        if (line.positionCount > 2)
            coll.transform.position = line.GetPosition(line.positionCount / 2);
        else if (line.positionCount == 2)
            coll.transform.position = (line.GetPosition(0) + line.GetPosition(1)) / 2;
    }

    private void Update()
    {
        if (coll == null)
            Destroy(gameObject);
        else
        {
            if (controller.selectedID == 1)
            {
                if (line.positionCount > 2)
                    coll.transform.position = line.GetPosition(line.positionCount / 2);
                else if (line.positionCount == 2)
                    coll.transform.position = (line.GetPosition(0) + line.GetPosition(1)) / 2;
                coll.gameObject.SetActive(true);
            }
            else
            {
                coll.gameObject.SetActive(false);
            }
        }
    }
}
