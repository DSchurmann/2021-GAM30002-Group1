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
    public DeathState DeathState { get; private set; }
    #endregion

    private bool interactGrab;
    public bool interacting;
    public bool alive = true;

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


        DeathState = new DeathState(this, "Death");


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
        isTouchingWall = CheckTouchingWall();

        //Debug.Log("INTERACTING: " + interacting);
        //isTouchingWall = GetComponent<ClimbingController>()
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void EnableControls()
    {
        base.EnableControls();
        //QueueState(AIWaitState);
        //ChangeState(IdleState);
    }
    public override void DisableControls()
    {
        base.DisableControls();
        //QueueState(AIWaitState);
        //Debug.Log("CHILD PUT INTO WAIT STATE");
        ChangeState(AIWaitState);
    }
    // change facing direction 
    public override void Flip()
    {
        base.Flip();
        //flip sprite
        transform.Rotate(0.0f, 180.0f, 0, 0f);
    }

    public override bool CheckTouchingWall()
    {
        return GetComponent<ClimbingController>().isTouchingWall;
    }
    #endregion
    // Trigger functions for interactable objects
    #region Trigger Functions
    public void OnTriggerEnter(Collider other)
    {
        InteractableItem item = other.GetComponent<InteractableItem>();
        if (item != null && ControllerEnabled)
        {
            item.DisplayUI();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        InteractableItem item = other.GetComponent<InteractableItem>();
        List<string> interactStates = new List<string>(new string[] { "IdleState", "LandState", "MoveState" });
        bool canInteract = interactStates.Contains(CurrentState.GetType().Name);
        interactGrab = InputHandler.InputInteract;


        if (item != null && interactGrab && canInteract)
        {
            item.Interact();
            InputHandler.SetInteractFalse();
            ChangeState(IdleState);
            //GameController.GH.uiHandler.GetComponentInChildren<PauseMenu>().Pause(false);
        }
        else if (item != null && !ControllerEnabled)
        {
            item.HideUI();
            if(item.IsOpen)
            {
                item.Interact();
            }

        }
        else if(item!= null && item.IsOpen)
        {
            item.HideUI();
            interacting = true;
            //GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
            //GetComponent<PlayerInput>().actions.FindActionMap("Menu").FindAction("Interact").performed += GameController.GH.uiHandler.GetComponentInChildren<PauseMenu>().Unpause;
            //DisableControls();
            //GetComponent<PlayerInput>().currentActionMap.Disable();
        }
        else if(item!= null && !item.isTextActive)
        {
            item.DisplayUI();
            interacting = false;
            //EnableControls();
            //GetComponent<PlayerInput>().currentActionMap.Enable();
        }

       
    }


    public void OnTriggerExit(Collider other)
    {
        InteractableItem item = other.GetComponent<InteractableItem>();
        if (item != null || (item != null && !ControllerEnabled))
        {
            item.HideUI();
        }
    }
    #endregion
}
