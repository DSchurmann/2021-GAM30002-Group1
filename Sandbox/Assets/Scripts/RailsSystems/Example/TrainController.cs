using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    private TrainInputs inputs;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    private float jumpHeight = 7.5f;

    private Rigidbody rb;
    private Train train;

    private Vector3 curVecity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        train = GetComponent<Train>();
        inputs = GetComponent<TrainInputs>();

        curVecity = new Vector3();
    }

    private void Update()
    {
        curVecity = rb.velocity;

        Vector3 workingVelocity = new Vector3();
        workingVelocity = train.MoveX(moveSpeed * inputs.Movement.x, inputs.Movement.y);

        // check to jump
        if (CheckIfGrounded() && inputs.Jump)
        {
            workingVelocity.y = jumpSpeed;
        }
        else
        {
            workingVelocity.y = curVecity.y;
        }

        // Update the velocity
        rb.velocity = workingVelocity;
    }

    public bool CheckIfGrounded()
    {
        const float GroundDistance = 0.5f;
        return Physics.Raycast(transform.position, -Vector3.up, GroundDistance + 0.1f);
    }
}
