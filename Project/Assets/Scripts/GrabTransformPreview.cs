using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTransformPreview : MonoBehaviour
{
    public delegate void DragDelegate ();
    public event DragDelegate onGrabTransformDrag;

    public void Drag()
    {
        onGrabTransformDrag?.Invoke();
        transform.position = new Vector3(GlobalSetting.mainCamera.ScreenToWorldPoint(Input.mousePosition).x, GlobalSetting.mainCamera.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
    }
}
