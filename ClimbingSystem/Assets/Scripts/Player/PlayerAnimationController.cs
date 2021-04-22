using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    PlayerMove moveController;
    LedgeDetector ledgeDetector;

    private string currentState;

    //Animation States
    const string IDLE = "Idle";
    const string WALK = "Walk";
    const string RUN = "Run";
    const string CRAWL = "Crawl";
    const string JUMP= "Jump";
    const string ATTACK = "Attack";
    const string GRABLEDGE = "LedgeHang";
    const string CLIMBLEDGE = "LedgeHangClimb";
    const string CLIMBUP = "ClimbUp";
    const string VAULT = "Vault";


    private bool doVault;
    private bool vaultLedge;
    private bool grabLedge;
    private bool climbShortLedge;
    private bool hangingOntoLedge;
    private bool climbingLedge;

    private Vector3 ledgePoint;
    private Vector3 ledgeForwardNormal;

    private bool performingAction;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<PlayerMove>();
        ledgeDetector = GetComponent<LedgeDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hangingOntoLedge)
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                DropFromLedge();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                climbingLedge = true;
            }
        }

        if (moveController.CharacterController.velocity.magnitude < 0.1f)
        {
            if(!performingAction)
                ChangeState(IDLE);
        }
        else
        {
            if(moveController.CharacterController.isGrounded)
            {
                if(moveController.movementEnabled)
                {
                    if (moveController.IsWalking)
                    {
                        ChangeState(WALK);
                    }
                    else if (moveController.IsRunning)
                    {
                        ChangeState(RUN);
                    }
                    else if (moveController.IsCrawling)
                    {
                        ChangeState(CRAWL);
                    }
                }
            }
        }

        if (moveController.JumpPressed)
        {
            if (moveController.canJump )
            {
                if(moveController.Jumping)
                    ChangeState(JUMP);
            }
        }

        if (DetectLedges() != -1)
        {
            moveController.canJump = false;
            
            switch(DetectLedges())
            {
                case 1: // vault small ledges
                    Debug.Log("SHORT LEDGE AHEAD: Climb up or vault");
                    if (moveController.JumpPressed && moveController.CharacterController.isGrounded)
                    {
                       
                    }
                    if (!vaultLedge)
                    {
                        vaultLedge = true;
                    }
                    break;

                case 2: // climbup medium ledges
                    Debug.Log("MEDIUM LEDGE AHEAD: Climb-up");
                    if (moveController.JumpPressed && moveController.CharacterController.isGrounded)
                    {
                        if (!climbShortLedge)
                        {
                            climbShortLedge = true;
                        }
                    }
                    break;

                case 3: // jump to and hang from tall ledges
                    Debug.Log("HIGH LEDGE AHEAD: Jump-to-hang");

                    if (moveController.JumpPressed && moveController.CharacterController.isGrounded)
                    {
                        if (!grabLedge)
                        {
                            grabLedge = true;
                        }
                    }
                    else
                    {
                        if(moveController.Falling)
                        {
                            if (!grabLedge)
                            {
                                grabLedge = true;
                            }
                        }
                    }
                   
                    break;

            }

            //doVault = true;
        }
        else
        {
            moveController.canJump = true;
            //doVault = false;
        }

        // vault short ledges (needs to check if cler first)
        VaultLedges();
        // climb medium ledges
        ClimbUpShortLedges();

        GrabLedges();
        ClimbUpLedges();
     

    }


    public int DetectLedges()
    {
        if(ledgeDetector.EdgeFound && !performingAction)
        {
            // get forward hit normal
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.position + transform.TransformDirection(Vector3.forward), out hit, 1))
            {
                ledgeForwardNormal = hit.normal;
            }

            float ledgeHeight = Mathf.Round(ledgeDetector.EdgePoint.y - transform.position.y);
            return (int)ledgeHeight;
        }

        return -1;
    }

    public void ClimbUpShortLedges()
    {
        if (climbShortLedge)
        {
            StartClimbingAction();

            grabLedge = false;
            hangingOntoLedge = false;
            ChangeState(CLIMBUP);
            /*Vector3 targetPos = transform.position + transform.forward*1;
            targetPos.y += 0.5f;*/

            Vector3 targetPos = transform.position + transform.forward * 0.1f;
            targetPos.y += 0.5f;

            transform.DOMove(targetPos, 0.5f).OnComplete(FinishClimbLedge);
        }
    }
    public void VaultLedges()
    {
        if(vaultLedge)
        {
            StartClimbingAction();

            grabLedge = false;
            hangingOntoLedge = false;
         
            ChangeState(CLIMBUP);

            Vector3 landingPos = transform.position + transform.forward * 0.4f;

            transform.DOJump(landingPos, 0.1f, 1, 0.5f).OnComplete(FinishVaultLedge);
        }
    }

    // climb up when hanging from ledge
    public void ClimbUpLedges()
    {
        if (climbingLedge)
        {
            grabLedge = false;
            hangingOntoLedge = false;

            ChangeState(CLIMBLEDGE);

            Vector3 targetPos = transform.localPosition;
            targetPos.y += 3;
            targetPos.z += 0.5f;
            transform.DOLocalMoveY((targetPos.y), 0.5f);
            transform.DOLocalMoveZ((transform.position + transform.TransformDirection(Vector3.forward)).z, 0.25f).OnComplete(FinishClimbUpLedge);
            //transform.DOLocalMove(targetPos + transform.TransformDirection(Vector3.forward) * 0.001f, 1.3f).OnComplete(FinishClimbUpLedge);
            climbingLedge = false;
        }
    }

    // grab and hand from ledges
    public void GrabLedges()
    {
        // grab ledges
        if (grabLedge)
        {
            hangingOntoLedge = true;

            /* Quaternion q = Quaternion.FromToRotation(-transform.forward, ledgeForwardNormal);
             transform.rotation = q * transform.rotation;*/

            StartClimbingAction();

            ChangeState(GRABLEDGE);

            // define ledge point and move to it

            ledgePoint.y -= 2.36f;
            //ledgePoint.y -= 1.5f;
            transform.DOLocalMove(ledgePoint, 0.1f);
            transform.DOLocalMove(transform.position + transform.TransformDirection(Vector3.forward) * -0.10f, 0.1f);
          
            if (moveController.Jumping)
                moveController.Jumping = false;
        }
    }
    public void FinishVaultLedge()
    {
        EndClimbingAction();
    }
    public void FinishClimbUpLedge()
    {
        EndClimbingAction();
    }
    public void DropFromLedge()
    {
        EndClimbingAction();
    }
    public void FinishClimbLedge()
    {
        EndClimbingAction();
    }
    public void StartClimbingAction()
    {
        Debug.Log("CLIMBING ACTION STARTED");
        performingAction = true;
        ledgePoint = ledgeDetector.EdgePoint;
        moveController.canJump = false;
        ledgeDetector.detectLedges = false;
        moveController.CharacterController.enabled = false;
        moveController.movementEnabled = false;
        moveController.EnableGravity = false;
    }

    public void EndClimbingAction()
    {
        Debug.Log("CLIMBING ACTION ENDED");
        ledgeDetector.detectLedges = true;
        performingAction = false;
        doVault = false;
        vaultLedge = false;
        climbingLedge = false;
        climbShortLedge = false;
        grabLedge = false;

        moveController.CharacterController.enabled = true;
        moveController.movementEnabled = true;
        moveController.EnableGravity = true;
        moveController.canJump = true;

        if (currentState != IDLE)
            ChangeState(IDLE);
    }


    public void ChangeState(string newState)
    {
        // stop same animation from interrupting
        if (currentState == newState) return;

        // play animation state
        animator.Play(newState);

        // set new current state
        currentState = newState;
    }

    public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
    {
        if (animator.IsInTransition(0)) return;
        if (animator.isMatchingTarget) return;

        float normalizeTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);

        if (normalizeTime > normalisedEndTime)
        {
            return;
        }

        animator.MatchTarget(matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
    }
}
