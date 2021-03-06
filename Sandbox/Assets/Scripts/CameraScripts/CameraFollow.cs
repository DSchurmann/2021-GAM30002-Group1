using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Target")]
    public Transform target;
    [Header("Camera Target Offset")]
    public Vector3 targetOffset;
    [Header("Follow Properties")]
    [Range(0.0f, 10.0f)]
    public float followSpeed = 4;
    public Vector3 minFollowDistance;
    public Vector3 panTarget;

    public enum FollowMode { FOLLOWTARGET, CURRENTPLAYER, LOOKATPAN, LOOKATROTATE }
    public FollowMode followMode;

    //Start
    private void Start()
    {
        followMode = FollowMode.CURRENTPLAYER;
    }

    // Update is called once per frame
    void Update()
    {

        switch (followMode)
        {
            case FollowMode.CURRENTPLAYER:
                FollowCurrentPlayer();
                break;

            case FollowMode.FOLLOWTARGET:
                if (target)
                    FollowTarget(target);
                break;

            case FollowMode.LOOKATPAN:
                PanLook(panTarget);
                break;
            case FollowMode.LOOKATROTATE:

                break;
        }

    }

    //Set PlayerFollow Mode
    public void FollowToPlayer()
    {
        //Set FollowMode
        followMode = FollowMode.CURRENTPLAYER;
    }

    // follow currently controlled player
    private void FollowCurrentPlayer()
    {
        if (GameController.GH.CurrentPlayer() != null)
            FollowTarget(GameController.GH.CurrentPlayer().transform);
    }

    // follow currently controlled player
    private void FollowPlayer(Transform targetPlayer)
    {
        FollowTarget(targetPlayer);
    }

    //Start Pan (set position)
    public void PanTo(Vector3 pos)
    {
        //Set State
        followMode = FollowMode.LOOKATPAN;
        panTarget = pos;
    }

    // follow target;
    private void FollowTarget(Transform targetPlayer)
    {
        Vector3 angle = targetPlayer.position - transform.position;
        float distX = Mathf.Abs(angle.x);
        float distY = Mathf.Abs(angle.y);
        float distZ = Mathf.Abs(angle.z);

        // follow x axis
        if (distX <= minFollowDistance.x && distX >= 0.01f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, (followSpeed / 2) * Time.deltaTime);
        }
        else if (distX > minFollowDistance.x)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, followSpeed * Time.deltaTime);
        }
        // follow y axis
        if (distY <= minFollowDistance.y && distY >= 0.1f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, (followSpeed / 4) * Time.deltaTime);
        }
        else if (distY > minFollowDistance.y)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, followSpeed * Time.deltaTime);
        }
    }

    //pan to location
    private void PanLook(Vector3 targPos)
    {
        Vector3 angle = targPos - transform.position;
        float distX = Mathf.Abs(angle.x);
        float distY = Mathf.Abs(angle.y);
        float distZ = Mathf.Abs(angle.z);

        // follow x axis
        if (distX <= minFollowDistance.x && distX >= 0.01f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targPos, (followSpeed / 2) * Time.deltaTime);
        }
        else if (distX > minFollowDistance.x)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targPos, followSpeed * Time.deltaTime);
        }
        // follow y axis
        if (distY <= minFollowDistance.y && distY >= 0.1f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targPos, (followSpeed / 4) * Time.deltaTime);
        }
        else if (distY > minFollowDistance.y)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targPos, followSpeed * Time.deltaTime);
        }
    }
}
