using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform Player;
    public Transform target;
    private bool performing;
    public bool following;

    public Transform Head;
    public float sightRange;
    public float pointSpotRange;
    public GameObject AimTarget;
    private bool pointTargetSpotted;
    private bool pointing;
    

    public float minFollowDistance;

    private NavMeshAgent nav;
    public Animator animator;

    AIBehaviours behaviour;

    public Transform Shoulder;

    public Transform RightTarget;
    public Transform LeftTarget;
    public Transform RightElbow;
    public Transform LeftElbow;
    
    public Transform idleRightTarget;
    public Transform idleLeftTarget;
    public Transform idleRightElbow;
    public Transform idleLeftElbow;

    // Start is called before the first frame update
    void Start()
    {
        behaviour = GetComponent<AIBehaviours>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = 2;

        following = false;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMovement();

        HandleFollowing();

        HandlePointing();

        HandleInterestPoints();

        //bool playerInSight = behaviour.InSight(Player);

    }

    public void HandleInterestPoints()
    {
        int result = GameObject.Find("InterestPoints").transform.childCount;
        List<Transform> POIs = new List<Transform>();
        if (result>0)
        {
            
            foreach (Transform child in GameObject.Find("InterestPoints").transform)
            {
                if (child.tag == "POI")
                {
                    POIs.Add(child.gameObject.transform);
                }
            }
            target = GetClosest(POIs.ToArray());
        }
    }

    public void HandleFollowing()
    {
        if (following)
        {
            if (Distance(Player) > minFollowDistance)
            {
                nav.enabled = true;
                nav.SetDestination(Player.position);

            }
            else
            {
                nav.enabled = false;
            }
        }
    }

    public Transform GetClosest(Transform[] list)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        foreach(var item in list)
        {
            var distanceFromTarget = Vector3.Distance(Head.position, item.position);
            if(distanceFromTarget<closestDistance)
            {
                closest = item;
                closestDistance = distanceFromTarget;
            }
        }
        return closest;
    }

    public void DisableNav()
    {
        nav.isStopped = true;
        nav.enabled = false;
    }
    public void EnableNav()
    {
        nav.enabled = true;
        nav.isStopped = false;
    }
    public void StopFollowing()
    {
        if (following)
        {
            following = false;
            nav.enabled = false;
        }
    }

    // handle pointing
    private void HandlePointing()
    {
        if (target && behaviour.InFOV(target.gameObject))
        {
            
            if (!pointTargetSpotted)
            {
                Debug.Log("Target in sight");
                pointTargetSpotted = true;
            }
        }
        else
        {
            if (pointTargetSpotted)
            {
                Debug.Log("Target not in sight");
                pointTargetSpotted = false;
            }
        }

        // perform if target available
       /* if (target)
        {
            Debug.Log("Dist: " + Distance(target));
        }*/

        if (pointTargetSpotted && (Distance(target) <= pointSpotRange || DistanceFromHead(target) <= pointSpotRange))
        {
            pointing = true;
        }
        else
        {
            pointing = false;
        }

        // handle pointing side
        if (pointing && transform.InverseTransformPoint(target.transform.position).x < 0)
        {
            LeftTarget.position = target.transform.position;
        }
        else
        {
            LeftTarget.position = idleLeftTarget.position;
            LeftElbow.position = idleLeftElbow.position;
        }

        if (pointing && transform.InverseTransformPoint(target.transform.position).x >= 0)
        {
            RightTarget.position = target.transform.position;
        }
        else
        {
            RightTarget.position = idleRightTarget.position;
            RightElbow.position = idleRightElbow.position;
        }
    }

    public void IdleHands()
    {
        LeftTarget.position = idleLeftTarget.position;
        LeftElbow.position = idleLeftElbow.position;
        RightTarget.position = idleRightTarget.position;
        RightElbow.position = idleRightElbow.position;
    }

    public float Distance(Transform other)
    {
        return Vector3.Distance(other.position, transform.position);
    }

    public float DistanceFromHead(Transform other)
    {
        return Vector3.Distance(other.position, Head.position);
    }

    private void AnimateMovement()
    {
        animator.SetFloat("Velocity", nav.velocity.magnitude);
    }

    public void SetNavTarget(Vector3 pos)
    { 
        if(following)
        {
            following = false;
            //Debug.Log("STOPPED FOLLOWING");
        }
        nav.enabled = true;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, -1))
        {
            //Debug.Log("RECEIVED NAVIGATION COMMAND");
            //Debug.Log("Nav hit: " + myNavHit.position);
            nav.SetDestination(myNavHit.position);
        }
    }
}
