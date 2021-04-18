using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHandler : MonoBehaviour
{
    //Parameters -- Core
    public bool isFocus; //Is the Child the active player character?
    public bool isActive; //Is the Child in a state where they are receiving inputs?\
    public enum States { idle, walk, run, jump, crouch }; //Default: Idle
    public States state;
    public bool grounded; //Detecting ground?

    public GameObject highlightedObj;
    public GameObject liftedObj;
    public bool riding;

    //Parameters -- Input
    public Vector2 inputMovement;
    public Vector2 grabAngle;

    //Parameters -- Calc
    private float groundCheckDist;
    public float runSpeed;
    public float crawlSpeed;
    public float movSpeed;
    public float jumpHeight;
    public float followDist;
    private float grabCheckDist;

    public bool crouching;
    public bool jumping;
    public bool canMove;

    //Parameters -- Components
    private Animator animator;
    private PlayerAnimationController animation;
    public Rigidbody rb;
    public CapsuleCollider col;
    private float colHeight;
    private Vector3 colCenter;

    // Start is called before the first frame update
    void Start()
    {
        //By default, set active to true
        isActive = (true);

        //Set Distance to Check for Ground
        groundCheckDist = (0.1f);
        grabCheckDist = (1f);

        //Set Animator
        animator = GetComponent<Animator>();
        animation = GetComponent<PlayerAnimationController>();
        //Set Rigidbody
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        colHeight = col.height;
        colCenter = col.center;
        //rb.AddForce(new Vector3(5f, 0f, 0f));

        //Set Movement Speed
        //movSpeed = (3.75f);

        //Set Jump Height
        //jumpHeight = (7f);

        //Set Follow Dist
        followDist = (3f);
    }

    // Update is called once per frame
    void Update()
    {
        //Set FocusedState to the Opposite of GameHandler's SwitchMode
        isFocus = (!GameHandler.GH.switchMode);

        //Is not focus? Follow
        if (!isFocus && !GameHandler.GH.waitMode)
        {
            //Follow
            FollowProcedure();
        }
        else if (!isFocus)
        {
            //Set Mov
            inputMovement = (new Vector2(0f, 0f));
        }

        //If Active
        if (isActive)
        {
            //States
            GetInputs(); //Determines what state we're in
        }

        //Animate!
        StateAnimate();         
    }

    //Physics
    private void FixedUpdate()
    {
        //Check for the Ground Below Us
        GroundCheck();

        if(crouching)
        {
            col.height = 1;
            col.center = new Vector3(0, 0.6f, 0);
        }
        else
        {
            col.height = colHeight;
            col.center = colCenter;
        }
    }

    //Read Movement Input
    public void GetMove(Vector2 mov)
    {
        //Set
        inputMovement = mov;

        if (inputMovement.sqrMagnitude > 1)
        {
            inputMovement.Normalize();
        }
    }

    //Fake Movement Inputs to Follow Child
    public void FollowProcedure()
    {
        //Get Our Position, Position of Child
        Vector3 pos = transform.position;
        Vector3 targPos = GameHandler.GH.golemObj.transform.position;

        //Check Distance
        if (Vector3.Distance(pos, targPos) > followDist)
        {
            //Move Towards
            Vector3 angle = (targPos - pos).normalized;

            //Set Mov
            inputMovement = new Vector2(angle.x, angle.z);
        }
        else
        {
            //Set Mov
            inputMovement = (new Vector3(0f, 0f, 0f));
        }
    }

    //Read Jump Input
    public void GetJump()
    {
        //If Grounded && Active
        if (grounded && isActive)
        {
            //Add Force
            Debug.Log("JUMP PRESSED");
            rb.velocity = (new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z));
            
        }
    }

    //Read Crouch Input
    public void GetCrouch(bool value)
    {
        //If Grounded && Active && not carrying something
        if (grounded && isActive)
        {
            crouching = value;
        }
    }

    //Get Inputs
    public void GetInputs()
    {
        //Is our Movement Input 0?
        if (inputMovement == Vector2.zero && grounded)
        {
            //Idle
            state = States.idle;
            if(jumping)
                 jumping = false;
        }
        else
        {
            if(crouching)
            {
                if(liftedObj)
                {
                    DropHeldObject();
                }
                state = States.crouch;
            }
            else
            {
                //Airborne?
                if (grounded)
                {
                    rb.useGravity = false;
                    //Moving!
                    state = States.walk;
                    jumping = false;
                }
                else
                {
                    rb.useGravity = true;
                    //Falling!
                    state = States.jump;
                    //jumping = false;
                }
            }
         
        }
    }
    public void Interact() //On Interact press
    {
        Debug.Log("Grab! (CH)");

        if(liftedObj)
        {
            DropHeldObject();
        }
        else
        {
            if(!crouching)
            {
                // pickup object if pass check
                highlightedObj = GrabCheck();
                if (highlightedObj != null)
                {
                    PickupObject(highlightedObj);
                }
            }
        }
    }
    // drop held object
    void DropHeldObject()
    {
        if (liftedObj)
        {
            liftedObj.transform.parent = null;
            liftedObj.GetComponent<BoxCollider>().enabled = true;
            liftedObj.GetComponent<Rigidbody>().isKinematic = false;
            liftedObj = null;
        }
    }
    // pickup detected object
    void PickupObject(GameObject obj)
    {
        if(obj != null)
        {
            Vector3 pos = transform.position;
            pos.y += 1;
            liftedObj = highlightedObj;
            liftedObj.GetComponent<BoxCollider>().enabled = false;
            liftedObj.GetComponent<Rigidbody>().isKinematic = true;

            liftedObj.transform.position = pos;
            liftedObj.transform.parent = transform;
        }
    }

    //Animate based on state
    public void StateAnimate()
    {
        //Animate!
        if (state == States.idle)
        {
            //animator.Play("Idle");
            animation.ChangeState(animation.IDLE);
        }
        //Move!
        else if (state == States.walk)
        {
            //animator.Play("Run");
            movSpeed = runSpeed;
            animation.ChangeState(animation.RUN);
        }
        //Airborne!
        else if (state == States.jump)
        {
            //animator.Play("Jump");
            movSpeed = crawlSpeed*2;
            animation.ChangeState(animation.JUMP);
        }
        else if (state == States.crouch)
        {
            //animator.Play("Jump");
            movSpeed = crawlSpeed;
            animation.ChangeState(animation.CRAWL);
        }

        if (canMove)
        {
            // rotate to face direction of input
            Vector3 moveDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
            Vector3 direction = Camera.main.transform.TransformDirection(moveDirection);
            if (moveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(direction);
                newRotation.x = 0;
                newRotation.z = 0;
                transform.rotation = newRotation;
            }

            // moveDirection in direction facing 
            Vector3 desiredMove = transform.forward * direction.magnitude;

            transform.position += (desiredMove * movSpeed * Time.deltaTime);
        }
       

        // rotate to input direction

    }

    //Ground Check
    private void GroundCheck()
    {
        //Cast Ray
        RaycastHit hit;
        

        //If Colliding with the Ground?
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist))
        {
            Debug.DrawLine(transform.position, (transform.position - new Vector3(0f, groundCheckDist, 0f)), Color.green);
            //Set Grounded
            grounded = true;

            //Set Ride
            if (hit.collider.gameObject.name == "Head")
            {
                //Riding!
                riding = (true);
            }
            else
                riding = (false);
        }
        else
        {
            //Set Not Grounded
            grounded = false;
            riding = false;
        }
    }

    //Grab Check -- Check for Grabbables?
    private GameObject GrabCheck()
    {
        //Set
        if (inputMovement != new Vector2(0f, 0f))
            grabAngle = inputMovement;

        //Cast Ray
        RaycastHit hit;
        int bitMask = 1 << 10;
        Vector3 root = (transform.position + (new Vector3(0f, .35f, 0f)));
        Vector3 targ = (root + transform.forward);
        Debug.DrawLine(root, targ, Color.yellow);

        //If Colliding with the Ground?
        if (Physics.Raycast(root, transform.forward, out hit, grabCheckDist, bitMask))
        {
            //Set Grounded
            Debug.Log("Grabbable! CHILD");

            //Set Highlighted
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
}
