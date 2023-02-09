using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinCollider : MonoBehaviour
{
    public GameObject coll;
    ConstraintController controller;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        controller = GameObject.Find("ConstraintButton").GetComponent<ConstraintController>();
        coll.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (coll == null)
            Destroy(gameObject);
        else
        {
            if (controller.selectedID == 1 || mainCamera.GetComponent<CameraMovementForPhone>().capturedSelectZone != null)
            {
                coll.gameObject.SetActive(true);
            }
            else
            {
                coll.gameObject.SetActive(false);
            }

        }
    }
}
