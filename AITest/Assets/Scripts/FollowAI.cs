using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FollowAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float minDist = 5f;
    private float moveSpeed = 5f;
    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //create look rotation for looking at the target (child)
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
        //get only the y rotation
        Quaternion newRot = Quaternion.Euler(0f, lookRot.eulerAngles.y, 0f);
        //change the object rotation with Slerp (spherical linear interpolation, thanks Google), this rotates the object gradually, or interpolates, instead of snapping to a rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 10f * Time.deltaTime);


        //get target heading
        Vector3 offset = target.position - transform.position;

        //if target distance is further than the min distance...
        if(offset.magnitude > minDist)
        {
            //change heading to move vector
            offset = offset.normalized * moveSpeed;
            //move object towards target
            cc.Move(offset * Time.deltaTime);
        }
    }
}
