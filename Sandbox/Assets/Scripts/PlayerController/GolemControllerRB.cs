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
    // pose states
    public GolemRaiseState RaiseAbility { get; private set; }
    public GolemIdleState StepAbility { get; private set; }
    // ai states
    public GolemWaitState AIWaitState { get; private set; }
    public GolemFollowState AIFollowState { get; private set; }
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
    {
        base.Awake();
        IdleState = new GolemIdleState(this, "Idle");
        MoveState = new GolemMoveState(this, "Movement");
        RaiseAbility = new GolemRaiseState(this, "Raise");
        StepAbility = new GolemIdleState(this, "Step");
        AIFollowState = new GolemFollowState(this, "Movement");
        AIWaitState = new GolemWaitState(this, "Idle");
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // set initial controlled state
        ControllerEnabled = false;
        if (ControllerEnabled)
        {
            CanSwitch = true;
            InitialState(IdleState);
        }
        else
        {   // set initial AI state
           InitialState(AIFollowState);
        }
    }
    #endregion
    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    public override void EnableControls()
    {
        base.EnableControls();
        //ChangeState(IdleState);
    }
    public override void DisableControls()
    {
        base.DisableControls();
    }
    // change facing direction
    public override void Flip()
    {
        base.Flip();
    }
}




