using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHandler : MonoBehaviour
{
    //Parameters -- Core
    public bool isFocus; //Is the Golem the active player character?
    public bool isActive; //Is the Golem in a state where they are receiving inputs?\
    public enum States { idle, move, air, rune }; //Default: Idle
    public States state;
    public bool grounded; //Detecting ground?
    public bool flip; //Moving left?
    public Rune activeRune;
    public GameObject highlightedObj;
    public GameObject liftedObj;

    //Parameters -- Input
    public Vector2 inputMovement;
    public Vector2 grabAngle;

    //Parameters -- Calc
    private float groundCheckDist;
    public float movSpeed;
    public float followDist;
    private float grabCheckDist;
    public float runeProgress;

    //Parameters -- Components
    private Animator animator;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //By default, set active to true
        isActive = (true);

        //Set GroundCheck
        groundCheckDist = (2.75f);

        //Set Grabcheck
        grabCheckDist = (5f);

        //Set Animator
        animator = GetComponent<Animator>();

        //Set Rigidbody
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(new Vector3(5f, 0f, 0f));

        //Set Movement Speed
        movSpeed = (2.5f);

        //Set Follow Dist
        followDist = (3f);
    }

    // Update is called once per frame
    void Update()
    {
        //Set FocusedState to the Opposite of GameHandler's SwitchMode
        isFocus = (GameHandler.GH.switchMode);

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

        //SMASH RUNE?
        if (activeRune != null && activeRune.runeType == Rune.RuneType.red && activeRune.runeRedType == Rune.RedRune.smash)
        {
            //Increase SmashRune Progress
            if (runeProgress < 1f)
                runeProgress += (Time.deltaTime);
            else
                activeRune = (null);
        }
        //LEAP RUNE?
        else if (activeRune != null && activeRune.runeType == Rune.RuneType.red && activeRune.runeRedType == Rune.RedRune.leap)
        {
            //Increase LeapRune Progress
            if (runeProgress < 2.5f)
                runeProgress += (Time.deltaTime);
            else
                activeRune = (null);
        }
        //TOSS RUNE?
        else if (liftedObj != null && activeRune != null && activeRune.runeType == Rune.RuneType.yellow && activeRune.runeYellowType == Rune.YellowRune.toss)
        {
            //Increase SmashRune Progress
            if (runeProgress < 1f)
                runeProgress += (Time.deltaTime);
            else
                activeRune = (null);
        }
        else
            runeProgress = (0f);

        //Set Hold
        if (liftedObj != null)
            liftedObj.transform.position = (transform.position - (new Vector3(0f, .75f, 0.9f)));
    }

    //Physics
    private void FixedUpdate()
    {
        //Check for the Ground Below Us
        GroundCheck();

        //Check for grabbable objects?
        GrabCheck();
    }

    //Rune Use
    public void ActivateRune(int runeIndex)
    {
        //Debug
        Debug.Log("Activating (if possible) rune " + runeIndex);

        //Does the rune exist?
        bool exists = (GameHandler.GH.runes[runeIndex] != null);

        //Return if not / Golem not receiving input
        if (!exists || !isActive)
            return;

        //Otherwise, activate / deactivate Rune
        if (activeRune == null && grounded)
            activeRune = (GameHandler.GH.runes[runeIndex]);
        else if (grounded)
        {
            //Is this the same Rune? AND toggleable?
            if (activeRune == GameHandler.GH.runes[runeIndex] && activeRune.runeType == Rune.RuneType.blue)
            {
                //Deactivate
                activeRune = (null);
            }
            //Else are we activating a different rune?
            else if (activeRune != GameHandler.GH.runes[runeIndex])
            {
                //Activate new
                activeRune = (GameHandler.GH.runes[runeIndex]);
            }
        }

        //SMASH RUNE?
        if (activeRune != null && activeRune.runeType == Rune.RuneType.red && activeRune.runeRedType == Rune.RedRune.smash)
        {
            //Increase SmashRune Progress
            if (runeProgress < 1f)
                runeProgress += (Time.deltaTime);
            else
                activeRune = (null);
        }
        
        //LEAP RUNE?
        else if (activeRune != null && activeRune.runeType == Rune.RuneType.red && activeRune.runeRedType == Rune.RedRune.leap)
        {
            if (grounded)
            {
                //Do LeapRune AddForce
                if (!flip)
                    rb.velocity = (new Vector2(movSpeed, movSpeed * 3f));
                else
                    rb.velocity = (new Vector2(-movSpeed, movSpeed * 3f));

                //Increase LeapRune Progress
                if (runeProgress < 3f)
                    runeProgress += (Time.deltaTime);
                else
                    activeRune = (null);
            }
        }
        else
            runeProgress = (0f);

        //THROW RUNE?
        if (liftedObj != null && activeRune != null && activeRune.runeType == Rune.RuneType.yellow && activeRune.runeYellowType == Rune.YellowRune.toss)
        {
            //Increase SmashRune Progress
            if (runeProgress < 1f)
                runeProgress += (Time.deltaTime);
            else
                activeRune = (null);

            //Set Thing
            GameObject toss = liftedObj;
            liftedObj = (null);
            if (!flip)
                toss.GetComponent<Rigidbody>().velocity = new Vector3(movSpeed * 2f, movSpeed * 3f);
            else
                toss.GetComponent<Rigidbody>().velocity = new Vector3(-movSpeed * 2f, movSpeed * 3f);

            //Unset Rune
            activeRune = (null);
        }
        else if (liftedObj == null && activeRune != null && activeRune.runeType == Rune.RuneType.yellow && activeRune.runeYellowType == Rune.YellowRune.toss)
            activeRune = (null);

        //LIFT RUNE?
        if (activeRune != null && activeRune.runeType == Rune.RuneType.red && activeRune.runeRedType == Rune.RedRune.lift)
        {
            //Run Lift Code
            if (liftedObj != null)
            {
                //Unset
                liftedObj = (null);
            }
            else if (highlightedObj != null)
            {
                //Lift!
                liftedObj = highlightedObj;
            }

            //Unset Rune
            activeRune = (null);
        }
        else if (liftedObj != null)
        {
            //Drop!
            liftedObj = (null);
        }
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
        Vector3 targPos = GameHandler.GH.childObj.transform.position;
        targPos.z += 1f;
        //Check Distance
        if (Vector3.Distance(pos, targPos) > followDist)
        {
            //Move Towards
            Vector3 angle = (targPos - pos).normalized;

            //Set Mov
            inputMovement = new Vector2(angle.x,angle.z);
        }
        else
        {
            //Set Mov
            inputMovement = (new Vector3(0f, 0f, 0f));
        }
    }

    //Get Inputs
    public void GetInputs()
    {
        //Is our Movement Input 0? (Not acting on a Rune)
        if (inputMovement == new Vector2(0f, 0f) && grounded && activeRune == null)
        {
            //Idle
            state = States.idle;
        }
        else
        {
            //Airborne?
            if (grounded)
            {
                //RUNE CHECK
                if (activeRune != null)
                    state = States.rune;
                else
                    state = States.move;
            }
            else
            {
                //RUNE CHECK
                if (activeRune != null)
                    state = States.rune;
                else
                    state = States.air;
            }

            //Check for flip? (When not acting on Rune)
            if (state != States.rune)
                flip = (inputMovement.x < 0f);
            else if (state == States.rune && activeRune.runeType != Rune.RuneType.blue)
                flip = !(rb.velocity.x > 0f);
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
            }

            //Move based on movement vector
            float x = (inputMovement.x * (movSpeed * Time.deltaTime));
            float y = (inputMovement.y * (movSpeed * Time.deltaTime));
            transform.Translate(new Vector3(x, 0f, y));
            //rb.velocity = (new Vector3(x, rb.velocity.y, y));
        }
        //Airborne!
        else if (state == States.air)
        {
            //Animate!
            //animator.Play("Jump");

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
            }

            //Move based on movement vector
            float x = (inputMovement.x * (movSpeed * Time.deltaTime));
            float y = (inputMovement.y * (movSpeed * Time.deltaTime));
            transform.Translate(new Vector3(x, 0f, y));
            //rb.velocity = (new Vector3(x, rb.velocity.y, y));
        }
        //RUNE
        else if (state == States.rune)
        {
            //Play Animation!
            animator.Play(activeRune.runeAnim);

            //Rotate
            foreach (Transform t in transform)
            {
                if (t.gameObject.GetComponent<SpriteRenderer>() != null && t != this.transform)
                {
                    t.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
            }

            //Check Flip
            if (flip)
                transform.eulerAngles = (new Vector3(0f, 180f, 0f));
            else
            {
                transform.eulerAngles = (new Vector3(0f, 0, 0f));
            }
        }

        //Blanket Fix
        if (state != States.rune)
            transform.eulerAngles = (new Vector3(0f, 0, 0f));
    }

    //Ground Check
    private void GroundCheck()
    {
        //Cast Ray
        RaycastHit hit;
        int bitMask = 1 << 7;
        Debug.DrawLine(transform.position, (transform.position - new Vector3(0f, groundCheckDist, 0f)));

        //If Colliding with the Ground?
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDist, bitMask))
        {
            //Set Grounded
            grounded = (true);
        }
        else
        {
            //Set Not Grounded
            grounded = (false);
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
        int bitMask = 1 << 9;
        Vector3 root = (transform.position - (new Vector3(0f, 2f, 0f)));
        Vector3 targ = (root + new Vector3(grabAngle.x, 0f, grabAngle.y));
        Debug.DrawLine(root, targ, Color.red);

        //If Colliding with the Ground?
        if (Physics.Raycast(root, targ, out hit, grabCheckDist, bitMask))
        {
            //Set Grounded
            Debug.Log("Grabbable!");

            //Set Highlighted
            highlightedObj = hit.collider.gameObject;
        }
        else
        {
            //Null
            highlightedObj = (null);
        }
    }

    //Smash Hit
    public void OnTriggerEnter(Collider other)
    {
        //Check Destroyable
        if (other.gameObject.CompareTag("Destructible"))
        {
            Destroy(other.gameObject);
        }
    }
}
