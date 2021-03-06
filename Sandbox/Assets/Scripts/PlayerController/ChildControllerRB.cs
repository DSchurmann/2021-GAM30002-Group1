using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChildControllerRB : PlayerControllerRB
{
    public string _CurrentState;
    // states
    #region States
    // player controlled movement states
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }
    public InAirState InAirState { get; private set; }
    public LandState LandState { get; private set; }
    public AttackState AttackState { get; private set; }
    // player Wall states
    public WallSlideState WallSlideState { get; private set; }
    public WallGrabState WallGrabState { get; private set; }
    public WallClimbState WallClimbState { get; private set; }
    public WallGrabLedgeState WallGrabLedgeState { get; private set; }
    public WallClimbLedgeState WallClimbLedgeState { get; private set; }
    public WallJumpState WallJumpState { get; private set; }
    // player AI states
    public AIFollowState AIFollowState { get; private set; }
    public AIWaitState AIWaitState { get; private set; }
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
    {
        base.Awake();
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

        WallClimbLedgeState = new WallClimbLedgeState(this, "LedgeClimb");
        WallGrabLedgeState = new WallGrabLedgeState(this, "LedgeHang");


    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
       
        // set initial controlled state
        ControllerEnabled = true;
        if (ControllerEnabled)
        {
            CanSwitch = true;
            InitialState(IdleState);
        }
        else
        {
            // set initial AI state
            InitialState(AIWaitState);
        }
    }
    #endregion
    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _CurrentState = CurrentState.GetType().Name;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void EnableControls()
    {
        base.EnableControls();
        QueueState(IdleState);
        //ChangeState(IdleState);
    }
    public override void DisableControls()
    {
        base.DisableControls();
        QueueState(AIWaitState);
        //ChangeState(AIWaitState);
    }
    // change facing direction 
    public override void Flip()
    {
        base.Flip();
        //flip sprite
        transform.Rotate(0.0f, 180.0f, 0, 0f);
    }
    #endregion
}
