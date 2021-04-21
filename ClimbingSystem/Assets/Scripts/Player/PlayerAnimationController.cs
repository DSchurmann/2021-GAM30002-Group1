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
    const string CLIMB = "Climb";


    private bool grabLedge;
    private bool hangingOntoLedge;
    private bool climbingLedge;
    private bool climbingWall;

    private Vector3 ledgePoint;
    private Vector3 ledgeForwardNormal;


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
              /*  Vector3 targetPos = transform.localPosition;
                targetPos.y += 2;
                //targetPos.z += 1;
                //MatchTarget(targetPos, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0), 0.01f, 0.08f);

                transform.localPosition = targetPos;*/
                ChangeState(CLIMBLEDGE);
            }
        }

        if (moveController.CharacterController.velocity.magnitude < 0.1f)
        {
            ChangeState(IDLE);
        }
        else
        {
            if(moveController.CharacterController.isGrounded)
            {
                if(moveController.IsWalking)
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
            else
            {
                if(moveController.Jumping)
                {
                    if(ledgeDetector.EdgeFound)
                    {
                        float ledgeHeight = Mathf.Round(ledgeDetector.EdgePoint.y - transform.position.y);
                        if (ledgeHeight == 3)
                        {
                            Debug.Log("HIGH LEDGE AHEAD: Jump-to-hang");

                            RaycastHit hit;
                            if (Physics.Raycast(transform.position, transform.position + transform.TransformDirection(Vector3.forward), out hit, 1))
                            {
                                ledgeForwardNormal = hit.normal;
                            }
                            grabLedge = true;
                           
                        }
                        if (ledgeHeight == 2)
                        {
                            Debug.Log("MEDIUM LEDGE AHEAD: Climb-up");
                        }
                        if (ledgeHeight == 1)
                        {
                            Debug.Log("SHORT LEDGE AHEAD: Climb up (or vault if clear to)");
                        }
                       
                      
                    }
                    else
                    {
                        ChangeState(JUMP);
                    }
                }
            }
        }
        // grab ledge
        if(grabLedge)
        {
            ChangeState(GRABLEDGE);
            ledgePoint = ledgeDetector.EdgePoint;
            moveController.canJump = false;
            ledgeDetector.detectLedges = false;
            moveController.CharacterController.enabled = false;
            moveController.movementEnabled = false;
            moveController.EnableGravity = false;
            //animator.applyRootMotion = true;
           
            //MatchTarget(ledgePoint, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0), 0.01f, 0.02f);

            ledgePoint.y -= 2.36f;
            //ledgePoint.y -= 1.5f;
            transform.DOLocalMove(ledgePoint, 0.1f);
            transform.DOLocalMove(transform.position + transform.TransformDirection(Vector3.forward) * -0.10f, 0.1f);
            var q = Quaternion.FromToRotation(-transform.forward, ledgeForwardNormal);
            transform.rotation = q * transform.rotation;

            hangingOntoLedge = true;
            if (moveController.Jumping)
                moveController.Jumping = false;
        }

        // climb ledge
        if(climbingLedge)
        {
            grabLedge = false;
            hangingOntoLedge = false;
            /* moveController.canJump = false;
             ledgeDetector.detectLedges = false;
             moveController.CharacterController.enabled = false;
             moveController.movementEnabled = false;
             moveController.EnableGravity = false;
             if (moveController.Jumping)
                 moveController.Jumping = false;*/
            // animator.applyRootMotion = false;

           /* Vector3 targetPos = transform.localPosition;
            targetPos.y += 2;
            transform.localPosition = targetPos;*/
            //targetPos.z += 1;
            //MatchTarget(targetPos, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0), 0.01f, 0.08f);


            Vector3 targetPos = transform.localPosition;
            targetPos.y += 2;
            //targetPos.z += 1;
            //MatchTarget(targetPos, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0), 0.01f, 0.08f);

            //transform.localPosition = targetPos;

            //transform.DOLocalMove(targetPos, 0.5f).OnComplete(FinishClimbUpLedge);
            transform.DOLocalMove(targetPos + transform.TransformDirection(Vector3.forward)*0.001f, 0.2f).OnComplete(FinishClimbUpLedge);
            climbingLedge = false;
        }
    }

    public void GrabLedge()
    {
        grabLedge = true;
    }

    public void FinishClimbUpLedge()
    {
        climbingLedge = false;
        moveController.canJump = true;
        grabLedge = false;
        ChangeState(IDLE);
        ledgeDetector.detectLedges = true;
        moveController.CharacterController.enabled = true;
        moveController.movementEnabled = true;
        moveController.EnableGravity = true;
    }
    public void DropFromLedge()
    {
        moveController.canJump = true;
        grabLedge = false;
        ChangeState(IDLE);
        ledgeDetector.detectLedges = true;
        moveController.CharacterController.enabled = true;
        moveController.movementEnabled = true;
        moveController.EnableGravity = true;
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
