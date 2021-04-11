using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotSpeed = 5f;
    private GameObject target;
    private float rotY;
    private float maxY = 60f;
    private float minY = -45f;

    private void Start()
    {
        target = GetComponentInParent<Transform>().gameObject;
    }

    private void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            Camera.main.transform.LookAt(target.transform);

            transform.Rotate(0f, Input.GetAxis("Mouse X") * rotSpeed, 0f);
            rotY += Input.GetAxis("Mouse Y") * rotSpeed;
            rotY = Mathf.Clamp(rotY, minY, maxY);
            transform.localEulerAngles = new Vector3(-rotY, transform.localEulerAngles.y, 0f);
        }
    }
}
