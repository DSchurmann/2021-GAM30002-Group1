using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    private float maxSpeed = 5f;
    private float rotSpeed = 45f;
    private CharacterController cc;
    private Quaternion rot;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        //get move input, forwards and backwards
        move.z = Input.GetAxis("Vertical") * maxSpeed;
        move.x = Input.GetAxis("Horizontal") * maxSpeed;
        //get rotation based on input
        Vector3 rotation = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + Input.GetAxis("Turn") * rotSpeed * Time.deltaTime, rot.eulerAngles.z);
        rot.eulerAngles = rotation;

        //change rotation and adjust movement based on changes
        transform.rotation = rot;
        move = rot * move;

        //move object
        cc.Move(move * Time.deltaTime);
    }
}
