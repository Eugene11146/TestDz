using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : MonoBehaviour
{
    public bool dir = false;
    public CameraMovementForPhone cam;
    public RotationButton anotherButton;
    float rotationSpeed = 0;
    public bool isRotating = false;

    private void FixedUpdate()
    {
        if (isRotating && !anotherButton.isRotating)
        {
            cam.rotationSpeed = rotationSpeed;
            if (dir)
            {
                rotationSpeed = 0.0001f;
            }
            else
            {
                rotationSpeed = -0.0001f;
            }
        }
    }

    public void startRotating()
    {
        isRotating = true;
    }

    public void endRotating()
    {
        isRotating = false;
        rotationSpeed = 0;
    }
}
