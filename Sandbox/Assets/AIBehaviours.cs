using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviours : MonoBehaviour
{
    AIController owner;

    public int targetSide;


    private void Start()
    {
        owner = GetComponent<AIController>();
    }

    private void Update()
    {
        
    }
    public bool InSightRange(Transform target)
    {
        float dist = Vector3.Distance(owner.gameObject.transform.position, target.position);
        if (dist <= owner.sightRange)
        {
            //Debug.DrawLine(owner.gameObject.transform.position, target.transform.position, Color.white);
            return true;
        }
        return false;
    }
    // return true in in sight and nothing inbwteen target
    public bool InSight(Transform target)
    {
        if (InSightRange(target))
        {

            Debug.DrawLine(owner.gameObject.transform.position, target.transform.position, Color.white);
            RaycastHit hit;
            Vector3 dir;

            dir = owner.Head.transform.position - target.position;

            Vector3 origin = owner.Head.transform.position;
            Vector3 dest = target.position;

            if (Physics.Linecast(origin, dest, out hit))
            {
                if (hit.collider)
                {
                    if (hit.collider.gameObject.CompareTag(target.gameObject.tag))
                    {
                        Debug.DrawLine(origin, dest, Color.green);
                        return true;
                    }
                    else
                    {
                        Debug.DrawLine(origin, dest, Color.yellow);
                        return false;
                    }
                }
            }
            else
            {
                Debug.DrawLine(origin, dest, Color.red);
            }

            return false;
        }
        return false;
    }

    // true if target is in field of view                     
    public bool InFOV(GameObject target)
    {
        Vector3 targetDir = target.transform.position - owner.transform.position;
        float angleToPlayer_letRight = (Vector3.Angle(targetDir, owner.transform.forward));
        float angleToPlayer_upDown = (Vector3.Angle(targetDir, owner.transform.up));
        Vector3 origin = owner.Head.transform.position;

        Vector3 dest = target.transform.position;

        //Debug.Log("angleToPlayer_upDown = " + angleToPlayer_upDown);
        //Debug.Log("angleToPlayer_letRight = " + angleToPlayer_letRight);
        if (angleToPlayer_upDown >= -20 && angleToPlayer_upDown <= 180) // FOV U to D
        {
            //Debug.Log("Target in FOV y");
             //return true;
            if (angleToPlayer_letRight >= -90 && angleToPlayer_letRight <= 90) //FOV L to R
            {
                Debug.DrawLine(origin, dest, Color.green);
                //Debug.Log("Target in FOV");
                if(angleToPlayer_letRight < 0)
                {
                    targetSide = 0;
                }
                else if (angleToPlayer_letRight >= 0)
                {
                    targetSide = 1;
                }
                return true;
            }
        }
        //Debug.DrawLine(origin, dest, Color.red);
        return false;
    }
}
