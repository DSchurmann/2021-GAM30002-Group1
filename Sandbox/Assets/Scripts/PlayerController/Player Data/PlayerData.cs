using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    [Range(0.0f, 10.0f)]
    public float MovementSpeed = 4f;
    [Header("Jump State")]
    [Range(0.0f, 10.0f)]
    public float JumpSpeed = 10f;
    public int jumpsAllowed = 1;
    [Header("In Air State")]
    [Range(0.0f, 10.0f)]
    public float inAirMovementSpeed = 0.2f;
    [Range(0.0f, 1.0f)]
    public float jumpInputMultiplier = 0.5f;
    [Range(0.0f, 1.0f)]
    public float jumpTimeBuff = 0.5f;
    // enable wall slide code in wall state
    [Header("Wall Slide State")]
    [Range(0.0f, 10.0f)]
    public float wallSlideSpeed = 1.5f;
    [Header("Wall Climb State")]
    [Range(-10.0f, 10.0f)]
    public float wallClimbSpeed = 3f;
    [Range(-0.5f, 0.5f)]
    public float wallClimbOffsetPosition = 0.5f;
    [Range(-0.5f, 0.5f)]
    public float wallClimbDistance = 0.3f;
    [Header("Wall Jump State")]
    [Range(0.0f, 10.0f)]
    public float wallJumpSpeed = 4.0f;
    [Range(0.0f, 1.0f)]
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
}
