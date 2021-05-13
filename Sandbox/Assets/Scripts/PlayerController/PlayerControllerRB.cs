using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRB : StateMachine
{
    // player state variables
    #region Player State Variables
    [Header("Movement State")]
    [Range(0.0f, 10.0f)]
    public float MovementSpeed = 4f;
    [Header("Jump State")]
    public int jumpsAllowed = 1;
    [Range(0.0f, 10.0f)]
    public float JumpSpeed = 10f;
    [Header("In Air State")]
    public float jumpTimeBuff = 0.2f;
    public float jumpMultiplier = 0.5f;
    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;
    [Header("Wall Climb State")]
    public float wallClimbSpeed = 3f;
    [Header("Wall Jump State")]
    public float wallJumpSpeed = 4.0f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    [Header("Inventory")]
    public List<string> Inventory; 
    #endregion
    // components
    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody RB { get; private set; }
    public Collider Collider { get; private set; }
    public Animator Anim { get; private set; }
    #endregion
    // states
    #region States
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }
    public InAirState InAirState { get; private set; }
    public LandState LandState { get; private set; }
    public AttackState AttackState { get; private set; }
    public WallSlideState WallSlideState { get; private set; }
    public WallGrabState WallGrabState { get; private set; }
    public WallClimbState WallClimbState { get; private set; }
    public WallJumpState WallJumpState { get; private set; }
    #endregion
    // checks variables
    #region Check Variables
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance = 0.5f;
    //public float groundCheckRadius = 0.3f;
    //public LayerMask groundLayer;
    #endregion
    // other variables
    #region Other Variables
    // current vecocity
    public Vector2 CurrentVelocity { get; private set; }
    // facing direcion
    public int FacingDirection { get; private set; }
    // work vector for calculation
    private Vector2 workVector;
    #endregion
    // Awake and Start functions
    #region Start Functions
    private void Awake()
    {
        IdleState = new IdleState(this, "Idle");
        MoveState = new MoveState(this, "Movement");
        JumpState = new JumpState(this, "Jump");
        InAirState = new InAirState(this, "Jump");
        LandState = new LandState(this, "Idle");
        AttackState = new AttackState(this, "Attack");
        WallSlideState = new WallSlideState(this, "WallSlide");
        WallGrabState = new WallGrabState(this, "WallGrab");
        WallClimbState = new WallClimbState(this, "WallClimb");
        WallJumpState = new WallJumpState(this, "Jump");
    }

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        FacingDirection = 1;
        InitialState(IdleState);
    }
    #endregion
    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    void Update()
    {
        // keep record of current velocity
        CurrentVelocity = RB.velocity;
        // update current state
        CurrentState.Update();
    }

    private void FixedUpdate()
    {
        // fixed update current state
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




