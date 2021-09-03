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
    [Range(0.0f, 50.0f)]
    public float followSpeed = 4;
    public Vector3 minFollowDistance;
    public Vector3 panTarget;

    public enum FollowMode { FOLLOWTARGET, CURRENTPLAYER, LOOKATPAN, LOOKATROTATE }
    public FollowMode followMode;

    private bool cinemachineEnabled = true;

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

    // enable cinemachine components
    public void EnableCinemachine()
    {
        cinemachineEnabled = true;
        transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
        transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
    }
    // disable cinemachine componenets
    public void DisableCinemachine()
    {
        cinemachineEnabled = false;
        transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
        transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
    }

    public void SetCinemachineTargetFollow(Vector3 targetPosition)
    {
        transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
        transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
        transform.Find("Camera_target").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
        
        Transform target = transform.Find("Camera_target").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow;
        target.position = targetPosition;
        //transform.Find("Camera_target").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow;
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
        //EnableCinemachine();

        if (GameController.GH.CurrentPlayer() != null)
        {
            if(GameController.GH.CurrentPlayer().GetComponent<ChildControllerRB>() != null)
            {
                transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
                transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
            }
            else if (GameController.GH.CurrentPlayer().GetComponent<GolemControllerRB>() != null)
            {
                transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
                transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
            }

        }
         
            //FollowTarget(GameController.GH.CurrentPlayer().transform);
    }

    // follow currently controlled player
    private void FollowPlayer(Transform targetPlayer)
    {
        //FollowTarget(targetPlayer);
       // GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_Follow = GameController.GH.CurrentPlayer().transform;
    }

    //Start Pan (set position)
    public void PanTo(Vector3 pos)
    {
        //Set State
        followMode = FollowMode.LOOKATPAN;
        panTarget = pos;
    }

    // follow target;
    private void FollowTarget(Transform target)
    {

        if (target == GameController.GH.childObj.transform)
        {
            transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
            transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
        }
        else if (target == GameController.GH.golemObj.transform)
        {
            transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
            transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 1;
        }
        else
        {
            SetCinemachineTargetFollow(target.position);
        }

        /* Vector3 angle = targetPlayer.position - transform.position;
         float distX = Mathf.Abs(angle.x);
         float distY = Mathf.Abs(angle.y);
         float distZ = Mathf.Abs(angle.z);

         Vector3 targetPos = targetPlayer.transform.position;
         targetPos += targetOffset;

         // follow x axis
         if (distX <= minFollowDistance.x && distX >= 0.01f)
         {
             //Go go go
             transform.position = Vector3.MoveTowards(transform.position, targetPos, (followSpeed / 2) * Time.deltaTime);
         }
         else if (distX > minFollowDistance.x)
         {
             //Go go go
             transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
         }
         // follow y axis
         if (distY <= minFollowDistance.y && distY >= 0.1f)
         {
             //Go go go
             transform.position = Vector3.MoveTowards(transform.position, targetPos, (followSpeed / 4) * Time.deltaTime);
         }
         else if (distY > minFollowDistance.y)
         {
             //Go go go
             transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
         }*/
    }

    //pan to location
    private void PanLook(Vector3 targPos)
    {
        //DisableCinemachine();
        SetCinemachineTargetFollow(targPos);

        /*Vector3 angle = targPos - transform.position;
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
        }*/
    }
}
