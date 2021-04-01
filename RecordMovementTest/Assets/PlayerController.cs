using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isRecordingMovement = false;
    private List<Vector3> RecordedPositions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        RecordedPositions = new List<Vector3>();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }
    void OnCollisionExit()
    {
        isGrounded = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0f,1f,0f) * 6f, ForceMode.Impulse);

            isGrounded = false;
        }

        // Movement vector
        Vector3 movement = new Vector3(xInput * 0.25f, 0f, 0f);

        // Current position
        Vector3 currPosition = transform.position;

        // New position
        Vector3 newPosition = currPosition + movement;

        // Move the rigid body
        rb.MovePosition(newPosition);

        // Record positon
        if (Input.GetButton("RecordMovement"))
        {
            if (!isRecordingMovement)
            {
                isRecordingMovement = true;
            }
            RecordedPositions.Add(currPosition);
        }
        else
        {
            if (isRecordingMovement)
            {
                isRecordingMovement = false;
                // tell golem to move
                GameObject golem = GameObject.Find("Golem");

                golem.GetComponent<GolemController>().setRecordedPositions(RecordedPositions);

                // reset the list 
                RecordedPositions = new List<Vector3>();
            }
        }
    }
}
