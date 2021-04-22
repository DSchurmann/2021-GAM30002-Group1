using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(CharacterController))]

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public bool IsWalking = true;
    [SerializeField] public bool IsRunning = false;
    [SerializeField] public bool IsCrawling = false;
    [SerializeField] private float CarrySpeed = 1;
    [SerializeField] private float CrawlSpeed = 2;
    [SerializeField] private float WalkSpeed = 2;
    [SerializeField] private float RunSpeed = 5;
    [SerializeField] private float JumpSpeed = 5;
    [SerializeField] private float StickToGroundForce = 10;
    [SerializeField] private float GravityMultiplier = 2;

    public Camera Camera;

    private Vector2 Input;
    private Vector3 MoveDir = Vector3.zero;
    public CharacterController CharacterController;

    private CollisionFlags CollisionFlags;
    private bool PreviouslyGrounded;

    private bool Jump;
    public bool Jumping;
    public bool Falling;
    public bool canJump;
    public bool movementEnabled;
    public bool EnableGravity;

    public bool JumpPressed;

    private float CharControllerHeighCopy;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        Jumping = false;
        CharControllerHeighCopy = CharacterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        OnPlatform();

        // the jump state needs to read here to make sure it is not missed
      
        if (UnityEngine.Input.GetButtonDown("Jump"))
        {
            if (!Jump)
            {
                if (canJump)
                {
                    Jump = true;
                }
                JumpPressed = true;
                Invoke("ReleaseJumpPressed", 0.2f);
            }
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

        if (moveDirection != Vector3.zero && movementEnabled)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = newRotation;
        }
    }

    void ReleaseJumpPressed()
    {
        JumpPressed = false;
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

            if (canJump)
            {
                if(Jump)
                {
                    Debug.Log("JUMP PRESSED");
                    MoveDir.y = JumpSpeed;
                    Jump = false;
                    Jumping = true;
                }
            }
        }
        else
        {
            if (EnableGravity)
                MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
        }

         
        CollisionFlags = CharacterController.Move(MoveDir * Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool IsWalkingMod = UnityEngine.Input.GetButton("Fire3");
        IsCrawling = UnityEngine.Input.GetKey(KeyCode.LeftControl);

        if(!Jumping)
            IsWalking = IsWalkingMod;

        if(-CharacterController.velocity.y>0.1f)
        {
            Falling = true;
        }
        else
        {
            Falling = false;
        }
        //bool isCarrying = GetComponent<PlayerInteractions>().carrying;
        // change walkspeed if carrying

        // set the desired speed to be walking or running
        //canJump = true;
        if(IsCrawling)
        {
            IsRunning = false;
            IsWalking = false;
            speed = CrawlSpeed;
            CharacterController.height = 1;
            CharacterController.center = new Vector3(0, 0.5f, 0);
        }
        else
        {
            speed = IsWalking ? WalkSpeed : RunSpeed;
            if(speed == RunSpeed && Input.sqrMagnitude > 0.01f && CharacterController.isGrounded)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }
            CharacterController.height = CharControllerHeighCopy;
            CharacterController.center = new Vector3(0, CharacterController.height / 2, 0);
        }

        if (movementEnabled)
            Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (Input.sqrMagnitude > 1)
        {
            Input.Normalize();
        }
    }

    public Vector3 Velocity()
    {
        return CharacterController.velocity;
    }

    public float GroundDistance()
    {
        float groundDistance = -1;
        float groundMinDistance = 0.25f;
        float groundMaxDistance = 0.5f;
        RaycastHit groundHit;

        if (CharacterController != null)
        {
            // radius of the SphereCast
            float radius = CharacterController.radius * 0.9f;
            var dist = 10f;
            // ray for RayCast
            Ray ray2 = new Ray(transform.position + new Vector3(0, CharacterController.height / 2, 0), Vector3.down);
            // raycast for check the ground distance
            if (Physics.Raycast(ray2, out groundHit, (CharacterController.height / 2) + dist) && !groundHit.collider.isTrigger)
                dist = transform.position.y - groundHit.point.y;
            // sphere cast around the base of the capsule to check the ground distance
            if (dist >= groundMinDistance)
            {
                Vector3 pos = transform.position + Vector3.up * (CharacterController.radius);
                Ray ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out groundHit, CharacterController.radius + groundMaxDistance) && !groundHit.collider.isTrigger)
                {
                    Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit);
                    float newDist = transform.position.y - groundHit.point.y;
                    if (dist > newDist) dist = newDist;
                }
            }
            groundDistance = (float)System.Math.Round(dist, 2);
            return groundDistance;
        }
        return groundDistance;
    }

    public bool OnPlatform()
    {
        RaycastHit groundHit;

        Ray ray = new Ray(transform.position , Vector3.down);
        // raycast for check the ground distance
        if (Physics.Raycast(ray, out groundHit, 1))
        {
            if (groundHit.collider.gameObject.GetComponentInChildren<MovingPlatform>() != null)
            {
                transform.parent = groundHit.collider.gameObject.transform;
                return true;
            }
        }
        transform.parent = null;

        return false;
    }
}
