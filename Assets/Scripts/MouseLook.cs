using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public bool canMoveCamera = true;
    public float mouseSensitivity = 10;

    private float x = 0;
    private float y = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (canMoveCamera == true)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        float dtime = Time.deltaTime;
        x += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        y += Input.GetAxis("Mouse X") * mouseSensitivity;

        x = Mathf.Clamp(x, -90, 90);

        transform.localRotation = Quaternion.Euler(x, 0, 0);
        player.transform.localRotation = Quaternion.Euler(0, y, 0);

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }*/
    }
}
