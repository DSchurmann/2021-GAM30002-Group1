using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChildControllerRB : PlayerControllerRB
{

    // states
    #region States
    // player controlled states
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
    // player AI states
    public AIFollowState AIFollowState { get; private set; }
    public AIWaitState AIWaitState { get; private set; }
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
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
        AIWaitState = new AIWaitState(this, "Idle");
        AIFollowState = new AIFollowState(this, "Movement");
       
    }

    // Start is called before the first frame update
    public override void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        FacingDirection = 1;

        // set initial controlled state
        ControllerEnabled = true;
        CanSwitch = false;
        if (ControllerEnabled)
        {
           //CanSwitch = true;
            InitialState(IdleState);
            
        }
        else
        {
            //CanSwitch = false;
            InitialState(AIFollowState);
        }
        
        
    }
    #endregion
    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public override void Update()
    {
        
        // keep record of current velocity
        CurrentVelocity = RB.velocity;
        // update current state
        CurrentState.Update();
    }

    public override void FixedUpdate()
    {
        // fixed update current state
        CurrentState.FixedUpdate();
    }
    #endregion

}




