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
    public GolemStepState StepAbility { get; private set; }
    public GolemRaiseState TPoseAbility { get; private set; }
    public GolemStepState CrouchAbility { get; private set; }
    // ai states
    public GolemWaitState AIWaitState { get; private set; }
    public GolemFollowState AIFollowState { get; private set; }

    public bool posing;
    public bool poseLocked;
    public string _CurrentState = "none";
    #endregion
    // Awake and Start functions
    #region Start Functions
    public override void Awake()
    {
        base.Awake();
        IdleState = new GolemIdleState(this, "Idle");
        MoveState = new GolemMoveState(this, "Movement");
       
        RaiseAbility = new GolemRaiseState(this, "Raise");
        StepAbility = new GolemStepState(this, "Step");
        TPoseAbility = new GolemRaiseState(this, "Raise");
        CrouchAbility = new GolemStepState(this, "Step");

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
           InitialState(AIWaitState);
        }
        posing = false;
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
    #endregion

    public override void EnableControls()
    {
        InputHandler.SetNorthFalse();
        InputHandler.SetSouthFalse();
        InputHandler.SetEastFalse();
        InputHandler.SetWestFalse();
        base.EnableControls();
        //ChangeState(IdleState);
    }

    public void DisablePoseLocked()
    {
        poseLocked = false;
    }
    public override void DisableControls()
    {
        base.DisableControls();
        //QueueState(AIWaitState);
        //Debug.Log("GOLEM PUT INTO WAIT STATE");
        if(!posing)
            ChangeState(AIWaitState);
    }
    // change facing direction
    public override void Flip()
    {
        base.Flip();
    }
}




