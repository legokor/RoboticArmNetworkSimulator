using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://forum.unity.com/threads/rotating-with-middle-mouse-button-issue.491248/

public class CameraOrbit : MonoBehaviour
{
    public float mouseSensitivity;
    public float scrollSensitvity;
    public float linearSpeed;

    protected bool pressed = false;
    protected Vector3 localRot;

    void Start()
    {
        localRot = transform.localRotation.eulerAngles;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            pressed = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            pressed = false;
        }

        if (((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0)) && (pressed))
        {
            localRot.y += Input.GetAxis("Mouse X") * mouseSensitivity;
            localRot.x -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            //Clamp the y Rotation to horizon and not flipping over at the top
            if (localRot.x < 0f)
                localRot.x = 0f;
            else if (localRot.x > 90f)
                localRot.x = 90f;

            Quaternion QT = Quaternion.Euler(localRot);
            transform.rotation = QT; // Time.deltaTime * orbitSensitivity *
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSensitvity;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * Time.deltaTime * linearSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * Time.deltaTime * linearSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.up * Time.deltaTime * linearSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.up * Time.deltaTime * linearSpeed;
        }
    }
}
