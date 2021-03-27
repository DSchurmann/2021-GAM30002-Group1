using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    public Camera playerCamera;
    public float minZoomLevel;
    public float maxZoomLevel;
    private float zoomLevel;
    private float zoomScale;
    private Vector3 initCamPos;

    public Transform Sword;
    public Transform BackSwordHolderBone;

    public bool AI_controlled;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        initCamPos = playerCamera.transform.position;
        zoomScale = 1;
        zoomLevel = -8;

        if (Sword)
            Sword.parent = BackSwordHolderBone;

    }

    // Update is called once per frame
    void Update()
    {
        //ZoomPlayerCamera();
        AnimateMovement();

        if (Input.mouseScrollDelta.magnitude != 0)
        {
            //zoomLevel += Input.mouseScrollDelta.y * zoomScale;
            playerCamera.transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * zoomScale);
        }
    }

    private void ZoomPlayerCamera()
    {
        Debug.Log(zoomLevel);
        zoomLevel += Input.mouseScrollDelta.y * zoomScale;
        zoomLevel = Mathf.Clamp(zoomLevel, minZoomLevel, maxZoomLevel);
        float vel = 0.5f;
        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, zoomLevel, ref vel, 1);

        //playerCamera.transform.DOLocalMoveZ((playerCamera.transform.TransformDirection(Vector3.forward).magnitude) * zoomScale, 0.5f);
    }

    private void AnimateMovement()
    {
        animator.SetFloat("Velocity", controller.velocity.magnitude);
    }

}




