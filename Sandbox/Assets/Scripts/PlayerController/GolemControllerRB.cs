using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GolemControllerRB : PlayerControllerRB
{

    // states
    #region States
    public GolemIdleState IdleState { get; private set; }
    public GolemIdleState RaiseAbility { get; private set; }
    public GolemFollowState FollowState { get; private set; }
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
    {
        IdleState = new GolemIdleState(this, "Idle");
        RaiseAbility = new GolemIdleState(this, "Raise");
        FollowState = new GolemFollowState(this, "Movement");
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
        if (ControllerEnabled)
        {
            InitialState(IdleState);
        }
        else
        {
            InitialState(FollowState);
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




