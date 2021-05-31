﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Train : MonoBehaviour
{
    // the distance to move along the rail in each step
    private const float TRAIN_INCREMENT_DIST = 0.6f;

    [SerializeField] private int segment = 3;
    [SerializeField] private Rail rail;

    private float percentage;

    public Vector3 MoveX(float velocityX)
    {
        // get the position of the train
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        // get percentage of the distance of the player in the current segment
        percentage = rail.ClosestPointOnCatmullRom(pos, segment);
        // get the position of the train along the segment
        Vector3 catmullP = rail.CatmullMove(segment, percentage);

        // debug in editor
        Debug.DrawLine(catmullP, pos, Color.red);
        Debug.DrawLine(transform.position, pos, Color.red);
        Debug.DrawLine(catmullP, transform.position, Color.red);

        // Calculate the percentage to increment, so that the amount is the same in all segments
        float dist = rail.GetSegmentLength(segment);
        float incremenmtAmount = TRAIN_INCREMENT_DIST / dist;

        // move the target
        float targetPercentage = percentage + Mathf.Sign(velocityX) * incremenmtAmount;
        Debug.Log("test");
        Debug.Log(percentage);
        Debug.Log(targetPercentage);

        if (targetPercentage > 1.0f)
        {
            if (rail.NodeLength - 1 == segment + 1)
            {
                targetPercentage = 1.0f;
            }
            else
            {
                segment += 1;
                targetPercentage -= 1;
            }
        }
        else if (targetPercentage < 0.0f)
        {
            if (0 == segment)
            {
                targetPercentage = 0.0f;
            }
            else
            {
                segment -= 1;
                targetPercentage += 1;
            }
        }

        Vector3 targetPos = rail.CatmullMove(segment, targetPercentage);
        Debug.DrawLine(catmullP, targetPos, Color.green);
        Vector3 offset = targetPos - pos;
        offset = offset.normalized * Mathf.Abs(velocityX);

        Vector3 move = new Vector3(offset.x, 0, offset.z);

        return move;
    }
}