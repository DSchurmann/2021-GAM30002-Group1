using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    LedgeDetector ledgeDetector;
    ChildHandler controller;

    private string currentState;

    //Animation States
    public string IDLE = "Idle";
    public string WALK = "Walk";
    public string RUN = "Run";
    public string CRAWL = "Crawl";
    public string JUMP= "Jump";
    public string ATTACK = "Attack";
    public string GRABLEDGE = "LedgeHang";
    public string CLIMBLEDGE = "LedgeHangClimb";


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ledgeDetector = GetComponent<LedgeDetector>();
        controller = GetComponent<ChildHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ledgeDetector.EdgeFound)
        {
            float ledgeHeight = Mathf.Round(ledgeDetector.EdgePoint.y - transform.position.y);
            Debug.Log("LEDGE HEIGHT: " + ledgeHeight);
            if (ledgeHeight >= 1 && ledgeHeight < 3)
            {
                Debug.Log("HIGH LEDGE AHEAD: Jump-to-hang");
            }
            if (ledgeHeight == 2)
            {
                Debug.Log("MEDIUM LEDGE AHEAD: Climb-up");
            }
            if (ledgeHeight == 1)
            {
                Debug.Log("SHORT LEDGE AHEAD: Vaul if clear or climb-up");
            }
        }
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
