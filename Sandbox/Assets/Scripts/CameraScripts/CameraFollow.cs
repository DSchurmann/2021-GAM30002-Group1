using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Target")]
    public Transform target;
    [Header("Camera Target Offset")]
    public Vector3 targetOffset;
    [Header("Follow Properties")]
    [Range(0.0f, 50.0f)]
    public float followSpeed = 4f;
    public Vector3 minFollowDistance;
    public Vector3 panTarget;
    [SerializeField] private float MaxRotateAngle = 25f;
    [SerializeField] private float RotateSpeed = 4f;

    private bool IsShaking = false;

    public Cinemachine.CinemachineVirtualCamera child_cam;
    public Cinemachine.CinemachineVirtualCamera golem_cam;
    public Cinemachine.CinemachineVirtualCamera target_cam;

    public enum FollowMode { FOLLOWTARGET, CURRENTPLAYER, LOOKATPAN, LOOKATROTATE }
    public FollowMode followMode;

    private bool cinemachineEnabled = true;

    //Start
    private void Start()
    {
        followMode = FollowMode.CURRENTPLAYER;

        // find cameras 
        if (child_cam == null)
            child_cam = transform.Find("Camera_child").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        if (golem_cam == null)
            golem_cam = transform.Find("Camera_golem").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        if (target_cam == null)
            target_cam = transform.Find("Camera_target").GetComponent<Cinemachine.CinemachineVirtualCamera>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.GH.GamePaused)
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

            RotateCamera();

        }
    }

    private void RotateCamera()
    {
        PlayerControllerRB curPlayer = GameController.GH.CurrentPlayer();
        //float dir = Mathf.Sign(curPlayer.CurrentVelocity.x);
        float dir = curPlayer.FacingDirection;
        if (GameController.GH.GetComponent<Director>().inCutscene)
        {
            dir = 0;
        }
        //Vector3 curRotation = transform.rotation.eulerAngles;
        //Vector3 newRotation = new Vector3(0f, Mathf.Lerp(curRotation.y, MaxRotateAngle * dir, 0.5f * Time.deltaTime), 0f);
        Quaternion targetRotaion = Quaternion.Euler(transform.rotation.x, transform.rotation.y + (MaxRotateAngle * dir), transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotaion, RotateSpeed * Time.deltaTime);
    }

    // enable cinemachine components
    public void EnableCinemachine()
    {
        cinemachineEnabled = true;
        child_cam.enabled = true;
        golem_cam.enabled = true;
    }
    // disable cinemachine componenets
    public void DisableCinemachine()
    {
        cinemachineEnabled = false;
        child_cam.enabled = false;
        golem_cam.enabled = false;
    }

    public void SetCinemachineTargetFollow(Vector3 targetPosition)
    {
        child_cam.Priority = 0;
        golem_cam.Priority = 0;
        target_cam.Priority = 1;
        
        Transform target = target_cam.Follow;
        target.position = targetPosition;
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
    }

    //pan to location
    private void PanLook(Vector3 targPos)
    {
        SetCinemachineTargetFollow(targPos);
    }

    public void Shake(float intesity, float time)
    {
        if (IsShaking)
        {
            CancelInvoke("StopShake");
        }
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinChild = child_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(cinemachineBasicMultiChannelPerlinChild);
        cinemachineBasicMultiChannelPerlinChild.m_AmplitudeGain = intesity;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinGolem = golem_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlinGolem.m_AmplitudeGain = intesity;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinTarget = target_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlinTarget.m_AmplitudeGain = intesity;
        IsShaking = true;

        Invoke("StopShake", time);
    }

    private void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinChild = child_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlinChild.m_AmplitudeGain = 0f;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinGolem = golem_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlinGolem.m_AmplitudeGain = 0f;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlinTarget = target_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlinTarget.m_AmplitudeGain = 0f;
        IsShaking = false;
    }
}

