using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(CharacterController))]


// climbing based off tutorial https://www.youtube.com/watch?v=_pOJipOVTfg

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public bool IsWalking = true;
    [SerializeField] public bool IsRunning = false;
    [SerializeField] public bool IsCrawling = false;
    [SerializeField] public bool IsClimbing = false;
    [SerializeField] private float CarrySpeed = 1;
    [SerializeField] private float CrawlSpeed = 2;
    [SerializeField] private float ClimbSpeed = 2;
    [SerializeField] private float ClimbRotSpeed = 2;
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

    private RaycastHit wallRayHit;
    private Transform climbingHelper;
    private Vector3 climbingStartPos;
    private Vector3 climbingTargetPos;
    private Quaternion climbingStartRot;
    private Quaternion climbingTargetRot;
    private bool climbingInPosition;
    public float climbingPositionOffset = 0.5f;
    public float climbingOffsetFromWall = 0.3f;
    private bool climbingIsLerping;
    private float climbingT;

    // Start is called before the first frame update
    void Start()
    {
        climbingHelper = new GameObject().transform;
        climbingHelper.name = "climb helper";
        CharacterController = GetComponent<CharacterController>();
        Jumping = false;
        CharControllerHeighCopy = CharacterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        OnPlatform();
        if (!IsClimbing)
        {
            // the jump state needs to read here to make sure it is not missed
            if (!Jump)
            {
                Jump = UnityEngine.Input.GetButtonDown("Jump");
            }
            JumpPressed = Jump;

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
    }

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        if (!IsClimbing)
        {
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

                if (Jump && canJump)
                {
                    MoveDir.y = JumpSpeed;
                    Jump = false;
                    Jumping = true;
                }
            }
            else
            {
                if (EnableGravity)
                    MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
            }

            if (movementEnabled)
                CollisionFlags = CharacterController.Move(MoveDir * Time.fixedDeltaTime);
        }
        else
        {
            ClimbUpdate();
        }
        
    }

    private void ClimbUpdate()
    {
        if (!climbingInPosition)
        {
            ClimbingGetInPosition();
            return;
        }
        if (!climbingIsLerping)
        {
            Vector3 h = climbingHelper.right * Input.x;
            Vector3 v = climbingHelper.up * Input.y;
            Vector3 moveDir = (h + v).normalized;

            if (moveDir == Vector3.zero)
                return;

            IsClimbing = ClimbingCanMove(moveDir);
            if (!IsClimbing)
                return;

            climbingT = 0;
            climbingIsLerping = true;
            climbingStartPos = transform.position;
            //Vector3 tp = climbingHelper.position - transform.position;

            climbingTargetPos = climbingHelper.position;
        }
        else
        {
            float delta = Time.deltaTime;
            climbingT += delta * ClimbSpeed;
            if (climbingT > 1)
            {
                climbingT = 1;
                climbingIsLerping = false;
            }

            Vector3 cp = Vector3.Lerp(climbingStartPos, climbingTargetPos, climbingT);
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, climbingHelper.rotation, delta * ClimbRotSpeed);
        }

    }

    private bool ClimbingCanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        float dis = climbingPositionOffset;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            return false;
        }

        origin += moveDir * dis;
        dir = climbingHelper.forward;
        float dis2 = 0.5f;

        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            climbingHelper.position = PosWithOffset(origin, hit.point);
            climbingHelper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin += dir * dis2;
        dir = -Vector3.up;
        Debug.DrawRay(origin, dir, Color.yellow);

        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            float angle = Vector3.Angle(climbingHelper.up, hit.normal);
            if (angle < 40)
            {
                climbingHelper.position = PosWithOffset(origin, hit.point);
                climbingHelper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }

        return false;
    }

    private void ClimbingGetInPosition()
    {
        float delta = Time.deltaTime;
        climbingT += delta * ClimbSpeed;

        if (climbingT > 1)
        {
            climbingT = 1;
            climbingInPosition = true;
        }

        Vector3 tp = Vector3.Lerp(climbingStartPos, climbingTargetPos, climbingT);
        transform.position = tp;
        transform.rotation = Quaternion.Slerp(transform.rotation, climbingHelper.rotation, delta * ClimbRotSpeed);
    }

    private Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * climbingOffsetFromWall;
        return target + offset;
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool IsWalkingMod = UnityEngine.Input.GetButton("Fire2");
        IsCrawling = UnityEngine.Input.GetKey(KeyCode.LeftControl);
        bool IsClimbingMod = UnityEngine.Input.GetButton("Fire3");

        Input = new Vector2(horizontal, vertical);
        // normalize input if it exceeds 1 in combined length:
        if (Input.sqrMagnitude > 1)
        {
            Input.Normalize();
        }

        if (IsClimbingMod && (IsClimbing || CheckForClimb()))
        {
            IsRunning = false;
            IsWalking = false;
            IsCrawling = false;
            speed = ClimbSpeed;
            CharacterController.height = CharControllerHeighCopy;
            CharacterController.center = new Vector3(0, CharacterController.height / 2, 0);


            if (!IsClimbing)
            {
                IsClimbing = true;
                climbingHelper.transform.rotation = Quaternion.LookRotation(-wallRayHit.normal);
                climbingStartPos = transform.position;
                climbingTargetPos = wallRayHit.point + (wallRayHit.normal * climbingOffsetFromWall);
                climbingT = 0;
                climbingInPosition = false;
            }
            return;
        }

        if (!Jumping)
            IsWalking = IsWalkingMod;

        if(-CharacterController.velocity.y>0.1f)
        {
            Falling = true;
        }
        //bool isCarrying = GetComponent<PlayerInteractions>().carrying;
        // change walkspeed if carrying

        // set the desired speed to be walking or running
        //canJump = true;
        if(IsCrawling)
        {
            IsRunning = false;
            IsWalking = false;
            IsClimbing = false;
            speed = CrawlSpeed;
            CharacterController.height = 1;
            CharacterController.center = new Vector3(0, 0.5f, 0);
        }
        else
        {
            IsClimbing = false;
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
    }

    // check if the player can climb from current position
    public bool CheckForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 0.5f;
        Vector3 dir = transform.forward;
        if( Physics.Raycast(origin, dir, out wallRayHit, 1))
        {
            climbingHelper.position = PosWithOffset(origin, wallRayHit.point);
            return true;
        }
        return false;
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
            if (groundHit.collider.gameObject.GetComponent<MovingPlatform>() != null)
            {
                transform.parent = groundHit.collider.gameObject.transform;
                return true;
            }
        }
        transform.parent = null;

        return false;
    }
}
