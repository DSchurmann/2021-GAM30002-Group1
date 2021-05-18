using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GolemControllerRB : PlayerControllerRB
{

    // states
    #region States
    public GolemIdleState IdleState { get; private set; }
    public GolemMoveState MoveState { get; private set; }
    public GolemIdleState RaiseAbility { get; private set; }
    public GolemWaitState AIWaitState { get; private set; }
    public GolemFollowState AIFollowState { get; private set; }
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
    {
        IdleState = new GolemIdleState(this, "Idle");
        MoveState = new GolemMoveState(this, "Movement");
        RaiseAbility = new GolemIdleState(this, "Raise");
        AIFollowState = new GolemFollowState(this, "Movement");
        AIWaitState = new GolemWaitState(this, "Idle");
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
        ControllerEnabled = false;
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




