using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Camera playerCamera;
    private float zoomScale;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        zoomScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        // control camera zoom
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            playerCamera.transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * zoomScale);
        }

       
    }


}




