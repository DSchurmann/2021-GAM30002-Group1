using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHandler : MonoBehaviour
{
    //Parameters -- Core
    public bool isFocus; //Is the Child the active player character?
    public bool isActive; //Is the Child in a state where they are receiving inputs?\
    public enum States { idle, move, jump }; //Default: Idle
    public States state;
    public bool grounded; //Detecting ground?
    public bool flip; //Moving left?
    public GameObject highlightedObj;
    public GameObject liftedObj;
    public bool riding;

    //Parameters -- Input
    public Vector2 inputMovement;
    public Vector2 grabAngle;

    //Parameters -- Calc
    private float groundCheckDist;
    public float movSpeed;
    public float jumpHeight;
    public float followDist;
    private float grabCheckDist;

    //Parameters -- Components
    private Animator animator;
    private Rigidbody rb;
    public List<GameObject> leftSide;
    public List<GameObject> rightSide;


    // Start is called before the first frame update
    void Start()
    {
        //By default, set active to true
        isActive = (true);

        //Set Distance to Check for Ground
        groundCheckDist = (0.75f);
        grabCheckDist = (1f);

        //Set Animator
        animator = GetComponent<Animator>();

        //Set Rigidbody
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(new Vector3(5f, 0f, 0f));

        //Set Movement Speed
        movSpeed = (3.75f);

        //Set Jump Height
        jumpHeight = (7f);

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

        //Set Hold
        if (liftedObj != null)
            liftedObj.transform.position = (transform.position + (new Vector3(0f, .7f, 0f)));
    }

    //Physics
    private void FixedUpdate()
    {
        //Check for the Ground Below Us
        GroundCheck();

        //Check for grabbable objects?
        GrabCheck();
    }

    //Read Movement Input
    public void GetMove(Vector2 mov)
    {
        //Set
        inputMovement = mov;
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
            rb.velocity = (new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z));

            //Play Jump sound (FMod Test)
            FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/TestSound", transform.position);
        }
    }

    //Get Inputs
    public void GetInputs()
    {
        //Is our Movement Input 0?
        if (inputMovement == new Vector2(0f, 0f) && grounded)
        {
            //Idle
            state = States.idle;
        }
        else
        {
            //Airborne?
            if (grounded)
            {
                //Moving!
                state = States.move;
            }
            else
            {
                //Falling!
                state = States.jump;
            }

            //Check for flip?
            flip = (inputMovement.x < 0f);
        }
    }
    public void Interact() //On Interact press
    {
        //Niceu
        Debug.Log("Grab! (CH)");
        if (highlightedObj != null)
        {
            liftedObj = (highlightedObj);
        }
        else
        {
            liftedObj = (null);
        }
    }

    //Animate based on state
    public void StateAnimate()
    {
        //Animate!
        if (state == States.idle)
        {
            //Animate!
            if (liftedObj == null)
                animator.Play("Idle");
            else
                animator.Play("Hold");

            //If riding...
            if (riding)
                transform.position = new Vector3(GameHandler.GH.golemObj.transform.position.x, transform.position.y, GameHandler.GH.golemObj.transform.position.z);
        }
        //Move!
        else if (state == States.move)
        {
            //Animate!
            if (liftedObj == null)
                animator.Play("Movement");
            else
                animator.Play("HoldMove");

            //Check Flip
            if (flip)
            {
                //Rotate
                foreach (Transform t in transform)
                {
                    if (t.gameObject.GetComponent<SpriteRenderer>() != null && t != this.transform)
                    {
                        t.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    }
                }

                //Set Order
                foreach (GameObject i in leftSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (-1);
                }
                //Set Order
                foreach (GameObject i in rightSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (1);
                }
            }
            else
            {
                //Rotate
                foreach (Transform t in transform)
                {
                    if (t.gameObject.GetComponent<SpriteRenderer>() != null && t != this.transform)
                    {
                        t.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    }
                }

                //Set Order
                foreach (GameObject i in leftSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (1);
                }
                //Set Order
                foreach (GameObject i in rightSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (-1);
                }
            }

            //Move based on movement vector
            float x = (inputMovement.x * (movSpeed * Time.deltaTime));
            float y = (inputMovement.y * (movSpeed * Time.deltaTime));
            transform.Translate(new Vector3(x, 0f, y)); 
            //rb.velocity = (new Vector3(x, rb.velocity.y, y));
        }
        //Airborne!
        else if (state == States.jump)
        {
            //Animate!
            animator.Play("Jump");

            //Check Flip
            if (flip)
            {
                //Rotate
                foreach (Transform t in transform)
                {
                    if (t.gameObject.GetComponent<SpriteRenderer>() != null && t != this.transform)
                    {
                        t.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    }
                }

                //Set Order
                foreach (GameObject i in leftSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (-1);
                }
                //Set Order
                foreach (GameObject i in rightSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (1);
                }
            }
            else
            {
                //Rotate
                foreach (Transform t in transform)
                {
                    if (t.gameObject.GetComponent<SpriteRenderer>() != null && t != this.transform)
                    {
                        t.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    }
                }

                //Set Order
                foreach (GameObject i in leftSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (1);
                }
                //Set Order
                foreach (GameObject i in rightSide)
                {
                    //Set Order
                    i.GetComponent<SpriteRenderer>().sortingOrder = (-1);
                }
            }

            //Move based on movement vector
            float x = (inputMovement.x * (movSpeed * Time.deltaTime));
            float y = (inputMovement.y * (movSpeed * Time.deltaTime));
            transform.Translate(new Vector3(x, 0f, y)); 
            //rb.velocity = (new Vector3(x, rb.velocity.y, y));
        }
    }

    //Ground Check
    private void GroundCheck()
    {
        //Cast Ray
        RaycastHit hit;
        int bitMask = 1 << 7;
        int bittwo = 1 << 9;
        int bitthree = 1 << 10;
        int bitfour = 1 << 11;
        Debug.DrawLine(transform.position, (transform.position - new Vector3(0f, groundCheckDist, 0f)));

        //If Colliding with the Ground?
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist, bitMask) || Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist, bittwo) || Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist, bitthree) || Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist, bitfour))
        {
            //Set Grounded
            grounded = (true);

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
            grounded = (false);
            riding = (false);
        }
    }

    //Grab Check -- Check for Grabbables?
    private void GrabCheck()
    {
        //Set
        if (inputMovement != new Vector2(0f, 0f))
            grabAngle = inputMovement;

        //Cast Ray
        RaycastHit hit;
        int bitMask = 1 << 10;
        Vector3 root = (transform.position - (new Vector3(0f, .5f, 0f)));
        Vector3 targ = (root + new Vector3(grabAngle.x, 0f, grabAngle.y));
        Debug.DrawLine(root, targ, Color.red);

        //If Colliding with the Ground?
        if (Physics.Raycast(root, targ, out hit, grabCheckDist, bitMask))
        {
            //Set Grounded
            Debug.Log("Grabbable! CHILD");

            //Set Highlighted
            highlightedObj = hit.collider.gameObject;
        }
        else
        {
            //Null
            highlightedObj = (null);
        }
    }
}
