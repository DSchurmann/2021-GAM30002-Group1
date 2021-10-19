using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]
public class ClimbingController : MonoBehaviour
{
    [Header("Properies")]
    public Vector3 vault_SPH; //' speed, height and distance variables of vault

    public float[] origins_forward;
    public float[] origins_down;
    public LedgeDetector ledgeDetector;

    public bool isTouchingWall;
    public bool isGapAhead;
    public bool isGapJumpable;

    // gap variables
    public float minDistanceToGap = 0.5f;
    public float gapCheckHeight = 0.5f;
    public float gapCheckDepth = 1f;

    // climbing variables
    public bool isVaulting;
    public bool isClimbing;

    public bool canVault;
    public bool canClimb;
    public bool canJumpClimb;

    public bool ledgeFound;

    public LayerMask Climbable;
    public bool isEnabled;

    // player climbing properties
    public float maxLedgeJumpHeight = 2;
    public float climbForwardAmount = 0.4f;
    //public float MaxDistanceToLedge_WallClimb;

    // ground properties
    public Vector3 groundAngle;
    public GameObject groundObject;
    public float maxSlopeAngle;


    // components
    public Rigidbody _rb;
    public PlayerInputHandler InputHandler { get; protected set; }

    //protected bool inputInteract;

  
    // Start is called before the first frame update
    void Start()
    {
        ledgeDetector = new LedgeDetector(this.transform, origins_forward, origins_down);

        _rb = GetComponent<Rigidbody>();
        InputHandler = GetComponent<PlayerInputHandler>();


    }

    // Update is called once per frame
    void Update()
    {
        //inputInteract = InputHandler.InputInteract;

        if (ledgeDetector != null && isEnabled)
        {
            groundObject = ledgeDetector.GroundBelow(transform.position + Vector3.up * 0.2f, gapCheckDepth);
            isTouchingWall = ledgeDetector.TouchingWall().Any(w => w == true);
            isGapAhead = ledgeDetector.GapCheck(transform.position + Vector3.up * gapCheckHeight + (transform.forward * minDistanceToGap), gapCheckDepth);
            groundAngle = ledgeDetector.GroundCheck(transform.position + Vector3.up * 0.2f , gapCheckDepth);
            ledgeFound = ledgeDetector.ledgeFound;
        }
        else
        {
            ledgeFound = false;
            isTouchingWall = false;
        }
       

        if(isTouchingWall)
        {
            if(ledgeDetector.TouchingWall().Length>0 && ledgeDetector.HeightCheck(ledgeDetector.ledgePosition, 2))
            {

                bool[] hits = ledgeDetector.TouchingWall();

               


                if (ledgeFound && ledgeDetector.ForwardCheck(ledgeDetector.ledgePosition+Vector3.up*0.25f,0.5f))
                {
                    Vector3 diff = (ledgeDetector.ledgePosition - transform.position);


                    canVault = hits[0] && !hits[1] && !hits[2];
                    canClimb = hits[1] && !hits[2] && ledgeDetector.ledgePosition != Vector3.zero;
                    if(diff.y < maxLedgeJumpHeight)
                        canJumpClimb = hits[2];
                }
                else
                {
                    canVault = false;
                    canClimb = false;
                    canJumpClimb = false;
                }

                //Debug.Log(ColourConsoleText(canVault, "VAULT") + "    " + ColourConsoleText(canClimb, "CLIMB") + "    " + ColourConsoleText(canJumpClimb, "JUMP"));

            }
        }
    }


    // vault
    public void Vault()
    {
        isVaulting = true;

        Vector3 jumpPos = transform.position + transform.up * 1f;
        Vector3 landingPos = transform.position + transform.forward * 0.5f;
        Vector3 distance = landingPos - transform.position;

        _rb.DOJump(landingPos, 0.5f, 1, 0.8f).OnComplete(FinishVault);
        GetComponent<Animator>().SetBool("Vault", true);
        //Invoke(nameof(SetVaultAnimFalse), 0.8f);
    }

    void FinishVault()
    {
        isVaulting = false;
        GetComponent<Animator>().SetBool("Vault", false);
    }

    // climb
    public void Climb()
    {
       
        if(!isClimbing)
        {
            isClimbing = true;

            Vector3 landingPos = ledgeDetector.ledgePosition + (transform.forward * climbForwardAmount + (transform.up * 0.01f));
            _rb.DOJump(landingPos, 0.4f, 1, 0.8f).OnComplete(FinishClimb);

        }

    }

    void FinishClimb()
    {
        isClimbing = false;
        //StartCoroutine(FinishClimb(0.1f));
        //GetComponent<Animator>().SetBool("ClimbUp", false);
    }



    // console text display
    string ColourConsoleText(bool result, string text)
    {
        string newString = "";
        if (result)
        {
            newString = string.Format("<color=#00ff00>{0}</color>", text);
        }
        else
        {
            newString = string.Format("<color=#ff0000>{0}</color>", text);
        }

        return newString;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (ledgeDetector!= null && ledgeDetector.ledgeFound)
        {
            Gizmos.DrawSphere(ledgeDetector.ledgePosition, 0.1f);
        }

    }
}
