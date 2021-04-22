using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IKSnap : MonoBehaviour
{
    public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;

    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 leftHandOffset;
    public Vector3 rightHandOffset;

    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    public bool LTargetReached;
    public bool RTargetReached;
    private float minDistToTarget;

    //public Transform EdgeDetector_L;
    //public Transform EdgeDetector_R;

    private Animator animator;

    public float xRot = 0.0f;
    public float yRot = 0.0f;
    public float zRot = 0.0f;

    public Collider characterCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        characterCollider = GetComponent<Collider>();
        minDistToTarget = 0.35f;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        float lengthForward1 = 0.5f;
        float lengthForward2 = 0.5f;
        float lengthDown = 6;

        Vector3 origin1 = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y/2, 0.0f));
        Vector3 dir1 = transform.TransformDirection(Vector3.forward);

        ///Vector3 origin2 = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, lengthForward));
        Vector3 dir2 = Vector3.down;

        RaycastHit hit1;
        RaycastHit hit2;

        // forward ray
        Debug.DrawRay(origin1, dir1 * lengthForward1, Color.green);
        // left ledge ray
        //Debug.DrawRay(origin2 + transform.TransformDirection(new Vector3(-0.1f, 0.0f, 0.0f)), dir2, Color.green);
        // right ledge ray
        //Debug.DrawRay(origin2 + transform.TransformDirection(new Vector3(0.1f, 0.0f, 0.0f)), dir2, Color.green);

        if (Physics.Raycast(origin1, dir1, out hit1, lengthForward1))
        {
            lengthForward2 = 0.25f;
            Vector3 origin2 = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, lengthForward2));
            float rayDistOffset_Z = 0.2f;
            for (int i = 0; i < 3; i++)
            {
                Vector3 origin3 = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, lengthForward2));
                lengthForward2 += rayDistOffset_Z;
                // left ledge rays
                Debug.DrawRay(origin3 + transform.TransformDirection(new Vector3(-0.1f, 0.0f, 0.0f)), dir2, Color.green);
                // right ledge rays
                Debug.DrawRay(origin3 + transform.TransformDirection(new Vector3(0.1f, 0.0f, 0.0f)), dir2, Color.green);
            }
            // left hand edge position
            if (Physics.Raycast(origin2 + transform.TransformDirection(new Vector3(-0.1f, 0.0f, 0.0f)), dir2, out hit2, lengthDown))
            {
                //Debug.DrawRay(origin2 + transform.TransformDirection(new Vector3(-0.2f, 0.0f, 0.0f)), dir2 + transform.TransformDirection(new Vector3(-0.1f, 0.0f, 0.0f)), Color.green);

                Vector3 EdgeL = new Vector3(hit2.point.x, hit2.point.y, hit1.point.z);

                Debug.DrawLine(origin1, EdgeL, Color.cyan);


                Vector3 lookAt = Vector3.Cross(-hit2.normal, transform.right);
                lookAt = lookAt.y < 0 ? -lookAt : lookAt;
                //Setting true if raycast hits something
                leftHandIK = true;
                //Setting leftHandPos to raycast hit points and subtracting the offsets
                leftHandPos = EdgeL - transform.TransformDirection(leftHandOffset);
                //leftHandRot = Quaternion.FromToRotation(Vector3.forward, LHit.normal);
                leftHandRot = Quaternion.LookRotation(dir1, hit2.normal);
            }
            else
            {
                leftHandIK = false;
            }


            // right hand edge position
            if (Physics.Raycast(origin2 + transform.TransformDirection(new Vector3(0.1f, 0.0f, 0.0f)), dir2, out hit2, lengthDown))
            {
                //Debug.DrawRay(origin2 + transform.TransformDirection(new Vector3(0.2f, 0.0f, 0.0f)), dir2 + transform.TransformDirection(new Vector3(0.1f, 0.0f, 0.0f)), Color.green);

                Vector3 EdgeR = new Vector3(hit2.point.x, hit2.point.y, hit1.point.z);

                Debug.DrawLine(origin1, EdgeR, Color.cyan);


                Vector3 lookAt = Vector3.Cross(-hit2.normal, transform.right);
                lookAt = lookAt.y < 0 ? -lookAt : lookAt;
                //Setting true if raycast hits something
                rightHandIK = true;
                //Setting leftHandPos to raycast hit points and subtracting the offsets
                rightHandPos = EdgeR - transform.TransformDirection(rightHandOffset);
                //leftHandRot = Quaternion.FromToRotation(Vector3.forward, LHit.normal);
                rightHandRot = Quaternion.LookRotation(dir1, hit2.normal);
            }
            else
            {
                rightHandIK = false;
            }

        }
        else
        {
            leftHandIK = false;
            rightHandIK = false;
        }
    }

    void OnDrawGizmos()
    {

       /* //Left Hand IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.5f, 0.5f)), transform.TransformDirection(new Vector3(-0.25f, -1.0f, 0.0f)), Color.green);

        //Right Hand IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.5f, 0.5f)), transform.TransformDirection(new Vector3(0.25f, -1.0f, 0.0f)), Color.green);*/

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            if (leftHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);

                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);

                if((Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.LeftHand),leftHandPos) < minDistToTarget))
                {
                    LTargetReached = true;
                }
                else
                {
                    LTargetReached = true;
                }
              
            }

            if (rightHandIK)
            {
                if ((Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandPos) < minDistToTarget))
                {
                    RTargetReached = true;
                }
                else
                {
                    RTargetReached = true;
                }
                
                //Debug.Log("\nL : " + Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.LeftHand), leftHandPos) + "     R: " + Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandPos));
                Debug.Log("\nR reached : " + (Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.RightHand), rightHandPos)<minDistToTarget));
                                
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);

                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
            }
        }
    }

   

}
