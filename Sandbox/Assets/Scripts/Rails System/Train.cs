using UnityEngine;
using System.Linq;


public class Train : MonoBehaviour
{
    // the distance to move along the rail in each step
    private const float TRAIN_INCREMENT_DIST = 0.6f;

    private int segment = -1;
    private Rail rail;
    private bool isConnectedtoRail = false;
    private Vector2 dir;

    [SerializeField] private Rail[] ExludeRails;
    [SerializeField] private float railSeekRange = 0.2f;

    private float percentage;

    private void Start()
    {
        dir = Vector2.right;
    }

    public Vector3 MoveX(float velocityX, float VelocityZ = 0)
    {
        // get the position of the train
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

        GameObject[] railObjects = GameObject.FindGameObjectsWithTag("Rail");

        // check if player is not currenly connected to a rail
        if (!isConnectedtoRail)
        {
            foreach (GameObject railObject in railObjects)
            {
                Rail r = railObject.GetComponent<Rail>();
                if (ExludeRails.Contains(r))
                    continue;
                // rail is within range
                if (r.IsRailWithinRange(pos, railSeekRange, false))
                {
                    rail = r;
                    segment = rail.GetSegmentOfClosestPoint(pos);
                    isConnectedtoRail = true;
                    break;
                }
            }

            // if still not connected to a rail move in direction dir
            if (!isConnectedtoRail)
            {
                return dir.normalized * velocityX;
            }
        }

        // jump rails
        foreach (GameObject railObject in railObjects)
        {
            Rail r = railObject.GetComponent<Rail>();

            // skip if refering to ourselves of the rail has a lower Priority
            if (r == rail || r.Priority < rail.Priority || ExludeRails.Contains(r))
                continue;

            if (r.Priority > rail.Priority)
            {
                // rail is within range
                if (r.IsRailWithinRange(pos, railSeekRange, false))
                {
                    rail = r;
                    segment = rail.GetSegmentOfClosestPoint(pos);
                    break;
                }
            }
            else if (VelocityZ != 0)
            {
                // rail is within range
                if (r.IsRailWithinRange(pos, railSeekRange))
                {
                    // use dot product to ensure key press is in the right direction
                    float dot = Vector3.Dot(Vector3.forward * VelocityZ, r.ClosestPointOnCatmullRom(pos) - pos);
                    if (dot > 0)
                    {
                        rail = r;
                        segment = rail.GetSegmentOfClosestPoint(pos);
                        break;
                    }
                }
            }
        }


        // get percentage of the distance of the player in the current segment
        percentage = rail.ClosestPointOnCatmullRomAsPercent(pos, segment);
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

        if (targetPercentage > 1.0f)
        {
            if (rail.NodeLength - 1 == segment + 1)
            {
                // at the end of the rail disconnect
                targetPercentage = 1.0f;
                isConnectedtoRail = false;
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
                // at the start of the rail disconnect
                targetPercentage = 0.0f;
                isConnectedtoRail = false;
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