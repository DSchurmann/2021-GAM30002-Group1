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
    public float minDistanceToGap;
    public float gapCheckDepth;

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
    protected float climbForwardAmount = 0.25f;
    //public float MaxDistanceToLedge_WallClimb;

    // ground properties
    public Vector3 groundAngle;
    public float maxSlopeAngle;


    // components
    public Rigidbody _rb;
    public PlayerInputHandler InputHandler { get; protected set; }

    protected bool inputInteract;

  
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
        inputInteract = InputHandler.InputInteract;

        if (ledgeDetector != null && isEnabled)
        {
            isTouchingWall = ledgeDetector.TouchingWall().Any();
            isGapAhead = ledgeDetector.GapCheck(transform.position + (transform.forward * origins_forward[0]), gapCheckDepth);
            groundAngle = ledgeDetector.GroundCheck(transform.position + Vector3.up * 0.2f , gapCheckDepth);
            ledgeFound = ledgeDetector.ledgeFound;
        }
        else
        {
            ledgeFound = false;
            isTouchingWall = false;
        }
       
        if(groundAngle.x != 0)
        {
            Debug.Log("Ground angle: " + groundAngle);
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
                    canClimb = hits[1] && !hits[2];
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
        isClimbing = true;

        Vector3 landingPos = ledgeDetector.ledgePosition + (transform.forward * climbForwardAmount); //+ (transform.up * 0.5f);
        Vector3 distance = landingPos - transform.position;

        //GetComponent<CharacterController>().Move(distance);
        _rb.DOMove(landingPos, 0.5f).OnComplete(FinishClimb);
        //GetComponent<Animator>().SetBool("ClimbUp", true);
    }

    public IEnumerator ClimbUp()
    {
        float time = 0;
        int secs = 0;
        int max = 3;
        while(secs < max)
        {
            time += Time.deltaTime;

            secs = (int)time % 60;
            Debug.Log(secs);
        }

        yield return null;
    }

    void FinishClimb()
    {
        isClimbing = false;
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
