using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRB : StateMachine
{
    // other character reference
    public PlayerControllerRB Other;
    [SerializeField]
   
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
    [Range(0.0f, 60.0f)]
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
    public bool isGrounded;
    [Header("Follow State")]
    [Range(0.0f, 10.0f)]
    public float closeDistance = 2f;
    public float closeRange = 0.3f;
    public float followSpeedFactor = 0.5f;
    public float maxFollowSpeed = 10f;

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
    [SerializeField] public float groundAngle = 0.0f;
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

    public float pathRotationSpeed;
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
        Train = GetComponent<Train>();
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
        isGrounded = CheckIfGrounded();
        // keep record of current velocity
        CurrentVelocity = RB.velocity;
        // update current state if game not paused
        if(CurrentState!=null && !GameController.GH.GamePaused)
        {
            CurrentState.Update();
            //Debug.Log(this.GetType().Name + ":  " + CurrentState.GetType().Name);
        }

        // change to next state, when animation complete
        if(NextState!=null)
        {
            PlayerState state = (PlayerState)CurrentState;

            if (state.AnimationComplete())
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
        //Debug.Log("Controls enable");
        ControllerEnabled = true;
        CanSwitch = true;
    }
    public virtual void DisableControls()
    {
        //Debug.Log("Controls Disable");
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
    public void SetVelocityX(float velocityX)
    {
        workVector = Train.MoveX(velocityX);

        // rotate character to path direction
        if (workVector.sqrMagnitude > 0)
        {
            Quaternion rot = Quaternion.LookRotation(workVector, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 2 * pathRotationSpeed);
        }

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

    // set position of target to move towards
    public void MoveTowardsTarget(Vector3 target, float speed, Rail targetRail, RailType railType)
    {
        // look for the closest node to the player 
        Node startNode = Train.rail.Nodes[Train.rail.GetSegmentOfClosestPoint(transform.position)];
        //Debug.Log("start " + startNode.name + " rail " + Train.rail.name);

        Node endNode;

        // ensure end node is a compadible railtype
        if (targetRail.RailType != RailType.Both)
        {
            GameObject[] railObjects = GameObject.FindGameObjectsWithTag("Rail");
            // find search for clostest rail to target

            float bestDistance = float.PositiveInfinity;
            Rail closestRail = Train.rail; // set the rail to the current rail as default

            foreach (GameObject railObject in railObjects)
            {
                Rail r = railObject.GetComponent<Rail>();

                if (r.RailType != RailType.Both && r.RailType != railType)
                    continue;

                float dist = r.DistanceToClosestPoint(target);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestRail = r;
                }
            }

            // get the node of the closest rail
            endNode = closestRail.Nodes[closestRail.GetSegmentOfClosestPoint(target)];
        }
        else
        {
            // look for the closest node to the target
            endNode = targetRail.Nodes[targetRail.GetSegmentOfClosestPoint(target)];
        }       
        Debug.Log("end " + endNode.name + " rail " + targetRail.name);

        // calculate the path though graph search
        Node targetNode = Rail.CalulateNextNodeinPathThroughRails(startNode, endNode, railType);
        //if (targetNode) Debug.Log("next " + targetNode.name + " rail " + targetNode.rail.name);

        // move player based on calculated path
        // test if both characters are on the same segment
        if (!targetNode)
        {
            // both characters on the same segment move towards player
            Vector3 angle = (target - transform.position).normalized;
            SetVelocityX(speed * angle.x);
        }
        // test if target node on the same rail as player
        else if (targetNode.rail == Train.rail)
        {
            // compare the index's of the rail to get the direction
            int direction = targetNode.GetNodeIndex() - startNode.GetNodeIndex();
            SetVelocityX(speed * direction);
        }
        // test if close enough to switch rails
        else if (Vector3.Distance(new Vector3(targetNode.transform.position.x, 0f, targetNode.transform.position.z), new Vector3(transform.position.x, 0f, transform.position.z)) > Train.RailSeekRange)
        {
            // keep moving the player closer to the junction
            SetVelocityX(-speed);
        }
        //move player to the next rail
        else
        {
            //Debug.Log("switch with: " + Vector3.Distance(new Vector3(targetNode.transform.position.x, 0f, targetNode.transform.position.z), new Vector3(transform.position.x, 0f, transform.position.z)));
            Train.rail = targetNode.rail;
            Train.segment = targetNode.GetNodeIndex();
            // ensure segment is within range
            if (Train.segment <= targetNode.rail.Nodes.Count - 1)
            {
                Train.segment = targetNode.rail.Nodes.Count - 2;
            }
        }

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
        float GroundDistance = 0.1f;
        //return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        return Physics.Raycast(groundCheck.position, -Vector3.up, GroundDistance + 0.1f);
    }

    //check forward wall
    public virtual bool CheckTouchingWall()
    {
        Debug.DrawRay(wallCheck.position, Vector3.right * FacingDirection * wallCheckDistance, Color.cyan);
        return Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, wallCheckDistance);
    }

    //check forward gap
    public bool CheckGapAhead()
    {
        return GetComponent<ClimbingController>().isGapAhead;
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




