using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(CharacterController))]

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private bool IsWalking = true;
    [SerializeField] private float WalkSpeed = 2;
    [SerializeField] private float RunSpeed = 5;
    [SerializeField] private float JumpSpeed = 5;
    [SerializeField] private float StickToGroundForce = 10;
    [SerializeField] private float GravityMultiplier = 2;

    public Camera Camera;
    private bool Jump;
    private float YRotation;
    private Vector2 Input;
    private Vector3 MoveDir = Vector3.zero;
    private CharacterController CharacterController;


    private CollisionFlags CollisionFlags;
    private bool PreviouslyGrounded;
    private float StepCycle;
    private float NextStep;
    private bool Jumping;

    public bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        Jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        // the jump state needs to read here to make sure it is not missed
        if (!Jump && canJump)
        {
            Jump = UnityEngine.Input.GetButtonDown("Jump");
        }

        if (!PreviouslyGrounded && CharacterController.isGrounded)
        {
            MoveDir.y = 0f;
            Jumping = false;
        }
        if (!CharacterController.isGrounded && !Jumping && PreviouslyGrounded)
        {
            MoveDir.y = 0f;
        }

        PreviouslyGrounded = CharacterController.isGrounded;

        Vector3 moveDirection = new Vector3(Input.x, 0, Input.y);
        Vector3 direction = Camera.main.transform.TransformDirection(moveDirection);

        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = newRotation;
        }

    }
    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        Vector3 desiredMove = transform.forward * Input.magnitude;
        
        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, CharacterController.radius, Vector3.down, out hitInfo,
                           CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        MoveDir.x = desiredMove.x * speed;
        MoveDir.z = desiredMove.z * speed;


        if (CharacterController.isGrounded)
        {
            MoveDir.y = -StickToGroundForce;

            if (Jump)
            {
                MoveDir.y = JumpSpeed;

                Jump = false;
                Jumping = true;
            }
        }
        else
        {
            MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
        }

        CollisionFlags = CharacterController.Move(MoveDir * Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool wasWalking = IsWalking;

        // set the desired speed to be walking or running
        speed = IsWalking ? WalkSpeed : RunSpeed;
        Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (Input.sqrMagnitude > 1)
        {
            Input.Normalize();
        }
    }

}
