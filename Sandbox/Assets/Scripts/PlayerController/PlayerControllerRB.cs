using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRB : StateMachine
{
    // other character reference
    public PlayerControllerRB Other;
   /* [Header("Movement")]
    [Range(0.1f, 10.0f)]
    public float SwitchPlayerDelay = 1f;*/
    // player variables
    #region Player Variables
    public List<string> Inventory;
    [Header("Movement")]
    [Range(0.0f, 10.0f)]
    public float MovementSpeed = 4f;
    [Header("Jump State")]
    [Range(0.0f, 10.0f)]
    public float JumpSpeed = 10f;
    public int jumpsAllowed = 1;
    [Header("In Air State")]
    [Range(0.0f, 10.0f)]
    public float inAirMovementSpeed = 0.2f;
    [Range(0.0f, 1.0f)]
    public float jumpInputMultiplier = 0.5f;
    [Range(0.0f, 1.0f)]
    public float jumpTimeBuff = 0.5f;
    [Header("Wall Slide State")]
    [Range(0.0f, 10.0f)]
    public float wallSlideSpeed = 1.5f;
    [Header("Wall Climb State")]
    [Range(-10.0f, 10.0f)]
    public float wallClimbSpeed = 3f;
    [Range(-0.5f, 0.5f)]
    public float wallClimbOffsetPosition = 0.5f;
    [Range(-0.5f, 0.5f)]
    public float wallClimbDistance = 0.3f;
    [Header("Wall Jump State")]
    [Range(0.0f, 10.0f)]
    public float wallJumpSpeed = 4.0f;
    [Range(0.0f, 1.0f)]
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public bool isTouchingWall;
    [Header("Follow State")]
    [Range(0.0f, 1.0f)]
    public float closeDistance = 2f;
    // states
    #endregion
    // player states
    #region States
    #endregion
    // checks variables
    #region Check Variables
    [SerializeField] Transform groundCheck;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance = 0.5f;
    //public float groundCheckRadius = 0.3f;
    //public LayerMask groundLayer;
    #endregion
    // components
    #region Components
    public PlayerInputHandler InputHandler { get; protected set; }
    public Rigidbody RB { get; protected set; }
    public Collider Collider { get; protected set; }
    public Animator Anim { get; protected set; }
    public Train Train { get; protected set; }
    #endregion
    // other variables
    #region Other Variables
    // player controlled
    public bool ControllerEnabled { get; set; }
    public bool Waiting { get; set; }
    public bool Following { get; set; }
    // player controlled
    public bool CanSwitch { get; set; }
    // current vecocity
    public Vector3 CurrentVelocity { get; protected set; }
    // facing direcion
    public int FacingDirection { get; protected set; }
    // work vector for calculation
    private Vector3 workVector;
    #endregion
    // Awake and Start functions
    #region Start Functions
    public virtual void Awake()
    {
        // initialise states with player and animation name
        // eg. IdleState = new IdleState(this, "Idle");
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        // set components
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        // set facing direction
        FacingDirection = 1;

        // set initial state
        //eg. InitialState(IdleState);
    }
    #endregion
    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public virtual void Update()
    {
        // keep record of current velocity
        CurrentVelocity = RB.velocity;
        // update current state
        if(CurrentState!=null)
        {
            CurrentState.Update();
            Debug.Log(this.GetType().Name + ":  " + CurrentState.GetType().Name);
        }

        if(NextState!=null)
        {
            if(CurrentState.AnimationComplete())
            {
                ChangeState(UseNextState());
            }
        }
           
    }

    public virtual void FixedUpdate()
    {
        // fixed update current state
        if (CurrentState != null)
            CurrentState.FixedUpdate();
    }
    #endregion
    // functions that switch players
    #region Switch Player Functions
    public virtual void SwitchPlayer()
    {

    }
    public virtual void EnableControls()
    {
        ControllerEnabled = true;
        CanSwitch = true;
    }
    public virtual void DisableControls()
    {
        ControllerEnabled = false;
        CanSwitch = false;
    }

    #endregion
    // functions that set
    #region Set Functions
    // set velocity 
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workVector = Train.MoveX(angle.x * velocity * direction);
        workVector.y = angle.y * velocity;
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }
    // set velocity of x 
    public void SetVelocityX(float velocityX, float velocityZ = 0f)
    {
        workVector = Train.MoveX(velocityX, velocityZ);
        workVector.y = CurrentVelocity.y;
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }
    // set velocity of y 
    public void SetVelocityY(float velocity)
    {
        workVector.Set(CurrentVelocity.x, velocity, CurrentVelocity.z);
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }

    public void SetRailedVelocity(Vector2 velocity)
    {
        Vector3 tempVelocity;
        tempVelocity = Train.MoveX(velocity.x);
        tempVelocity.y = velocity.y;
        RB.velocity = tempVelocity;
        CurrentVelocity = tempVelocity;
    }
    #endregion
    // functions that check
    #region Check Functions

    // check if can switch player
    public bool CheckIfCanSwitch()
    {
        // check if controlle is enabled
        if(ControllerEnabled)
        {
            return true;
        }
        return false;
    }
    // check if grounded
    public bool CheckIfGrounded()
    {
        float GroundDistance = 0.001f;
        //return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        return Physics.Raycast(groundCheck.position, -Vector3.up, GroundDistance + 0.1f);
    }
    //check forward wall
    public bool CheckTouchingWall()
    {
        Debug.DrawRay(Vector3.right, Vector3.right * FacingDirection);
        return Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, wallCheckDistance);
    } 
    //check forward wall Climb Ability
    public bool CheckgWallClimbAbility()
    {
        //RaycastHit wallHit;
        //Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, out wallHit, wallCheckDistance);
        // check if wall is climbable (currently just checks if on climbable wall layer)
        return Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, wallCheckDistance, 1<<12);
    }
    public virtual bool CheckTouchingWallBehind()
    {
        return Physics.Raycast(wallCheck.position, Vector3.right * -FacingDirection, wallCheckDistance);
    }

    public void HandlePlatformLanding()
    {
        RaycastHit groundHit;

        Ray ray = new Ray(transform.position, Vector3.down);
        // raycast for check the ground distance
        if (Physics.Raycast(ray, out groundHit, 1))
        {
            //Debug.Log("Landed on: " + groundHit.collider.gameObject.name);
            if (groundHit.collider.gameObject.GetComponentInChildren<MovingPlatform>() != null)
            {
                //Debug.Log("Player on platform");
                transform.parent = groundHit.collider.gameObject.transform;
                //return true;
            }

            if(groundHit.collider.gameObject.transform.parent)
            {
                if (groundHit.collider.gameObject.transform.parent.GetComponentInChildren<MovingPlatform>() != null)
                {
                    transform.parent = groundHit.collider.gameObject.transform.parent.transform;
                    //return true;
                }
            }
            
        }
        transform.parent = null;

        //return false;
    }
    // check for axis flip
    public void CheckForFlip(int xInput)
    {
        if(xInput!=0 && xInput!= FacingDirection)
        {
            Flip();
        }
    }
    #endregion
    // other functions
    #region Other Functions
    // flip direction
    public virtual void Flip()
    {
        FacingDirection *= -1;
    }
    #endregion
    // debug code
    #region Debug Code
    private void OnDrawGizmos()
    {
        // Draw ground check sphere
        //Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
    #endregion
}




