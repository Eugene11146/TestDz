using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public float scrollSensitivity = 0.5f;
    bool canDrag = false;
    Vector3 mousePos;
    RaycastHit2D hit;
    GameObject GOtarget;
    TargetJoint2D trgJoint;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity, 0.5f, 10);

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Rigidbody2D a))
            {
                GOtarget = hit.collider.gameObject;
                trgJoint = GOtarget.AddComponent<TargetJoint2D>();
                trgJoint.anchor = GOtarget.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                trgJoint.frequency = 20;
            }
            if (!EventSystem.current.IsPointerOverGameObject())
                canDrag = true;
            else
                canDrag = false;
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        }
        if (Input.GetMouseButton(0))
        {
            if (GOtarget != null)
            {
                trgJoint.target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (canDrag)
            {
                mainCamera.transform.position += mousePos - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(trgJoint);
            GOtarget = null;
        }
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
