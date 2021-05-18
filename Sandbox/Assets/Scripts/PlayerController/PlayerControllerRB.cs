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
    // states
    #endregion
    // player states
    #region States

    #endregion
    // checks variables
    #region Check Variables
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance = 0.5f;
    //public float groundCheckRadius = 0.3f;
    //public LayerMask groundLayer;
    #endregion
    // components
    #region Components
    public PlayerInputHandler InputHandler { get; protected set; }
    public Rigidbody RB { get; protected set; }
    public Collider Collider { get; protected set; }
    public Animator Anim { get; protected set; }
    #endregion
    // other variables
    #region Other Variables
    // player controlled
    public bool ControllerEnabled { get; set; }
    // player controlled
    public bool CanSwitch { get; set; }
    // current vecocity
    public Vector2 CurrentVelocity { get; protected set; }
    // facing direcion
    public int FacingDirection { get; protected set; }
    // work vector for calculation
    private Vector2 workVector;
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
            CurrentState.Update();
    }

    public virtual void FixedUpdate()
    {
        // fixed update current state
        if (CurrentState != null)
            CurrentState.FixedUpdate();
    }
    #endregion
    // functions that set
    #region Set Functions
    // set velocity 
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workVector.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }
    // set velocity of x 
    public void SetVelocityX(float velocity)
    {
        workVector.Set(velocity, CurrentVelocity.y);
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }

    // set velocity of y 
    public void SetVelocityY(float velocity)
    {
        workVector.Set(CurrentVelocity.x, velocity);
        RB.velocity = workVector;
        CurrentVelocity = workVector;
    }
    #endregion
    // functions that check
    #region Check Functions

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
        return Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, wallCheckDistance);
    }
    public bool CheckTouchingWallBehind()
    {
        return Physics.Raycast(wallCheck.position, Vector3.right * -FacingDirection, wallCheckDistance);
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
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0, 0f);
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




